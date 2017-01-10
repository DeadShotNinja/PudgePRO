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


            // Full combo
            // Check if Combo key is being held down.
            if (Game.IsKeyDown(comboKey.GetValue<KeyBind>().Key))
            {
                // Retrieves values for ability and item variables.
                GetAbilities();
                
                aetherRange = aetherLens != null ? (uint)220 : (uint)0;
                var hookRange = hook.GetCastRange() + me.HullRadius;

                if (target == null || !target.IsValid || target.IsIllusion || !target.IsAlive || target.IsInvul()) return;

                if (soulring != null && !soulRing.GetValue<bool>())
                    soulring = null;

                //if (bottle != null && bottle.CanBeCasted() && !me.IsChanneling()
                //    && Menu.Item("items").GetValue<AbilityToggler>().IsEnabled(bottle.Name)
                //    && !me.HasModifier("modifier_bottle_regeneration"))
                //    bottle.UseAbility(me);

                UseBlink();

                //UseForceStaff();

                UseRot();

                UseItem(ghost, 1000);
                UseItem(urn, urn.GetCastRange());
                UseItem(shivas, shivas.GetCastRange());

                if ((hookRange < target.NetworkPosition.Distance2D(me) || !MeHasMana()) && !me.IsChanneling() && 
                    !me.Spellbook.Spells.Any(x => x.IsInAbilityPhase) && Utils.SleepCheck("PudgePROorbwalkSleep"))
                {
                    //Game.PrintMessage("ORBWALKING.", MessageType.LogMessage);
                    Orbwalk();
                    Utils.Sleep(100, "PudgePROorbwalkSleep");
                }
                else if (dismember.GetCastRange() + me.HullRadius >= target.NetworkPosition.Distance2D(me) && 
                    dismember.CanBeCasted() && !dismember.IsInAbilityPhase && !me.IsChanneling())// && Utils.SleepCheck("PudgePROupclose"))
                {
                    //Game.PrintMessage("PRE-DISMEMBERING.", MessageType.LogMessage);
                    CastAbility(dismember, dismember.GetCastRange() + me.HullRadius);
                    //Utils.Sleep(100, "PudgePROupclose");
                }
                else if (hookRange >= target.NetworkPosition.Distance2D(me) && hook.CanBeCasted() && !dismember.IsInAbilityPhase && 
                    !me.IsChanneling() && Utils.SleepCheck("PudgePROcomboSleep"))
                {
                    //Game.PrintMessage("COMBO WORKING", MessageType.LogMessage);
                    CastAbility(hook, hook.GetCastRange());
                    CastAbility(dismember, hook.GetCastRange());
                    Utils.Sleep(100, "PudgePROcomboSleep");
                }
            }
        }
    }

}
