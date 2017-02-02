using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Ensage;
using Ensage.Common;
using Ensage.Common.Menu;
using Ensage.Common.Extensions;
using Ensage.Common.AbilityInfo;
using Ensage.Common.Objects;
using SharpDX;

namespace PudgePRO
{
    internal class PudgePRO : Variables
    {
        public static void Init()
        {
            Options.MenuInit();

            Events.OnLoad += OnLoad;
            Events.OnClose += OnClose;
        }

        private static void OnClose(object sender, EventArgs e)
        {
            Game.OnUpdate -= FullCombo;
            Drawing.OnDraw -= TargetIndicator;
            loaded = false;
            me = null;
            target = null;
            //playerList = new List<Hero>();
        }

        private static void OnLoad(object sender, EventArgs e)
        {
            if (!loaded)
            {
                me = ObjectManager.LocalHero;
                //playerList = new List<Hero>();

                if (!Game.IsInGame || me == null || me.Name != heroName)
                {
                    return;
                }

                loaded = true;
                Game.PrintMessage(
                    "<font face='Calibri Bold'><font color='#04B404'>" + AssemblyName +
                    " loaded.</font> (coded by <font color='#0404B4'>DeadShotNinja</font>) v" + Assembly.GetExecutingAssembly().GetName().Version,
                    MessageType.LogMessage);
                GetAbilities();
                Game.OnUpdate += FullCombo;
                Drawing.OnDraw += TargetIndicator;
            }

            if (me == null || !me.IsValid)
            {
                loaded = false;
            }
        }

        private static void FullCombo(EventArgs args)
        {
            if (!Game.IsInGame || Game.IsPaused || Game.IsWatchingGame || Game.IsChatOpen)
                return;

            // Selects target closest to mouse using user selected range from menu.
            //playerList = Heroes.All;
            target = me.ClosestToMouseTarget(ClosestToMouseRange.GetValue<Slider>().Value);
            allyTarget = ClosestToMouseAlly(me);

            if (Utils.SleepCheck("PudgePROrotatedCheckSleep"))
            {
                //Game.PrintMessage("Target Rotated: " + TargetRotated(), MessageType.LogMessage);
                targetRotate = TargetRotated();

                Utils.Sleep(comboSleepGet, "PudgePROrotatedCheckSleep");
            }

            //rotToggled = me.HasModifier("modifier_pudge_rot") == true ? true : false;

            //if (hook != null && hook.IsInAbilityPhase && targetRotate)
            //{
            //    Game.PrintMessage("I WORK NOW", MessageType.LogMessage);
            //    me.Stop();
            //}

            if (hook != null && target != null && hookPredictRad.GetValue<bool>()) GetCastSkillShotEnemy(hook, target, "pudge_meat_hook", soulring, true);

            //foreach (var unit in playerList)
            //{
            //    if (unit.Team == me.Team && unit.IsAlive && !unit.IsIllusion)
            //    {
            //        allyTarget = TargetSelector.ClosestToMouse(me).;//unit;
            //    }
            //}
            //var allyTarget = TargetSelector.ClosestToMouse(me).Equals(ally);

            //var targetFacingLocationB = target.InFront(1000);
            //var targetFacingAngleB = target.Position.ToVector2().FindAngleBetween(targetFacingLocationB.ToVector2(), true);
            //targetFacing = targetFacingAngleB;

            //if (Utils.SleepCheck("PudgePROGetAbilities")) GetAbilities();

            //if (hook != null && hook.IsInAbilityPhase)// && Utils.SleepCheck("PudgePROcastSleep"))
            //{
            //    var targetFacingLocationA = target.InFront(1000);
            //    var targetFacingAngleA = target.Position.ToVector2().FindAngleBetween(targetFacingLocationA.ToVector2(), true);

            //    Game.PrintMessage("targetFacing: " + targetFacing + "targetFacingAngleA: " + targetFacingAngleA, MessageType.LogMessage);

            //    if (targetFacing >= targetFacingAngleA + 0.1f && targetFacing <= targetFacingAngleA - 0.1f)
            //    {
            //        Game.PrintMessage("BAD HOOK", MessageType.LogMessage);
            //        me.Stop();
            //        return;
            //    }
            //}

            //if (Utils.SleepCheck("PudgePRObadHookSleep2"))
            //{
            //    if (Utils.SleepCheck("BLAHBLAHBLAH"))
            //    {
            //        Game.PrintMessage("target is turning " + TargetTurning(), MessageType.LogMessage);
            //        Utils.Sleep(100, "BLAHBLAHBLAH");

            //    }

            //    if (TargetTurning())// || (walkStraight < straightTimer && walkStraight != 0))
            //    {
            //        Game.PrintMessage("BAD HOOK", MessageType.LogMessage);
            //        me.Stop();
            //        return;
            //    }
            //    Utils.Sleep(100, "PudgePRObadHookSleep2");
            //}

            // Combo type.
            if (Menu.Item("comboType").GetValue<StringList>().SelectedIndex == 0 && Utils.SleepCheck("PudgePROcomboTypeSleep"))
            {
                GetAbilities();

                var dismAddRange = (dismember != null) ? dismember.GetCastRange() : 100;

                if (aetherLens == null) dismAddRange -= 75;
                else if (aetherLens != null) dismAddRange -= 125;

                //Menu.Item("itemsCon").Equals("item_force_staff");
                if (blink != null && target.NetworkPosition.Distance2D(me.NetworkPosition) < blink.GetCastRange() + me.HullRadius + dismAddRange)
                    useBlink.SetValue(true);
                else
                    useBlink.SetValue(false);

                Utils.Sleep(100, "PudgePROcomboTypeSleep");
            }
            else if (Menu.Item("comboType").GetValue<StringList>().SelectedIndex == 1)
            {
                if (useBlink.GetValue<bool>()) useBlink.SetValue(false);
            }
            else if (Menu.Item("comboType").GetValue<StringList>().SelectedIndex == 2)
            {
                // Do nothing
            }



            // Kill Steal with "nuke" abilities.
            if (killSteal.GetValue<bool>())
            {
                GetAbilities();

                if (hook == null || !hook.CanBeCasted() || hook.IsInAbilityPhase || me.IsChanneling() || !Utils.SleepCheck("PudgePROkillStealSleep")) return;

                var targets = ObjectManager.GetEntities<Hero>().Where(hero => hero.IsAlive && !hero.IsIllusion && hero.IsVisible && hero.Team == me.GetEnemyTeam()).ToList();

                if (targets == null || !targets.Any()) return;

                foreach (var enemy in targets)
                {
                    var hookFullDamage = AbilityDamage.CalculateDamage(hook, me, enemy);

                    //Game.PrintMessage("Hook Damage: " + hookFullDamage, MessageType.LogMessage);

                    if (enemy.Health < hookFullDamage && hook.GetCastRange() >= enemy.NetworkPosition.Distance2D(me.NetworkPosition))
                    {
                        //Game.PrintMessage("Trying to hook " + enemy, MessageType.LogMessage);
                        CastSkillShotEnemy(hook, target, "pudge_meat_hook", soulring, false);
                    }
                }

                Utils.Sleep(100, "PudgePROkillStealSleep");
            }



            if (Game.IsKeyDown(allyHookKey.GetValue<KeyBind>().Key))
            {
                GetAbilities();
                GetPredictionValues();

                aetherRange = aetherLens != null ? (uint)220 : (uint)0;
                var hookRange = hook.GetCastRange() + me.HullRadius;                

                if (allyTarget == null || !allyTarget.IsValid || allyTarget.IsIllusion || !allyTarget.IsAlive || allyTarget.IsInvul()) return;

                //if (!allyRotate)
                //{
                //    var allyFacingLocationB = allyTarget.InFront(100);
                //    var allyFacingAngleB = allyTarget.Position.ToVector2().FindAngleBetween(allyFacingLocationB.ToVector2(), true);
                //    allyFacing = allyFacingAngleB;
                //    allyRotate = true;
                //}
                
                if (soulring != null && !soulRing.GetValue<bool>())
                    soulring = null;

                if (hook != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(hook.Name) &&
                    hookRange >= allyTarget.NetworkPosition.Distance2D(me) && hook.CanBeCasted() &&
                    !me.IsChanneling() && Utils.SleepCheck("PudgePROallyComboSleep"))
                {
                    Utils.Sleep(100, "PudgePROallyComboSleep");

                    CastSkillShotAlly(hook, allyTarget, me.NetworkPosition);

                    //Game.PrintMessage("allyTarget: " + allyTarget.Name, MessageType.LogMessage);

                    //var predict = Prediction.PredictedXYZ(allyTarget, 1);//(me, allyTarget, 0, hook.GetProjectileSpeed("pudge_meat_hook"), hook.GetCastRange("pudge_meat_hook"));
                    //var mePos = me.Position;
                    //var reachTime = Prediction.CalculateReachTime(allyTarget, hook.GetProjectileSpeed(), predict - mePos);
                    //bool allyIdle = Prediction.IsIdle(allyTarget);
                    //var allyLocation = SkillShotTEST(me, allyTarget, (float)hook.GetHitDelay(allyTarget), hook.GetProjectileSpeed("pudge_meat_hook"), hook.GetRadius("pudge_meat_hook"));

                    //Game.PrintMessage("allyLocation: " + allyLocation, MessageType.LogMessage);
                    //hook.UseAbility(allyLocation);
                    //if (!allyIdle) hook.UseAbility(allyLocation);
                    //else hook.UseAbility(allyTarget.NetworkPosition);
                }


                    //if (((hook != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(hook.Name)) &&
                    //    hookRange >= allyTarget.NetworkPosition.Distance2D(me) && hook.CanBeCasted() &&
                    //    !me.IsChanneling() && Utils.SleepCheck("PudgePROcomboSleep")))
                    //{

                    //    Utils.Sleep(comboSleepGet, "PudgePROcomboSleep");

                    //    //Game.PrintMessage("COMBO WORKING", MessageType.LogMessage);
                    //    CastAbility(hook, hook.GetCastRange());

                    //    if (!Utils.SleepCheck("PudgePRObadHookSleep")) return;
                    //    //if (walkStraight < 500 && walkStraight != 0) return;

                    //    var allyFacingLocationA = allyTarget.InFront(100);
                    //    var allyFacingAngleA = allyTarget.Position.ToVector2().FindAngleBetween(allyFacingLocationA.ToVector2(), true);

                    //    //Game.PrintMessage("targetFacing: " + targetFacing + "targetFacingAngleA: " + targetFacingAngleA, MessageType.LogMessage);

                    //    if (badHook.GetValue<bool>() && hook != null && hook.IsInAbilityPhase && ((allyFacing + rotTolerance < allyFacingAngleA || allyFacing - rotTolerance > allyFacingAngleA) || (Prediction.IsIdle(allyTarget) && !allyStop)))
                    //    {
                    //        //Game.PrintMessage("BAD HOOK", MessageType.LogMessage);
                    //        me.Stop();
                    //        allyRotate = false;
                    //        if (Prediction.IsIdle(allyTarget)) allyStop = true;
                    //        return;
                    //    }
                    //    allyRotate = false;
                    //}

                    //if (hook != null && !hook.CanBeCasted() && allyStop && Utils.SleepCheck("PudgePROidleSleep"))
                    //{
                    //    //Game.PrintMessage("resetting idle", MessageType.LogMessage);

                    //    allyStop = false;
                    //    Utils.Sleep(100, "PudgePROidleSleep");
                    //}

                    //if (Utils.SleepCheck("PudgePRObadHookSleep") && allyRotate && Utils.SleepCheck("PudgePROrotateSleep"))
                    //{
                    //    //Game.PrintMessage("resetting rotate", MessageType.LogMessage);

                    //    allyRotate = false;
                    //    Utils.Sleep(100, "PudgePROrotateSleep");
                    //}





            }


            // Full combo
            // Check if Combo key is being held down.
            if (Game.IsKeyDown(comboKey.GetValue<KeyBind>().Key) || Menu.Item("comboToggleKey").IsActive())
            {
                // Retrieves values for ability and item variables.
                GetAbilities();
                GetPredictionValues();

                aetherRange = aetherLens != null ? (uint)220 : (uint)0;
                var hookRange = hook.GetCastRange() + me.HullRadius;

                //var targetFacingLocationA = target.InFront(1000); ;
                //float targetFacingAngleA = 0;
                //sleepTimer = sleepTime.GetValue<Slider>().Value;
                //straightTimer = straightTime.GetValue<Slider>().Value;

                
                if (target == null || !target.IsValid || target.IsIllusion || !target.IsAlive || target.IsInvul()) return;

                if (!targetRotateOld && !newHookCheck.GetValue<bool>())
                {
                    var targetFacingLocationB = target.InFront(100);
                    var targetFacingAngleB = target.Position.ToVector2().FindAngleBetween(targetFacingLocationB.ToVector2(), true);
                    targetFacing = targetFacingAngleB;
                    targetRotateOld = true;
                }


                if (target.HasModifier("modifier_pudge_meat_hook") && target.Distance2D(me.Position) > 1000 && Utils.SleepCheck("PudgePROblink"))
                {
                    me.Stop();
                    Utils.Sleep(100, "PudgePROblink");
                }


                if (soulring != null && !soulRing.GetValue<bool>())
                    soulring = null;

                //if (bottle != null && bottle.CanBeCasted() && !me.IsChanneling()
                //    && Menu.Item("itemsHD").GetValue<AbilityToggler>().IsEnabled(bottle.Name)
                //    && !me.HasModifier("modifier_bottle_regeneration"))
                //    bottle.UseAbility(me);

                UseBlink();

                if (!Utils.SleepCheck("PudgePROblink")) return;

                UseForceStaff();

                if (!Utils.SleepCheck("PudgePROforceStaff")) return;

                if (Utils.SleepCheck("PudgePROsheepThornSleep"))
                {
                    UseItem(bloodthorn, bloodthorn.GetCastRange());
                    UseItem(orchid, orchid.GetCastRange());
                    UseItem(sheep, sheep.GetCastRange());
                    UseItem(ghost, itemUseRange);
                    UseItem(glimmer, itemUseRange);
                    Utils.Sleep(100, "PudgePROsheepThornSleep");
                }

                UseRot();

                UseItem(dust, dust.GetCastRange());
                UseItem(veil, veil.GetCastRange());
                UseItem(ethereal, ethereal.GetCastRange());
                UseDagon();
                UseItem(urn, urn.GetCastRange());
                UseItem(shivas, shivas.GetCastRange());
                UseItem(pipe, itemUseRange);
                UseItem(hood, itemUseRange);
                UseItem(crimson, itemUseRange);

                if ((hookRange < target.NetworkPosition.Distance2D(me) || !MeHasMana()) && !me.IsChanneling() &&
                    !me.Spellbook.Spells.Any(x => x.IsInAbilityPhase) && Utils.SleepCheck("PudgePROorbwalkSleep"))
                {
                    //Game.PrintMessage("ORBWALKING.", MessageType.LogMessage);
                    Orbwalk();
                    Utils.Sleep(100, "PudgePROorbwalkSleep");
                }
                else if (dismember != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(dismember.Name) && dismember.GetCastRange() + me.HullRadius >= target.Position.Distance2D(me.Position) &&
                    dismember.CanBeCasted() && !dismember.IsInAbilityPhase && !me.IsChanneling() && Utils.SleepCheck("PudgePROpreComboSleep"))
                {
                    //Game.PrintMessage("PRE-DISMEMBERING.", MessageType.LogMessage);
                    CastAbility(dismember, dismember.GetCastRange() + me.HullRadius);
                    Utils.Sleep(100, "PudgePROpreComboSleep");
                }
                else if (((hook != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(hook.Name)) ||
                    (dismember != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(dismember.Name))) &&
                    hookRange >= target.NetworkPosition.Distance2D(me) && hook.CanBeCasted() && !dismember.IsInAbilityPhase &&
                    !me.IsChanneling() && Utils.SleepCheck("PudgePROcomboSleep"))
                {

                    Utils.Sleep(100, "PudgePROcomboSleep");

                    //Game.PrintMessage("COMBO WORKING", MessageType.LogMessage);
                    CastAbility(hook, hook.GetCastRange());

                    //if (targetFacing == targetFacingOld) targetRotate = false;

                    if (!Utils.SleepCheck("PudgePRObadHookSleep") && !newHookCheck.GetValue<bool>()) return;
                    //if (walkStraight < 500 && walkStraight != 0) return;

                    
                    var targetFacingLocationA = target.InFront(100);
                    var targetFacingAngleA = target.Position.ToVector2().FindAngleBetween(targetFacingLocationA.ToVector2(), true);

                        //Game.PrintMessage(" targetFacing: " + targetFacing + " targetFacingAngleA: " + targetFacingAngleA, MessageType.LogMessage);
                    

                    //if (targetFacing == targetFacingOld) targetFacing = targetFacingAngleA;

                    //Game.PrintMessage("NEED TO STOP HOOK " + targetRotate, MessageType.LogMessage);

                    if (badHook.GetValue<bool>() && hook != null && hook.IsInAbilityPhase &&
                        (((targetRotate && newHookCheck.GetValue<bool>()) || ((targetFacing + rotTolerance < targetFacingAngleA || targetFacing - rotTolerance > targetFacingAngleA) && !newHookCheck.GetValue<bool>()))
                        || (Prediction.IsIdle(target) && ((!TargetStillIdle() && newHookCheck.GetValue<bool>()) || !targetStop && !newHookCheck.GetValue<bool>())) ||
                        (dismember != null && dismember.CanBeCasted() && target.Position.Distance2D(me.Position) < dismember.GetCastRange() + me.HullRadius) || !CastSkillShotEnemy(hook, target, "pudge_meat_hook", soulring, true)))
                    {
                        //Game.PrintMessage("BAD HOOK " + hook.IsInAbilityPhase + " " + (targetFacing + rotTolerance < targetFacingAngleA) + " " + 
                        //    (targetFacing - rotTolerance > targetFacingAngleA) + " " + Prediction.IsIdle(target) + " " + targetStop + " " +
                        //    CastSkillShotEnemy(hook, target, "pudge_meat_hook", soulring, true), MessageType.LogMessage);
                        //Game.PrintMessage("STOPING HOOK", MessageType.LogMessage);
                        me.Stop();                        
                        targetRotateOld = false;
                        //targetFacing = targetFacingAngleA;
                        if (Prediction.IsIdle(target)) targetStop = true;
                        return;
                    }
                    targetRotateOld = false;

                    if (target.Position.Distance2D(me.Position) < hook.GetCastRange()) MoveToMousePos();

                    //if (0.1 + 0.1f < 0.5 || 0.1 - 0.1f > 0.5)

                    //if (Prediction.IsTurning(target))// || (walkStraight < straightTimer && walkStraight != 0))
                    //{
                    //    Game.PrintMessage("BAD HOOK", MessageType.LogMessage);
                    //    me.Stop();
                    //    return;
                    //}

                    CastAbility(dismember, hook.GetCastRange() + me.HullRadius);
                    //Utils.Sleep(100, "PudgePROcomboSleep");
                }
                else if ((hook != null ||
                    (dismember != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(dismember.Name))) &&
                    hookRange >= target.NetworkPosition.Distance2D(me) && !hook.CanBeCasted() && !dismember.IsInAbilityPhase &&
                    !me.IsChanneling() && Utils.SleepCheck("PudgePROdismemSleep"))
                {
                    Utils.Sleep(100, "PudgePROdismemSleep");
                    CastAbility(dismember, hook.GetCastRange() + me.HullRadius);
                }

                    if (hook != null && !hook.CanBeCasted() && targetStop && Utils.SleepCheck("PudgePROidleSleep"))
                {
                    //Game.PrintMessage("resetting idle", MessageType.LogMessage);
                
                    targetStop = false;
                    Utils.Sleep(100, "PudgePROidleSleep");
                }

                if (Utils.SleepCheck("PudgePRObadHookSleep") && targetRotate && Utils.SleepCheck("PudgePROrotateSleep") && !newHookCheck.GetValue<bool>())
                {
                    //Game.PrintMessage("resetting rotate", MessageType.LogMessage);

                    targetRotateOld = false;
                    Utils.Sleep(100, "PudgePROrotateSleep");
                }

            }
        }
    }
}


