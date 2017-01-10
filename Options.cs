using Ensage.Common.Menu;

namespace PudgePRO
{
    internal class Options : Variables
    {
        public static void MenuInit()
        {
            heroName = "npc_dota_hero_pudge";
            Menu = new Menu(AssemblyName, AssemblyName, true, heroName, true);
            comboKey = new MenuItem("comboKey", "Combo Key").SetValue(new KeyBind(70, KeyBindType.Press)).SetTooltip("Full combo in logical order.");
            useBlink = new MenuItem("useBlink", "Use Blink Dagger").SetValue(true).SetTooltip("Will auto blink (with logic) while combo key is held down.");
            soulRing = new MenuItem("soulRing", "Soulring").SetValue(true).SetTooltip("Will use soul ring before combo.");
            hookPredict = new MenuItem("hookPredict", "Auto Hook Prediction").SetValue(true).SetTooltip("Will auto predict target location for EZ hooks.");
            safeForce = new MenuItem("forcePredict", "Safe Force Staff").SetValue(true).SetTooltip("Will only forcestaff if you're facing the target.");
            //toggleHookTime = new MenuItem("toggleHookTime", "Bad Hook STOP").SetValue(new Slider(100, 200, 280)).SetTooltip("TESTING: Prevent fail hooks.");
            //badHook = new MenuItem("badHook", "Fail Hook Ticks").SetValue(new Slider(70, 1, 75)).SetTooltip("TESTING: Will STOP action within selected ticks to prevent POSSIBLE bad hooks.");
            //fountainBottle = new MenuItem("fountainBottle", "Bottle at Fountain").SetValue(true).SetTooltip("Will auto use bottle while at fountain.");            
            //bladeMail = new MenuItem("bladeMail", "Check for BladeMail").SetValue(false).SetTooltip("Will not combo if target used blademail.");
            drawTarget = new MenuItem("drawTarget", "Target indicator").SetValue(true).SetTooltip("Shows red circle around your target.");
            moveMode = new MenuItem("moveMode", "Orbwalk").SetValue(false).SetTooltip("Will orbwalk to mouse while combo key is held down.");
            ClosestToMouseRange = new MenuItem("ClosestToMouseRange", "Closest to mouse range").SetValue(new Slider(1500, 1, 2000)).SetTooltip("Will look for enemy in selected range around your mouse pointer.");
            //SafeBlinkRange = new MenuItem("SafeBlinkRange", "Safe Blink Range").SetValue(new Slider(400, 0, 1000)).SetTooltip("Will NOT blink closer to enemy than selected range.");

            items = new Menu("Items", "Items");
            abilities = new Menu("Abilities", "Abilities");
            targetOptions = new Menu("Target Options", "Target Options");

            Menu.AddItem(comboKey);

            Menu.AddSubMenu(items);
            Menu.AddSubMenu(abilities);
            Menu.AddSubMenu(targetOptions);

            items.AddItem(new MenuItem("items", "Items").SetValue(new AbilityToggler(itemsDictionary)));
            items.AddItem(useBlink);
            items.AddItem(safeForce);
            //items.AddItem(SafeBlinkRange);
            items.AddItem(soulRing);
            //items.AddItem(fountainBottle);
            //items.AddItem(bladeMail);
            abilities.AddItem(new MenuItem("abilities", "Abilities").SetValue(new AbilityToggler(abilitiesDictionary)));
            abilities.AddItem(hookPredict);
            targetOptions.AddItem(moveMode);
            targetOptions.AddItem(ClosestToMouseRange);
            targetOptions.AddItem(drawTarget);
            //targetOptions.AddItem(badHook);
            //targetOptions.AddItem(toggleHookTime);

            Menu.AddToMainMenu();
        }

    }
}