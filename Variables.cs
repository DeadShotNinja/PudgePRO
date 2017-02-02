using System.Collections.Generic;
using System.Timers;
using Ensage;
using Ensage.Common.Menu;
using SharpDX;

namespace PudgePRO
{
    internal partial class Variables
    {
        public const string AssemblyName = "PudgePRO";

        public static string heroName;

        //public static string[] modifiersNames =
        //{
        //    "modifier_medusa_stone_gaze_stone",
        //    "modifier_winter_wyvern_winters_curse",
        //    "modifier_item_lotus_orb_active",
        //    "modifier_nyx_assassin_spiked_carapace",
        //    "modifier_abaddon_borrowed_time",
        //    "modifier_naga_siren_song_of_the_siren"
        //};

        public static Dictionary<string, bool> abilitiesDictionary = new Dictionary<string, bool>
        {
            {"pudge_dismember", true},
            {"pudge_rot", true},
            {"pudge_meat_hook", true}
        };

        public static Dictionary<string, bool> itemsDictionaryDamage = new Dictionary<string, bool>
        {
                {"item_bloodthorn", true},
                {"item_orchid", true},
                {"item_shivas_guard", true},
                {"item_dagon", true},
                {"item_ethereal_blade", true},
                {"item_urn_of_shadows", true}
        };

        public static Dictionary<string, bool> itemsDictionaryControl = new Dictionary<string, bool>
        {
                {"item_sheepstick", true},
                {"item_force_staff", true},
                {"item_veil_of_discord", true},
                {"item_dust", true}
        };

        public static Dictionary<string, bool> itemsDictionaryHealingDef = new Dictionary<string, bool>
        {
                {"item_crimson_guard", true},
                {"item_hood_of_defiance", true},
                {"item_pipe", true},
                {"item_ghost", true},
                {"item_glimmer_cape", true}
                //{"item_bottle", true}
        };

        //public static Dictionary<float, double> RotSpeedDictionary = new Dictionary<float, double>();

        //public static List<Hero> playerList = new List<Hero>();

        public static Menu Menu;

        public static Menu items;

        public static Menu abilities;

        public static Menu targetOptions;

        public static Menu hookPredictions;

        public static MenuItem comboType;

        public static MenuItem comboKey;

        public static MenuItem comboToggleKey;

        public static MenuItem allyHookKey;

        public static MenuItem killSteal;

        public static MenuItem drawTarget;

        public static MenuItem moveMode;

        public static MenuItem blockedHookMove;

        public static MenuItem ClosestToMouseRange;

        //public static MenuItem SafeBlinkRange;

        public static MenuItem soulRing;

        public static MenuItem safeForce;

        public static MenuItem hookPredict;

        public static MenuItem hookPredictRad;

        public static MenuItem badHook;

        public static MenuItem newHookCheck;

        public static MenuItem comboSleep;

        public static MenuItem stopWait;

        public static MenuItem rotationTolerance;

        public static MenuItem itemRange;

        //public static MenuItem badHook;

        //public static MenuItem toggleHookTime;

        //public static MenuItem fountainBottle;

        //public static MenuItem bladeMail;

        public static MenuItem useBlink;

        public static bool loaded, rotOn, isInRange, blockedHook = false, targetRotate = false, 
            targetRotateOld = false, targetStop = false, allyRotate = false, allyStop = false, 
            initSleep = false, tStillIdling = false, rotToggled = false;

        //public static float minDistHook;

        public static Ability hook, rot, dismember;

        public static Item soulring, shivas, blink, ghost, aetherLens, urn, forcestaff, sheep, orchid, bloodthorn, veil, ethereal, dagon, glimmer, crimson, hood, pipe, dust; // bottle

        public static Hero me, target, allyTarget;

        public static uint aetherRange, comboSleepGet, stopWaitGet, rotationToleranceGet, itemUseRange;

        public static Vector2 iconSize, screenPosition;

        public static Vector3 predictedLocationVec;

        public static float targetFacing, targetFacingOld, targetFacingA, targetFacingB, allyFacing, targetFacingNew, walkStraight, sleepTimer, straightTimer, rotTolerance;

        public static double rotationSpeed;

        public static DotaTexture heroIcon;

        public static ParticleEffect circle;

        //public static Timer time;

        //public static readonly uint[] DagonDamage = { 0, 400, 500, 600, 700, 800 };
    }
}
