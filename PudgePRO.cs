using System;
using System.Linq;
using System.Reflection;
using Ensage;
using Ensage.Common;
using Ensage.Common.Menu;
using Ensage.Common.Extensions;
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
        }

        private static void OnLoad(object sender, EventArgs e)
        {
            if (!loaded)
            {
                me = ObjectManager.LocalHero;
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
            target = me.ClosestToMouseTarget(ClosestToMouseRange.GetValue<Slider>().Value);
            //var ally = ObjectManager.GetEntities<Hero>().Where(hero => hero.IsAlive && !hero.IsIllusion && hero.IsVisible && hero.Team == me.Team).ToList();
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

            //if (Game.IsKeyDown(allyHookKey.GetValue<KeyBind>().Key))
            //{
            //    GetAbilities();
            //    GetPredictionValues();

            //    aetherRange = aetherLens != null ? (uint)220 : (uint)0;
            //    var hookRange = hook.GetCastRange() + me.HullRadius;

            //    if (allyTarget == null || !allyTarget.IsValid || allyTarget.IsIllusion || !allyTarget.IsAlive || allyTarget.IsInvul()) return;
            //}


                // Full combo
                // Check if Combo key is being held down.
                if (Game.IsKeyDown(comboKey.GetValue<KeyBind>().Key))
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

                if (!targetRotate)
                {
                    var targetFacingLocationB = target.InFront(100);
                    var targetFacingAngleB = target.Position.ToVector2().FindAngleBetween(targetFacingLocationB.ToVector2(), true);
                    targetFacing = targetFacingAngleB;
                    targetRotate = true;
                }

                if (target == null || !target.IsValid || target.IsIllusion || !target.IsAlive || target.IsInvul()) return;

                if (soulring != null && !soulRing.GetValue<bool>())
                    soulring = null;

                //if (bottle != null && bottle.CanBeCasted() && !me.IsChanneling()
                //    && Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(bottle.Name)
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
                    UseItem(ghost, 1000);
                    UseItem(glimmer, 1000);
                    Utils.Sleep(100, "PudgePROsheepThornSleep");
                }

                UseRot();

                UseItem(veil, veil.GetCastRange());
                UseItem(ethereal, ethereal.GetCastRange());
                UseDagon();
                UseItem(urn, urn.GetCastRange());
                UseItem(shivas, shivas.GetCastRange());

                if ((hookRange < target.NetworkPosition.Distance2D(me) || !MeHasMana()) && !me.IsChanneling() &&
                    !me.Spellbook.Spells.Any(x => x.IsInAbilityPhase) && Utils.SleepCheck("PudgePROorbwalkSleep"))
                {
                    //Game.PrintMessage("ORBWALKING.", MessageType.LogMessage);
                    Orbwalk();
                    Utils.Sleep(100, "PudgePROorbwalkSleep");
                }
                else if (dismember != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(dismember.Name) && dismember.GetCastRange() + me.HullRadius >= target.NetworkPosition.Distance2D(me) &&
                    dismember.CanBeCasted() && !dismember.IsInAbilityPhase && !me.IsChanneling() && Utils.SleepCheck("PudgePROcomboSleep"))
                {
                    //Game.PrintMessage("PRE-DISMEMBERING.", MessageType.LogMessage);
                    CastAbility(dismember, dismember.GetCastRange() + me.HullRadius);
                    Utils.Sleep(100, "PudgePROcomboSleep");
                }
                else if (((hook != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(hook.Name)) ||
                    (dismember != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(dismember.Name))) &&
                    hookRange >= target.NetworkPosition.Distance2D(me) && hook.CanBeCasted() && !dismember.IsInAbilityPhase &&
                    !me.IsChanneling() && Utils.SleepCheck("PudgePROcomboSleep"))
                {

                    Utils.Sleep(comboSleepGet, "PudgePROcomboSleep");

                    //Game.PrintMessage("COMBO WORKING", MessageType.LogMessage);
                    CastAbility(hook, hook.GetCastRange());

                    if (!Utils.SleepCheck("PudgePRObadHookSleep")) return;
                    //if (walkStraight < 500 && walkStraight != 0) return;

                    var targetFacingLocationA = target.InFront(100);
                    var targetFacingAngleA = target.Position.ToVector2().FindAngleBetween(targetFacingLocationA.ToVector2(), true);

                    //Game.PrintMessage("targetFacing: " + targetFacing + "targetFacingAngleA: " + targetFacingAngleA, MessageType.LogMessage);

                    if (badHook.GetValue<bool>() && hook != null && hook.IsInAbilityPhase && ((targetFacing + rotTolerance < targetFacingAngleA || targetFacing - rotTolerance > targetFacingAngleA) || (Prediction.IsIdle(target) && !targetStop)))
                    {
                        //Game.PrintMessage("BAD HOOK", MessageType.LogMessage);
                        me.Stop();
                        targetRotate = false;
                        if (Prediction.IsIdle(target)) targetStop = true;
                        return;
                    }
                    targetRotate = false;
                    

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

                if (hook != null && !hook.CanBeCasted() && targetStop && Utils.SleepCheck("PudgePROidleSleep"))
                {
                    //Game.PrintMessage("resetting idle", MessageType.LogMessage);
                
                    targetStop = false;
                    Utils.Sleep(100, "PudgePROidleSleep");
                }

                if (Utils.SleepCheck("PudgePRObadHookSleep") && targetRotate && Utils.SleepCheck("PudgePROrotateSleep"))
                {
                    //Game.PrintMessage("resetting rotate", MessageType.LogMessage);
                    
                    targetRotate = false;
                    Utils.Sleep(100, "PudgePROrotateSleep");
                }

            }
        }
    }
}


