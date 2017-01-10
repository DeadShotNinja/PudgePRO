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

        public static Dictionary<string, bool> itemsDictionary = new Dictionary<string, bool>
        {
            //    {"item_sheepstick", true},
                {"item_shivas_guard", true},
            //    {"item_dagon", true},
            //    {"item_ethereal_blade", true},
                {"item_ghost", true},
                {"item_force_staff", true},
            //    {"item_veil_of_discord", true},
            //    {"item_bottle", true},
                {"item_urn_of_shadows", true}
        };

        public static Menu Menu;

        public static Menu items;

        public static Menu abilities;

        public static Menu targetOptions;

        public static MenuItem comboKey;

        public static MenuItem drawTarget;

        public static MenuItem moveMode;

        public static MenuItem ClosestToMouseRange;

        //public static MenuItem SafeBlinkRange;

        public static MenuItem soulRing;

        public static MenuItem safeForce;

        public static MenuItem hookPredict;

        //public static MenuItem badHook;

        //public static MenuItem toggleHookTime;

        //public static MenuItem fountainBottle;

        //public static MenuItem bladeMail;

        public static MenuItem useBlink;

        public static bool loaded, rotOn, isInRange;

        //public static float minDistHook;

        public static Ability hook, rot, dismember;

        public static Item soulring, shivas, blink, ghost, aetherLens, urn, forcestaff; //sheep, veil, dagon, ethereal, bottle

        public static Hero me, target;

        public static uint  aetherRange;

        public static Vector2 iconSize, screenPosition;

        //public static Vector3 hookLocation;

        public static DotaTexture heroIcon;

        public static ParticleEffect circle;

        public static Timer time;

        //public static readonly uint[] DagonDamage = { 0, 400, 500, 600, 700, 800 };
    }
}
