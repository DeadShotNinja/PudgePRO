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
    internal partial class Variables
    {
        public static void GetAbilities()
        {
            if (!Utils.SleepCheck("PudgePROGetAbilities")) return;
            blink = me.FindItem("item_blink");
            soulring = me.FindItem("item_soul_ring");
            //sheep = me.FindItem("item_sheepstick");
            //veil = me.FindItem("item_veil_of_discord");
            shivas = me.FindItem("item_shivas_guard");
            //dagon = me.GetDagon();
            ghost = me.FindItem("item_ghost");
            //ethereal = me.FindItem("item_ethereal_blade");
            aetherLens = me.FindItem("item_aether_lens");
            //bottle = me.FindItem("item_bottle");
            urn = me.FindItem("item_urn_of_shadows");
            hook = me.FindSpell("pudge_meat_hook");
            rot = me.FindSpell("pudge_rot");
            dismember = me.FindSpell("pudge_dismember");
            Utils.Sleep(1000, "PudgePROGetAbilities");
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


        public static bool MeHasMana()
        {
            //var itemCD = ObjectManager.GetEntities<Item>().Where(x => x.CanBeCasted());
            //foreach (var item in itemCD)
            //{

            //}
            if (
                //sheep != null && sheep.CanBeCasted() && me.Mana > sheep.ManaCost)
                //|| (dagon != null && dagon.CanBeCasted() && me.Mana > dagon.ManaCost)
                (hook != null && hook.CanBeCasted() && me.Mana > hook.ManaCost)
                //|| (veil != null && veil.CanBeCasted() && me.Mana > veil.ManaCost && !target.HasModifier("modifier_item_veil_of_discord_debuff"))
                || (shivas != null && shivas.CanBeCasted() && me.Mana > shivas.ManaCost)
                //|| (ethereal != null && ethereal.CanBeCasted() && me.Mana > ethereal.ManaCost)
                || (dismember != null && dismember.CanBeCasted() && me.Mana > dismember.ManaCost)
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
            }
            else
            {
                circle.SetControlPoint(2, me.Position);
                circle.SetControlPoint(6, new Vector3(1, 0, 0));
                circle.SetControlPoint(7, target.Position);
            }
        }

        public static void CastAbility(Ability ability, float range)
        {
            if (ability == null || !ability.CanBeCasted() || ability.IsInAbilityPhase ||
                !target.IsValidTarget(range, true, me.NetworkPosition) || me.IsChanneling() ||
                !Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(ability.Name)) return; //||
                //!Utils.SleepCheck("PudgePROcomboSleep")) return;


            if ((ability.Name.Contains("hook") && dismember == null) || (ability.Name.Contains("hook") && dismember != null &&
                !dismember.CanBeCasted() && !dismember.IsInAbilityPhase && !me.IsChanneling()) || (ability.Name.Contains("hook") && dismember != null &&
                dismember.GetCastRange() + me.HullRadius < target.NetworkPosition.Distance2D(me)))
            {
                //Game.PrintMessage("Trying to Hook.", MessageType.LogMessage);
                if (!hookPredict.GetValue<bool>()) ability.UseAbility(target.NetworkPosition);
                else ability.CastSkillShot(target, "pudge_meat_hook", soulring);
                return;
            }
            else if (ability.Name.Contains("dismember") && !dismember.IsInAbilityPhase && !me.IsChanneling())
            {
                //Game.PrintMessage("Trying to dismember.", MessageType.LogMessage);
                ability.UseAbility(target);
                return;
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

        //public static void UseDagon()
        //{
        //    if (dagon == null
        //        || !dagon.CanBeCasted()
        //        || target.IsMagicImmune()
        //        || !(target.NetworkPosition.Distance2D(me) - target.RingRadius <= dagon.CastRange)
        //        || !Menu.Item("items").GetValue<AbilityToggler>().IsEnabled("item_dagon")
        //        || !IsFullDebuffed()) return;
        //    //|| (!Utils.SleepCheck("PudgePROebsleep"))) return;          

        //    dagon.UseAbility(target);
        //}

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
                me.Spellbook.Spells.Any(x => x.IsInAbilityPhase) || !Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(item.Name))// || !Utils.SleepCheck("PudgePROitemSleep"))
                return;

            //if (item.Name.Contains("veil") && !target.HasModifier("modifier_item_veil_of_discord_debuff"))
            //{
            //    item.UseAbility(target.NetworkPosition);
            //    return;
            //}

            //if (item.Name.Contains("ethereal") && IsFullDebuffed())
            //{
            //    item.UseAbility(target);
            //    //Utils.Sleep(me.NetworkPosition.Distance2D(target.NetworkPosition) / 1200 * 1000, "PudgePROebsleep");
            //    return;
            //}
            if ((item.Name.Contains("urn") && urn.CurrentCharges > 0 && me.Distance2D(target) <= 400))// && Utils.SleepCheck("urn")))
            {
                item.UseAbility(target);
                //Utils.Sleep(240, "urn");
                //Utils.Sleep(100, "PudgePROitemSleep");
                return;
            }

            if (item.IsAbilityBehavior(AbilityBehavior.UnitTarget)) //&& !item.Name.Contains("item_dagon"))
            {
                //if (item.Name.Contains("bottle"))
                //{
                //    item.UseAbility(me);
                //    return;
                //}
                item.UseAbility(target);
                //Utils.Sleep(100, "PudgePROitemSleep");
                return;
            }

            if (item.IsAbilityBehavior(AbilityBehavior.Point))
            {
                item.UseAbility(target.NetworkPosition);
                //Utils.Sleep(100, "PudgePROitemSleep");
                return;
            }

            if (item.IsAbilityBehavior(AbilityBehavior.Immediate))
            {
                item.UseAbility();
                //Utils.Sleep(100, "PudgePROitemSleep");
            }
        }

        //public static bool IsFullDebuffed()
        //{
        //    if (
        //        (veil != null && veil.CanBeCasted() &&
        //         Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(veil.Name) &&
        //         !target.HasModifier("modifier_item_veil_of_discord_debuff"))
        //        ||
        //        (ethereal != null && //ethereal.CanBeCasted() &&
        //         Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(ethereal.Name) &&
        //         !target.HasModifier("modifier_item_ethereal_blade_slow"))
        //        )
        //        return false;
        //    return true;
        //}

        public static bool AllOnCooldown()
        {
            if (
                (hook.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= hook.CastRange && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(hook.Name))
                || (dismember.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= dismember.CastRange && Menu.Item("abilities").GetValue<AbilityToggler>().IsEnabled(dismember.Name))
                //|| (dagon.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= dagon.CastRange)
                //|| (sheep.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= sheep.CastRange)
                //|| (veil.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= veil.CastRange && !target.HasModifier("modifier_item_veil_of_discord_debuff"))
                //|| (ethereal.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= ethereal.CastRange)
                || (shivas.CanBeCasted() && target.NetworkPosition.Distance2D(me) <= shivas.CastRange)
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

        public static void UseBlink()
        {

            if (!useBlink.GetValue<bool>() || blink == null || !blink.CanBeCasted() || !Utils.SleepCheck("PudgePROblink")) return;// ||
                //(hook.GetCastRange() + me.HullRadius >= target.NetworkPosition.Distance2D(me))) return;

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

            Utils.Sleep(200, "PudgePROblink");
        }

        public static float GetDistance2D(Vector3 p1, Vector3 p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}