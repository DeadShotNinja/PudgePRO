// <copyright file="Variables.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace PudgePRO
{
    using System.Collections.Generic;

    using Ensage;
    using Ensage.Common.Menu;

    using SharpDX;

    internal partial class Variables
    {
        public const string AssemblyName = "PudgePRO";

        public static Menu abilities;

        public static Dictionary<string, bool> abilitiesDictionary =
            new Dictionary<string, bool>
            {
                { "pudge_dismember", true },
                { "pudge_rot", true },
                { "pudge_meat_hook", true }
            };

        public static Item aetherLens; // bottle

        public static uint aetherRange;

        public static float allyFacing;

        public static MenuItem allyHookKey;

        public static bool allyRotate = false;

        public static bool allyStop = false;

        public static Hero allyTarget;

        public static MenuItem badHook;

        public static Item blink; // bottle

        public static bool blockedHook = false;

        public static MenuItem blockedHookMove;

        public static Item bloodthorn; // bottle

        public static ParticleEffect circle;

        public static MenuItem ClosestToMouseRange;

        public static MenuItem comboKey;

        public static MenuItem comboSleep;

        public static uint comboSleepGet;

        public static MenuItem comboToggleKey;

        public static MenuItem comboType;

        public static Item crimson; // bottle

        public static Item cyclone; // bottle

        public static Item dagon; // bottle

        public static Ability dismember;

        public static MenuItem drawTarget;

        public static Item dust; // bottle

        public static Item ethereal; // bottle

        public static Item forcestaff; // bottle

        public static Item ghost; // bottle

        public static Item glimmer; // bottle

        public static DotaTexture heroIcon;

        public static string heroName;

        public static Item hood; // bottle

        // public static float minDistHook;
        public static Ability hook;

        public static MenuItem hookPredict;

        public static Menu hookPredictions;

        public static MenuItem hookPredictRad;

        public static Vector2 iconSize;

        public static bool initSleep = false;

        public static bool isInRange;

        public static MenuItem itemRange;

        public static Menu items;

        public static Dictionary<string, bool> itemsDictionaryControl =
            new Dictionary<string, bool>
            {
                { "item_sheepstick", true },
                { "item_force_staff", true },
                { "item_veil_of_discord", true },
                { "item_dust", true }
            };

        public static Dictionary<string, bool> itemsDictionaryDamage =
            new Dictionary<string, bool>
            {
                { "item_bloodthorn", true },
                { "item_orchid", true },
                { "item_shivas_guard", true },
                { "item_dagon", true },
                { "item_ethereal_blade", true },
                { "item_urn_of_shadows", true }
            };

        public static Dictionary<string, bool> itemsDictionaryHealingDef =
            new Dictionary<string, bool>
            {
                { "item_crimson_guard", true },
                { "item_hood_of_defiance", true },
                { "item_pipe", true },
                { "item_ghost", true },
                { "item_glimmer_cape", true }

                // {"item_bottle", true}
            };

        public static uint itemUseRange;

        public static MenuItem killSteal;

        public static bool loaded;

        public static Hero me;

        public static Menu Menu;

        public static MenuItem moveMode;

        public static MenuItem newHookCheck;

        public static Item orchid; // bottle

        public static Item pipe; // bottle

        public static Vector3 predictedLocationVec;

        public static Ability rot;

        public static double rotationSpeed;

        public static MenuItem rotationTolerance;

        public static uint rotationToleranceGet;

        public static bool rotOn;

        public static bool rotToggled = false;

        public static float rotTolerance;

        public static MenuItem safeForce;

        public static Vector2 screenPosition;

        public static Item sheep; // bottle

        public static Item shivas; // bottle

        public static float sleepTimer;

        public static Item soulring; // bottle

        public static MenuItem soulRing;

        public static MenuItem stopWait;

        public static uint stopWaitGet;

        public static float straightTimer;

        public static Hero target;

        public static float targetFacing;

        public static float targetFacingA;

        public static float targetFacingB;

        public static float targetFacingNew;

        public static float targetFacingOld;

        public static Menu targetOptions;

        public static bool targetRotate = false;

        public static bool targetRotateOld = false;

        public static bool targetStop = false;

        public static bool tStillIdling = false;

        public static Item urn; // bottle


        public static MenuItem useBlink;

        public static Item veil; // bottle

        public static float walkStraight;

 
    }
}