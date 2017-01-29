using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
using System.Linq;
//using System.Timers;
//using System.Reflection;
using Ensage;
using Ensage.Common;
//using Ensage.Common.AbilityInfo;
//using Ensage.Common.Enums;
using Ensage.Common.Menu;
using Ensage.Common.Extensions;
using static Ensage.Common.Extensions.SharpDX.Vector2Extensions;
using Ensage.Common.Objects;
//using Ensage.Common.Objects.UtilityObjects;
using SharpDX;

//using global::SharpDX;

namespace PudgePRO
{
    internal partial class Variables
    {
        public static void GetAbilities()
        {
            if (!Utils.SleepCheck("PudgePROGetAbilities")) return;
            blink = me.FindItem("item_blink");
            soulring = me.FindItem("item_soul_ring");
            sheep = me.FindItem("item_sheepstick");
            veil = me.FindItem("item_veil_of_discord");
            shivas = me.FindItem("item_shivas_guard");
            dagon = me.GetDagon();
            ghost = me.FindItem("item_ghost");
            ethereal = me.FindItem("item_ethereal_blade");
            aetherLens = me.FindItem("item_aether_lens");
            //bottle = me.FindItem("item_bottle");
            urn = me.FindItem("item_urn_of_shadows");
            forcestaff = me.FindItem("item_force_staff");
            orchid = me.FindItem("item_orchid");
            bloodthorn = me.FindItem("item_bloodthorn");
            glimmer = me.FindItem("item_glimmer_cape");
            crimson = me.FindItem("item_crimson_guard");
            hood = me.FindItem("item_hood_of_defiance");
            pipe = me.FindItem("item_pipe");
            hook = me.FindSpell("pudge_meat_hook");
            rot = me.FindSpell("pudge_rot");
            dismember = me.FindSpell("pudge_dismember");
            Utils.Sleep(1000, "PudgePROGetAbilities");
        }

        public static void GetPredictionValues()
        {
            if (!Utils.SleepCheck("PudgePROGetPredictionValues")) return;
            comboSleepGet = (uint)comboSleep.GetValue<Slider>().Value;
            stopWaitGet = (uint)stopWait.GetValue<Slider>().Value;
            rotationToleranceGet = (uint)rotationTolerance.GetValue<Slider>().Value;

            // Other options from menu.
            itemUseRange = (uint)itemRange.GetValue<Slider>().Value;

            switch (rotationToleranceGet)
            {
                case 0:
                    rotTolerance = 0.0f;
                    break;
                case 1:
                    rotTolerance = 0.1f;
                    break;
                case 2:
                    rotTolerance = 0.2f;
                    break;
                case 3:
                    rotTolerance = 0.3f;
                    break;
                case 4:
                    rotTolerance = 0.4f;
                    break;
                case 5:
                    rotTolerance = 0.5f;
                    break;
                case 6:
                    rotTolerance = 0.6f;
                    break;
                case 7:
                    rotTolerance = 0.7f;
                    break;
                case 8:
                    rotTolerance = 0.8f;
                    break;
                case 9:
                    rotTolerance = 0.9f;
                    break;
                case 10:
                    rotTolerance = 1.0f;
                    break;
                default:
                    break;
            }
            Utils.Sleep(1000, "PudgePROGetPredictionValues");
        }

        public static bool TargetRotated()
        {
                if (hook != null && !hook.IsInAbilityPhase && !initSleep)
                {
                    var targetFacingLocationA = target.InFront(100);
                    var targetFacingAngleA = target.Position.ToVector2().FindAngleBetween(targetFacingLocationA.ToVector2(), true);
                    targetFacingA = targetFacingAngleA;                

                    Utils.Sleep(stopWaitGet, "PudgePROrotatedSleep");
                    initSleep = true;
                    return false;
                }

                if (Utils.SleepCheck("PudgePROrotatedSleep"))
                {
                    var targetFacingLocationB = target.InFront(100);
                    var targetFacingAngleB = target.Position.ToVector2().FindAngleBetween(targetFacingLocationB.ToVector2(), true);

                    targetFacingB = targetFacingAngleB;
                    initSleep = false;
                }
                else if (!Utils.SleepCheck("PudgePROrotatedSleep")) return false;

                //Game.PrintMessage(" targetFacing: " + targetFacing + " targetFacingAngleA: " + targetFacingAngleA, MessageType.LogMessage);


                return (targetFacingA + rotTolerance < targetFacingB || targetFacingA - rotTolerance > targetFacingB);
        }

        public static bool TargetStillIdle()
        {
            bool stillIdle = Prediction.IsIdle(target);

            if (Utils.SleepCheck("PudgePROstillIdleSleep") && tStillIdling)
            {
                stillIdle = Prediction.IsIdle(target);
                tStillIdling = false;
                return stillIdle;
            }

            if (stillIdle && Utils.SleepCheck("PudgePROstillIdleSleep"))
            {
                Utils.Sleep(100, "PudgePROstillIdleSleep");
                tStillIdling = true;
            }

            return stillIdle;
        }

        //public static bool HasModifiers()
        //{
        //    if (target.HasModifiers(modifiersNames, false) ||
        //        (bladeMail.GetValue<bool>() && target.HasModifier("modifier_item_blade_mail_reflect")) ||
        //        !Utils.SleepCheck("PudgePROHasModifiers"))
        //        return true;
        //    Utils.Sleep(100, "PudgePROHasModifiers");
        //    return false;
        //}

        //public static bool IsInFountain()
        //{
        //    if (me.HasModifier("modifier_fountain_aura_buff")
        //        && !Utils.SleepCheck("PudgePROIsInFountain"))
        //        return true;
        //    Utils.Sleep(100, "PudgePROIsInFountain");
        //    return false;
        //}       

        //public static float DistanceToLineSegment(
        //this Vector2 point,
        //Vector2 segmentStart,
        //Vector2 segmentEnd,
        //bool onlyIfOnSegment = false)
        //{
        //    var objects = point.ProjectOn(segmentStart, segmentEnd);

        //    return objects.IsOnSegment || onlyIfOnSegment == false
        //               ? Vector2.Distance(objects.SegmentPoint, point)
        //               : float.MaxValue;
        //}

        public static Hero ClosestToMouseAlly(Unit source, float rangeT = 1000)
        {
            var mousePosition = Game.MousePosition;
            Hero closestHero = null;
            foreach (var x in Heroes.All)
            {
                if (!(x.IsValid && x.Team == source.Team && x != me && !x.IsIllusion && x.IsAlive && x.IsVisible
                      && x.Distance2D(mousePosition) <= rangeT
                      && !x.HasModifier("modifier_skeleton_king_reincarnation_scepter_active")))
                {
                    continue;
                }

                if (closestHero == null || closestHero.Distance2D(mousePosition) > x.Distance2D(mousePosition))
                {
                    closestHero = x;
                }
            }

            return closestHero;
        }

        //public static Vector3 SkillShotTEST(Unit source, Unit target, float delay, float speed, float radius)
        //{
        //    if (Prediction.IsIdle(target))
        //    {
        //        Game.PrintMessage("TARGET IDLE", MessageType.LogMessage);
        //        return target.Position;
        //    }

        //    if (!(speed < 6000) || speed <= 0)
        //    {
        //        Game.PrintMessage("TARGET TOO FAST???", MessageType.LogMessage);
        //        return Prediction.PredictedXYZ(target, delay);
        //    }

        //    var predict = Prediction.PredictedXYZ(target, delay);
        //    var sourcePos = source.Position;
        //    var reachTime = Prediction.CalculateReachTime(target, speed, predict - sourcePos);
        //    predict = Prediction.PredictedXYZ(target, delay + reachTime);

        //    Game.PrintMessage("target: " + target.Name + " delay: " + delay + " ReachTime: " + reachTime, MessageType.LogMessage);

        //    if (!(source.Distance2D(predict) > radius))
        //    {
        //        Game.PrintMessage("TARGET OUTSIDE OF RAD", MessageType.LogMessage);
        //        return Prediction.PredictedXYZ(target, delay + reachTime);
        //    }

        //    if (target.MovementSpeed * ((predict.Distance2D(sourcePos) - radius) / speed) < radius)
        //    {
        //        sourcePos = (sourcePos - predict) * (sourcePos.Distance2D(predict) - radius)
        //                    / sourcePos.Distance2D(predict) + predict;
        //        reachTime = Prediction.CalculateReachTime(target, speed, predict - sourcePos);
        //    }
        //    else
        //    {
        //        sourcePos = (sourcePos - predict)
        //                    * (sourcePos.Distance2D(predict)
        //                       + target.MovementSpeed * ((predict.Distance2D(sourcePos) - radius) / speed) - radius)
        //                    / sourcePos.Distance2D(predict) + predict;
        //        reachTime = Prediction.CalculateReachTime(target, speed, predict - sourcePos);
        //    }
        //    Game.PrintMessage("RETURNING: " + target + (delay + reachTime), MessageType.LogMessage);
        //    return Prediction.PredictedXYZ(target, delay + reachTime);
        //}

        public static bool CastSkillShotAlly(
            Ability abilityA,
            Unit targetA,
            Vector3 sourcePositionA,
            string abilityNameA = null,
            Ability soulRingA = null)
        {
            //Utils.Sleep(100, "PudgePROCastSkillshot");

            if (abilityA == null || !abilityA.IsValid)
            {
                //Game.PrintMessage("Return 1", MessageType.LogMessage);
                return false;
            }

            if (targetA == null || !targetA.IsValid)
            {
                //Game.PrintMessage("Return 2", MessageType.LogMessage);
                return false;
            }

            if (!Utils.SleepCheck("PudgePROCastSkillshotAlly" + abilityA.Handle))
            {
                //Game.PrintMessage("Return 3", MessageType.LogMessage);
                return false;
            }

            var nameA = abilityNameA ?? abilityA.StoredName();
            var ownerA = abilityA.Owner as Unit;
            var positionA = sourcePositionA;
            var delayA = abilityA.GetHitDelay(targetA, nameA);
            var dataA = abilityA.CommonProperties();

            // delay += data.AdditionalDelay;
            if (targetA.IsInvul() && !Utils.ChainStun(targetA, delayA, null, false))
            {
                //Game.PrintMessage("Return 4", MessageType.LogMessage);
                return false;
            }

            var xyzA = abilityA.GetPrediction(targetA, abilityName: nameA);
            var radiusA = abilityA.GetRadius(nameA);
            var rangeA = abilityA.TravelDistance();

            if (dataA.AllyBlock)
            {
                if (
                    Creeps.All.Any(
                        x =>
                            x.IsValid && x.IsAlive && x.Team == ownerA.Team && x.Distance2D(xyzA) <= rangeA
                            && x.Distance2D(ownerA) < ownerA.Distance2D(targetA)
                            && x.Position.ToVector2()
                                .DistanceToLineSegment(sourcePositionA.ToVector2(), xyzA.ToVector2())
                            <= radiusA + x.HullRadius))
                {
                    return false;
                }

                if (
                    Heroes.GetByTeam(ownerA.Team)
                          .Any(
                              hero =>
                                  hero.IsAlive && !hero.Equals(ownerA) && !hero.Equals(targetA)
                                  && hero.Distance2D(xyzA) <= rangeA && hero.Distance2D(ownerA) < ownerA.Distance2D(targetA)
                                  && hero.Position.ToVector2()
                                         .DistanceToLineSegment(sourcePositionA.ToVector2(), xyzA.ToVector2())
                                  <= radiusA + hero.HullRadius))
                {
                    return false;
                }
            }

            if (dataA.EnemyBlock)
            {
                if (
                    Creeps.All.Any(
                        x =>
                            x.IsValid && x.IsAlive && x.Team != ownerA.Team && x.Distance2D(xyzA) <= rangeA
                            && x.Distance2D(ownerA) < ownerA.Distance2D(targetA)
                            && x.Position.ToVector2()
                                .DistanceToLineSegment(sourcePositionA.ToVector2(), xyzA.ToVector2())
                            <= radiusA + x.HullRadius))
                {
                    return false;
                }

                if (
                    Heroes.GetByTeam(ownerA.GetEnemyTeam())
                          .Any(
                              hero =>
                                  hero.IsAlive && !hero.Equals(targetA) && hero.Distance2D(xyzA) <= rangeA
                                  && hero.Distance2D(ownerA) < ownerA.Distance2D(targetA)
                                  && hero.Position.ToVector2()
                                         .DistanceToLineSegment(sourcePositionA.ToVector2(), xyzA.ToVector2())
                                  <= radiusA + hero.HullRadius))
                {
                    return false;
                }
            }

            var speedA = abilityA.GetProjectileSpeed(nameA);
            var distanceXyzA = xyzA.Distance2D(positionA);

            //Game.PrintMessage("xyz " + xyzA, MessageType.LogMessage);

            //if (xyzA == null) Game.PrintMessage("xyz is null", MessageType.LogMessage);

            if (!(distanceXyzA <= rangeA + radiusA + targetA.HullRadius))
            {
                //Game.PrintMessage("Return 5", MessageType.LogMessage);
                //Game.PrintMessage("distanceXyz " + distanceXyzA + " range " + rangeA + " radius " + radiusA + " HullRadius " + targetA.HullRadius, MessageType.LogMessage);
                return false;
            }

            if (distanceXyzA > rangeA)
            {
                xyzA = xyzA - positionA;
                xyzA /= xyzA.Length();
                xyzA *= rangeA;
                xyzA += positionA;
            }

            // Console.WriteLine(ability.GetCastRange() + " " + radius);

            if (soulRingA != null && abilityA.ManaCost > 0 && soulRingA.CanBeCasted())
            {
                soulRingA.UseAbility();
            }

            abilityA.UseAbility(xyzA);
            return true;
        }

        public static bool CastSkillShotEnemy(
            Ability abilityE,
            Unit targetE,
            string abilityNameE = null,
            Ability soulRingE = null,
            bool collisionCheckE = false)
        {
            if (abilityE == null || !abilityE.IsValid)
            {
                return false;
            }

            if (targetE == null || !targetE.IsValid)
            {
                return false;
            }

            return CastSkillShotEnemy(abilityE, targetE, abilityE.Owner.Position, abilityNameE, soulRingE, collisionCheckE);
        }

        public static bool CastSkillShotEnemy(
            Ability abilityE,
            Unit targetE,
            Vector3 sourcePositionE,
            string abilityNameE = null,
            Ability soulRingE = null,
            bool collisionCheckE = false)
        {
            if (abilityE == null || !abilityE.IsValid)
            {
                return false;
            }

            if (targetE == null || !targetE.IsValid)
            {
                return false;
            }

            if (!Utils.SleepCheck("PudgePROCastSkillshotEnemy" + abilityE.Handle))
            {
                return false;
            }

            var nameE = abilityNameE ?? abilityE.StoredName();
            var ownerE = abilityE.Owner as Unit;
            var positionE = sourcePositionE;
            var delayE = abilityE.GetHitDelay(targetE, nameE);
            var dataE = abilityE.CommonProperties();

            // delay += data.AdditionalDelay;
            if (targetE.IsInvul() && !Utils.ChainStun(targetE, delayE, null, false))
            {
                return false;
            }

            var xyzE = abilityE.GetPrediction(targetE, abilityName: nameE);

            var radiusE = abilityE.GetRadius(nameE);
            var rangeE = abilityE.TravelDistance();

            if (dataE.AllyBlock)
            {
                if (
                    Creeps.All.Any(
                        x =>
                            x.IsValid && x.IsAlive && x.Team == ownerE.Team && x.Distance2D(xyzE) <= rangeE
                            && x.Distance2D(ownerE) < ownerE.Distance2D(targetE)
                            && x.Position.ToVector2()
                                .DistanceToLineSegment(sourcePositionE.ToVector2(), xyzE.ToVector2())
                            <= radiusE + x.HullRadius))
                {
                    //Game.PrintMessage("AllyBlock", MessageType.LogMessage);
                    blockedHook = true;
                    return false;
                }

                if (
                    Heroes.GetByTeam(ownerE.Team)
                          .Any(
                              hero =>
                                  hero.IsAlive && !hero.Equals(ownerE) && !hero.Equals(targetE)
                                  && hero.Distance2D(xyzE) <= rangeE && hero.Distance2D(ownerE) < ownerE.Distance2D(targetE)
                                  && hero.Position.ToVector2()
                                         .DistanceToLineSegment(sourcePositionE.ToVector2(), xyzE.ToVector2())
                                  <= radiusE + hero.HullRadius))
                {
                    //Game.PrintMessage("AllyBlock", MessageType.LogMessage);
                    blockedHook = true;
                    return false;
                }
            }



            if (dataE.EnemyBlock)
            {
                if (
                    Creeps.All.Any(
                        x =>
                            x.IsValid && x.IsAlive && x.Team != ownerE.Team && x.Distance2D(xyzE) <= rangeE
                            && x.Distance2D(ownerE) < ownerE.Distance2D(targetE)
                            && x.Position.ToVector2()
                                .DistanceToLineSegment(sourcePositionE.ToVector2(), xyzE.ToVector2())
                            <= radiusE + x.HullRadius))
                {
                    //Game.PrintMessage("EnemyBlock", MessageType.LogMessage);
                    blockedHook = true;
                    return false;
                }

                if (
                    Heroes.GetByTeam(ownerE.GetEnemyTeam())
                          .Any(
                              hero =>
                                  hero.IsAlive && !hero.Equals(targetE) && hero.Distance2D(xyzE) <= rangeE
                                  && hero.Distance2D(ownerE) < ownerE.Distance2D(targetE)
                                  && hero.Position.ToVector2()
                                         .DistanceToLineSegment(sourcePositionE.ToVector2(), xyzE.ToVector2())
                                  <= radiusE + hero.HullRadius))
                {
                    //Game.PrintMessage("EnemyBlock", MessageType.LogMessage);
                    blockedHook = true;
                    return false;
                }
            }

            blockedHook = false;

            var speedE = abilityE.GetProjectileSpeed(nameE);
            var distanceXyzE = xyzE.Distance2D(positionE);
            if (!(distanceXyzE <= rangeE + radiusE + targetE.HullRadius))
            {
                return false;
            }

            if (distanceXyzE > rangeE)
            {
                xyzE = xyzE - positionE;
                xyzE /= xyzE.Length();
                xyzE *= rangeE;
                xyzE += positionE;
            }

            // Console.WriteLine(ability.GetCastRange() + " " + radius);          


            if (abilityE.ManaCost > 0 && soulRingE.CanBeCasted())
            {
                soulRingE.UseAbility();
            }

            if (collisionCheckE == false) abilityE.UseAbility(xyzE);

            predictedLocationVec = xyzE;

            return true;
        }


        public static bool GetCastSkillShotEnemy(
            Ability abilityE,
            Unit targetE,
            string abilityNameE = null,
            Ability soulRingE = null,
            bool collisionCheckE = false)
        {
            if (abilityE == null || !abilityE.IsValid)
            {
                return false;
            }

            if (targetE == null || !targetE.IsValid)
            {
                return false;
            }

            return GetCastSkillShotEnemy(abilityE, targetE, abilityE.Owner.Position, abilityNameE, soulRingE, collisionCheckE);
        }

        public static bool GetCastSkillShotEnemy(
            Ability abilityE,
            Unit targetE,
            Vector3 sourcePositionE,
            string abilityNameE = null,
            Ability soulRingE = null,
            bool collisionCheckE = false)
        {
            if (abilityE == null || !abilityE.IsValid)
            {
                return false;
            }

            if (targetE == null || !targetE.IsValid)
            {
                return false;
            }

            if (!Utils.SleepCheck("PudgePROCastSkillshotEnemy" + abilityE.Handle))
            {
                return false;
            }

            var nameE = abilityNameE ?? abilityE.StoredName();
            var ownerE = abilityE.Owner as Unit;
            var positionE = sourcePositionE;
            var delayE = abilityE.GetHitDelay(targetE, nameE);
            var dataE = abilityE.CommonProperties();

            // delay += data.AdditionalDelay;
            if (targetE.IsInvul() && !Utils.ChainStun(targetE, delayE, null, false))
            {
                return false;
            }

            var xyzE = abilityE.GetPrediction(targetE, abilityName: nameE);

            var radiusE = abilityE.GetRadius(nameE);
            var rangeE = abilityE.TravelDistance();           

            var speedE = abilityE.GetProjectileSpeed(nameE);
            var distanceXyzE = xyzE.Distance2D(positionE);
            if (!(distanceXyzE <= rangeE + radiusE + targetE.HullRadius))
            {
                return false;
            }

            if (distanceXyzE > rangeE)
            {
                xyzE = xyzE - positionE;
                xyzE /= xyzE.Length();
                xyzE *= rangeE;
                xyzE += positionE;
            }

            // Console.WriteLine(ability.GetCastRange() + " " + radius);          


            if (abilityE.ManaCost > 0 && soulRingE.CanBeCasted())
            {
                soulRingE.UseAbility();
            }

            if (collisionCheckE == false) abilityE.UseAbility(xyzE);

            predictedLocationVec = xyzE;

            return true;
        }


        public static bool MeHasMana()
        {
            //var itemCD = ObjectManager.GetEntities<Item>().Where(x => x.CanBeCasted());
            //foreach (var item in itemCD)
            //{

            //}
            if (
                (sheep != null && sheep.CanBeCasted() && me.Mana > sheep.ManaCost)
                || (bloodthorn != null && bloodthorn.CanBeCasted() && me.Mana > bloodthorn.ManaCost)
                || (orchid != null && orchid.CanBeCasted() && me.Mana > orchid.ManaCost)
                || (dagon != null && dagon.CanBeCasted() && me.Mana > dagon.ManaCost)
                || (hook != null && hook.CanBeCasted() && me.Mana > hook.ManaCost)
                || (veil != null && veil.CanBeCasted() && me.Mana > veil.ManaCost && !target.HasModifier("modifier_item_veil_of_discord_debuff"))
                || (shivas != null && shivas.CanBeCasted() && me.Mana > shivas.ManaCost)
                || (ethereal != null && ethereal.CanBeCasted() && me.Mana > ethereal.ManaCost)
                || (dismember != null && dismember.CanBeCasted() && me.Mana > dismember.ManaCost)
                || (glimmer != null && glimmer.CanBeCasted() && me.Mana > glimmer.ManaCost)
                )
                return true;
            return false;
        }

        public static void TargetIndicator(EventArgs args)
        {
            if (!drawTarget.GetValue<bool>())
            {
                if (circle == null) return;
                circle.Dispose();
                circle = null;
                return;
            }
            if (target != null && target.IsValid && !target.IsIllusion && target.IsAlive && target.IsVisible &&
                me.IsAlive)
            {
                DrawTarget();
            }
            else if (circle != null)
            {
                circle.Dispose();
                circle = null;
            }
        }

        public static void DrawTarget()
        {
            heroIcon = Drawing.GetTexture("materials/ensage_ui/miniheroes/pudge");
            iconSize = new Vector2(HUDInfo.GetHpBarSizeY() * 2);

            if (
                !Drawing.WorldToScreen(target.Position + new Vector3(0, 0, target.HealthBarOffset / 3), out screenPosition))
                return;

            screenPosition += new Vector2(-iconSize.X, 0);
            Drawing.DrawRect(screenPosition, iconSize, heroIcon);

            if (circle == null)
            {
                circle = new ParticleEffect(@"particles\ui_mouseactions\range_finder_tower_aoe.vpcf", target);
                circle.SetControlPoint(2, me.Position);
                circle.SetControlPoint(6, new Vector3(1, 0, 0));
                circle.SetControlPoint(7, target.Position);

                if (hookPredictRad.GetValue<bool>() && predictedLocationVec != null)
                {
                    //Game.PrintMessage("Location: " + predictedLocationVec, MessageType.LogMessage);
                    circle.SetControlPoint(2, me.Position);
                    circle.SetControlPoint(6, new Vector3(1, 0, 0));
                    circle.SetControlPoint(7, predictedLocationVec);
                }
            }
            else
            {
                circle.SetControlPoint(2, me.Position);
                circle.SetControlPoint(6, new Vector3(1, 0, 0));
                circle.SetControlPoint(7, target.Position);

                if (hookPredictRad.GetValue<bool>() && predictedLocationVec != null)
                {
                    //Game.PrintMessage("Location: " + predictedLocationVec, MessageType.LogMessage);
                    circle.SetControlPoint(2, me.Position);
                    circle.SetControlPoint(6, new Vector3(1, 0, 0));
                    circle.SetControlPoint(7, predictedLocationVec);
                }
            }
        }

        public static void CastAbility(Ability ability, float range)
        {
            if (ability == null || !ability.CanBeCasted() || ability.IsInAbilityPhase ||
                !target.IsValidTarget(range, true, me.NetworkPosition) || me.IsChanneling() ||
                !Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(ability.Name) ||
                !Utils.SleepCheck("PudgePROcastSleep") || !Utils.SleepCheck("PudgePRObadHookSleep")) return;

            //walkStraight = Prediction.StraightTime(target);

            //if (target.NetworkActivity != NetworkActivity.Move)
            //{
            //    Game.PrintMessage("Target not moving.", MessageType.LogMessage);
            //    walkStraight = 0;
            //}

            Utils.Sleep(100, "PudgePROcastSleep");

            if ((ability.Name.Contains("hook") && dismember == null) || (ability.Name.Contains("hook") && dismember != null &&
                !dismember.CanBeCasted() && !dismember.IsInAbilityPhase && !me.IsChanneling()) || (ability.Name.Contains("hook") && dismember != null &&
                dismember.GetCastRange() + me.HullRadius < target.NetworkPosition.Distance2D(me)))
            {
                //hookLocation = Prediction.SkillShotXYZ(me, target, (float)hook.GetHitDelay(target, hook.Name), hook.GetProjectileSpeed(hook.Name), hook.GetRadius(hook.Name));
                //var targetFacingLocation = target.InFront(100);
                //var targetFacingAngle = target.Position.ToVector2().FindAngleBetween(targetFacingLocation.ToVector2(), true);
                
                if (sheep != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(sheep.Name) && sheep.CanBeCasted() && sheep.GetCastRange() > target.NetworkPosition.Distance2D(me)) return;
                if ((bloodthorn != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(bloodthorn.Name) && bloodthorn.CanBeCasted() && bloodthorn.GetCastRange() > target.NetworkPosition.Distance2D(me)) ||
                    (orchid != null && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(orchid.Name) && orchid.CanBeCasted() && orchid.GetCastRange() > target.NetworkPosition.Distance2D(me))) return;

                //Game.PrintMessage("Trying to Hook.", MessageType.LogMessage);

                if (!hookPredict.GetValue<bool>())
                {
                    //Game.PrintMessage("Raw hook.", MessageType.LogMessage);
                    ability.UseAbility(target.NetworkPosition);
                }
                else
                {
                    //Game.PrintMessage("Trying to Hook.", MessageType.LogMessage);
                    //ability.CastSkillShot(target, "pudge_meat_hook", soulring);
                    CastSkillShotEnemy(ability, target, "pudge_meat_hook", soulring, false);

                    //if (walkStraight < 500)
                    //{
                    //    Game.PrintMessage("NOT ENOUGH TIME TO HOOK." + walkStraight, MessageType.LogMessage);
                    //    me.Stop();
                    //}

                    if (badHook.GetValue<bool>()) Utils.Sleep(stopWaitGet, "PudgePRObadHookSleep");

                    //targetFacing = targetFacingAngle;
                }
                //return;
            }
            else if (ability.Name.Contains("dismember") && !dismember.IsInAbilityPhase && !me.IsChanneling() && dismember.CanBeCasted())
            {
                if ((ethereal != null && ethereal.CanBeCasted()) || 
                    (dagon != null && dagon.CanBeCasted()) || 
                    (veil != null && veil.CanBeCasted() && !target.HasModifier("modifier_item_veil_of_discord_debuff"))) return;

                //Game.PrintMessage("Trying to dismember.", MessageType.LogMessage);
                ability.UseAbility(target);
                //return;
            }


            //if (ability.Name.Contains("hook") && (!dismember.CanBeCasted() && dismember.GetCastRange() + me.HullRadius < target.NetworkPosition.Distance2D(me)))
            //{
            //    Game.PrintMessage("Trying to Hook.", MessageType.LogMessage);
            //    if (!hookPredict.GetValue<bool>()) ability.UseAbility(target.NetworkPosition);
            //    else ability.CastSkillShot(target, "pudge_meat_hook", soulring);
            //    return;
            //}
            //else if (ability.Name.Contains("hook") && !dismember.IsInAbilityPhase)
            //{
            //    Game.PrintMessage("Trying to Hook 2.", MessageType.LogMessage);
            //    if (!hookPredict.GetValue<bool>()) ability.UseAbility(target.NetworkPosition);
            //    else ability.CastSkillShot(target, "pudge_meat_hook", soulring);
            //    return;
            //}
            //else if (ability.Name.Contains("dismember") && !dismember.IsInAbilityPhase && !me.IsChanneling())
            //{
            //    Game.PrintMessage("Trying to dismember.", MessageType.LogMessage);
            //    ability.UseAbility(target);
            //    return;
            //}
        }

        //public static bool TargetTurning()
        //{
        //    targetFacingNew = targetFacing;

        //    if (!Utils.SleepCheck("PudgePROtargetTurning")) return false;

        //        Game.PrintMessage("targetFacing: " + targetFacingNew + "targetFacingAngle: " + targetFacing, MessageType.LogMessage);

        //    Utils.Sleep(50, "PudgePROtargetTurning");

        //    if (targetFacingNew >= targetFacing + 0.1f && targetFacing <= targetFacing - 0.1f) return true;
        //    else return false;
            
        //}

        public static void UseDagon()
        {
            if (dagon == null
                || !dagon.CanBeCasted()
                || target.IsMagicImmune()
                || !(target.NetworkPosition.Distance2D(me) - target.RingRadius <= dagon.GetCastRange())
                || !Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled("item_dagon")
                || !IsFullDebuffed()
                || !Utils.SleepCheck("PudgePROdagonSleep")) return;
            //|| !Utils.SleepCheck("PudgePROebsleep")) return;

            if (dismember != null && (me.IsChanneling() || dismember.IsInAbilityPhase)) return;

            //Game.PrintMessage("Using dagon.", MessageType.LogMessage);
            dagon.UseAbility(target);
            Utils.Sleep(100, "PudgePROdagonSleep");
        }

        public static void UseRot()
        {
            if (rot == null || !rot.CanBeCasted() || rot.IsInAbilityPhase || !Utils.SleepCheck("PudgePROrotCheck") ||
                !Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(rot.Name)) return;

            rotOn = me.HasModifier("modifier_pudge_rot") == true ? true : false;
            isInRange = (target.NetworkPosition.Distance2D(me) <= rot.GetRadius() + me.HullRadius == true) ? true : false;

            if (!rotOn && isInRange)
            {
                //Game.PrintMessage("Turning rot ON.", MessageType.LogMessage);
                rot.ToggleAbility();
            }
            else if (rotOn && !isInRange)
            {
                //Game.PrintMessage("Turning rot OFF.", MessageType.LogMessage);
                rot.ToggleAbility();
            }
            Utils.Sleep(200, "PudgePROrotCheck");
        }

        public static void UseItem(Item item, float range, int speed = 0)
        {
            if (item == null || !item.CanBeCasted() || target.IsMagicImmune() || target.MovementSpeed < speed ||
                target.HasModifier(item.Name) || me.IsChanneling() || !target.IsValidTarget(range, true, me.NetworkPosition) ||
                me.Spellbook.Spells.Any(x => x.IsInAbilityPhase))// || !Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled(item.Name)) ||
                //!Menu.Item("itemsCon").GetValue<AbilityToggler>().IsEnabled(item.Name) ||
                //!Menu.Item("itemsHD").GetValue<AbilityToggler>().IsEnabled(item.Name))// || !Utils.SleepCheck("PudgePROitemSleep"))
            {
                //Game.PrintMessage("ITEMS RETURNED.", MessageType.LogMessage);
                return;
            }

            if (item.Name.Contains("veil") && !target.HasModifier("modifier_item_veil_of_discord_debuff"))
            {
                if (Utils.SleepCheck("PudgePROveilSleep"))
                {
                    if (dismember != null && (me.IsChanneling() || dismember.IsInAbilityPhase)) return;

                    //Game.PrintMessage("Using veil.", MessageType.LogMessage);
                    if (Menu.Item("itemsCon").GetValue<AbilityToggler>().IsEnabled(item.Name)) item.UseAbility(target.NetworkPosition);
                    Utils.Sleep(100, "PudgePROveilSleep");
                }
                return;
            }

            if (item.Name.Contains("ethereal"))// && IsFullDebuffed())
            {
                if (Utils.SleepCheck("PudgePROetherealSleep") && ethereal.CanBeCasted() && !ethereal.IsInAbilityPhase)
                {
                    if (dismember != null && (me.IsChanneling() || dismember.IsInAbilityPhase)) return;

                    //Game.PrintMessage("Using ethereal.", MessageType.LogMessage);
                    if (Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled(item.Name)) item.UseAbility(target);
                    Utils.Sleep(100, "PudgePROetherealSleep");
                    //Utils.Sleep(me.NetworkPosition.Distance2D(target.NetworkPosition) / 1200 * 1000, "PudgePROebsleep");
                }
                return;
            }
            if ((item.Name.Contains("urn") && urn.CurrentCharges > 0 && me.Distance2D(target) <= 400))// && Utils.SleepCheck("urn")))
            {
                //Game.PrintMessage("Using urn.", MessageType.LogMessage);
                if (Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled(item.Name)) item.UseAbility(target);
                //Utils.Sleep(240, "urn");
                //Utils.Sleep(100, "PudgePROitemSleep");
                return;
            }

            if (item.IsAbilityBehavior(AbilityBehavior.UnitTarget) && !item.Name.Contains("item_dagon"))
            {
                if (item.Name.Contains("sheep"))
                {
                    if ((dismember != null && dismember.GetCastRange() + me.HullRadius >= target.NetworkPosition.Distance2D(me) && dismember.CanBeCasted()) ||
                        me.IsChanneling() || me.Spellbook.Spells.Any(x => x.IsInAbilityPhase) || !sheep.CanBeCasted()) return;

                    //Game.PrintMessage("Using SheepStick.", MessageType.LogMessage);
                    if (Menu.Item("itemsCon").GetValue<AbilityToggler>().IsEnabled(item.Name)) item.UseAbility(target);
                    return;
                }
                if (item.Name.Contains("glimmer"))
                {
                    //Game.PrintMessage("Using glimmer.", MessageType.LogMessage);
                    if (Menu.Item("itemsHD").GetValue<AbilityToggler>().IsEnabled(item.Name)) item.UseAbility(me);
                    return;
                }
                //if (item.Name.Contains("orchid") || item.Name.Contains("bloodthorn"))
                //{
                //    if ((dismember != null && dismember.GetCastRange() + me.HullRadius >= target.NetworkPosition.Distance2D(me) && dismember.CanBeCasted()) ||
                //        me.IsChanneling() || me.Spellbook.Spells.Any(x => x.IsInAbilityPhase) || !sheep.CanBeCasted()) return;

                //    //Game.PrintMessage("Using SheepStick.", MessageType.LogMessage);
                //    item.UseAbility(target);
                //    return;
                //}
                //if (item.Name.Contains("bottle"))
                //{
                //    item.UseAbility(me);
                //    return;
                //}
                if (me.IsChanneling() || me.Spellbook.Spells.Any(x => x.IsInAbilityPhase)) return;

                //Game.PrintMessage("Using ITEM.", MessageType.LogMessage);
                item.UseAbility(target);
                //Utils.Sleep(100, "PudgePROitemSleep");
                return;
            }

            if (item.IsAbilityBehavior(AbilityBehavior.Point) && !item.Name.Contains("veil"))
            {
                //Game.PrintMessage("Using ITEM 2.", MessageType.LogMessage);
                item.UseAbility(target.NetworkPosition);
                //Utils.Sleep(100, "PudgePROitemSleep");
                return;
            }

            if (item.IsAbilityBehavior(AbilityBehavior.Immediate))
            {
                //Game.PrintMessage("Using ITEM 3.", MessageType.LogMessage);
                item.UseAbility();
                //Utils.Sleep(100, "PudgePROitemSleep");
            }
        }

        public static bool IsFullDebuffed()
        {
            if (
                (veil != null && veil.CanBeCasted() &&
                 Menu.Item("itemsCon").GetValue<AbilityToggler>().IsEnabled(veil.Name) &&
                 !target.HasModifier("modifier_item_veil_of_discord_debuff"))
                ||
                (ethereal != null && ethereal.CanBeCasted() &&
                 Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled(ethereal.Name) &&
                 !target.HasModifier("modifier_item_ethereal_blade_slow"))
                )
                return false;
            return true;
        }

        public static bool AllOnCooldown()
        {
            if (
                (hook.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= hook.CastRange && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(hook.Name))
                || (dismember.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= dismember.CastRange && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(dismember.Name))
                || (dagon.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= dagon.CastRange && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(dagon.Name))
                || (sheep.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= sheep.CastRange && Menu.Item("itemsCon").GetValue<AbilityToggler>().IsEnabled(sheep.Name))
                || (bloodthorn.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= bloodthorn.CastRange && Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled(bloodthorn.Name))
                || (orchid.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= orchid.CastRange && Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled(orchid.Name))
                || (veil.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= veil.CastRange && !target.HasModifier("modifier_item_veil_of_discord_debuff") && Menu.Item("itemsCon").GetValue<AbilityToggler>().IsEnabled(veil.Name))
                || (ethereal.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= ethereal.CastRange && Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled(ethereal.Name))
                || (shivas.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= shivas.CastRange && Menu.Item("itemsDmg").GetValue<AbilityToggler>().IsEnabled(shivas.Name))
                || (glimmer.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= glimmer.CastRange && Menu.Item("itemsHD").GetValue<AbilityToggler>().IsEnabled(glimmer.Name))
                )
                return false;
            return true;
        }

        public static void Orbwalk()
        {
            switch (moveMode.GetValue<bool>())
            {
                case true:
                    Orbwalking.Orbwalk(target);
                    break;
                case false:
                    me.Attack(target);
                    break;
            }
        }

        public static void MoveToMousePos()
        {
            if (blockedHook == false || !blockedHookMove.GetValue<bool>()) return;

            //Game.PrintMessage("Moving to MOuse Pos.", MessageType.LogMessage);
            me.Move(Game.MousePosition, false);
        }

        public static void UseBlink()
        {

            if (!useBlink.GetValue<bool>() || blink == null || !blink.CanBeCasted() || !Utils.SleepCheck("PudgePROblink") ||
                (dismember.GetCastRange() + me.HullRadius >= target.NetworkPosition.Distance2D(me))) return;

            var fullBlinkRange = aetherLens == null ? 1200 : 1420;
            var currentPosition = me.Position;
            var targetPosition = target.Position;
            var blinkLocation = target.NetworkPosition;

            //Game.PrintMessage("TRYING TO BLINK.", MessageType.LogMessage);

            if (me.Distance2D(targetPosition) > fullBlinkRange)
            {
                var meTargetAngle = currentPosition.ToVector2().FindAngleBetween(targetPosition.ToVector2(), true);
                
                fullBlinkRange -= (int)me.HullRadius;

                blinkLocation = new Vector3(
                    currentPosition.X + fullBlinkRange * (float)Math.Cos(meTargetAngle),
                    currentPosition.Y + fullBlinkRange * (float)Math.Sin(meTargetAngle),
                    100);
            }

            blink.UseAbility(blinkLocation); 

            Utils.Sleep(100, "PudgePROblink");
        }

        public static void UseForceStaff()
        {
            if (forcestaff == null || !Menu.Item("itemsCon").GetValue<AbilityToggler>().IsEnabled(forcestaff.Name) ||
                !forcestaff.CanBeCasted() || me.IsChanneling() || !Utils.SleepCheck("PudgePROforceStaff")) return;

            var fullForceRange = forcestaff.GetCastRange();
            var tToMeDist = target.NetworkPosition.Distance2D(me);
            var currentPosition = me.Position;
            var targetPosition = target.Position;
            var meTargetAngle = currentPosition.ToVector2().FindAngleBetween(targetPosition.ToVector2(), true);
            var meToAnglePosition = me.InFront(forcestaff.GetCastRange());
            var meForceAngle = currentPosition.ToVector2().FindAngleBetween(meToAnglePosition.ToVector2(), true);
            //var meForceAngle = currentPosition.ToVector2().FindAngleBetween(meForcePosition, true);

            var newPosition = new Vector3(
                    currentPosition.X + fullForceRange * (float)Math.Cos(meTargetAngle),
                    currentPosition.Y + fullForceRange * (float)Math.Sin(meTargetAngle),
                    100);

            if (dismember != null && dismember.CanBeCasted() && tToMeDist < dismember.GetCastRange() + me.HullRadius) return;
            //me.Move(newPosition);
            //var turnTime = currentPosition.FindAngleForTurnTime(targetPosition);
            //Game.PrintMessage("Range is: " + fullForceRange, MessageType.LogMessage);

            //Game.PrintMessage("Angles: " + meForceAngle + " and " + meTargetAngle, MessageType.LogMessage);

            if (safeForce.GetValue<bool>() && (meForceAngle + 0.1f >= meTargetAngle && meForceAngle - 0.1f <= meTargetAngle) && !me.IsChanneling())
            {
                //Game.PrintMessage("Safe Force", MessageType.LogMessage);
                forcestaff.UseAbility(me);
                //Utils.Sleep(100, "PudgePROforceStaff");
            }
            else if (!safeForce.GetValue<bool>() && targetPosition.Distance2D(newPosition) < tToMeDist && !me.IsChanneling())
            {
                //Game.PrintMessage("YOLO Force", MessageType.LogMessage);
                forcestaff.UseAbility(me);
                //Utils.Sleep(100, "PudgePROforceStaff");
            }
            else return;

            Utils.Sleep(100, "PudgePROforceStaff");
        }        

        public static float GetDistance2D(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }        
    }
}