using System.ComponentModel;
using Styx;
using Styx.Common;
using Styx.Helpers;
using Styx.WoWInternals.WoWObjects;

namespace IWantMovement.Settings
{
    internal class IWMSettings : Styx.Helpers.Settings
    {
        private static IWMSettings _instance;
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private IWMSettings()
            : base(SettingsPath + ".config")
        {
        }

        public static IWMSettings Instance
        {
            get { return _instance ?? (_instance = new IWMSettings()); }
        }

        private static new string SettingsPath
        {
            get
            {
                return string.Format("{0}\\Settings\\IWantMovement2\\Settings_{1}_2", Utilities.AssemblyDirectory,
                                     Me.Name);
            }
        }

        #region Movement
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- Movement")]
        [DisplayName("Enable Movement")]
        [Description("Allow IWM to handle movement")]
        public bool EnableMovement { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- Movement")]
        [DisplayName("Move Behind Target")]
        [Description("Attempt to move behind the target (i.e. for melee classes)")]
        public bool MoveBehindTarget { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("- Movement")]
        [DisplayName("Allow Suspending Movement")]
        [Description("Suspends movements on keypress Q/W/E/A/S/D")]
        public bool AllowSuspendMovement { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(3000)]
        [Category("- Movement")]
        [DisplayName("Suspend Duration (ms)")]
        [Description("Duration in milliseconds to wait after keypress before activating movement.")]
        public int SuspendDuration { get; set; }

        public int MovementThrottleTime = 500;

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("- Movement")]
        [DisplayName("Enable Auto Dismount")]
        [Description("Enables the plugin to dismount when it believe we're stuck on the mount.")]
        public bool EnableAutoDismount { get; set; }

        #endregion Movement

        #region Facing
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- Facing")]
        [DisplayName("Enable Facing")]
        [Description("Allow IWM to handle facing target")]
        public bool EnableFacing { get; set; }

        #endregion Facing

        #region Targeting
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- Targeting")]
        [DisplayName("Enable Targeting")]
        [Description("Allow IWM to handle targeting")]
        public bool EnableTargeting { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("- Targeting")]
        [DisplayName("Clear target when combat with other unit")]
        [Description("Will clear the target if we're in combat, but the unit we're in combat with isn't targeting us or a member of our group.")]
        public bool ClearTargetIfNotTargetingGroup { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5000)]
        [Category("- Targeting")]
        [DisplayName("Throttle Time")]
        [Description("Duration in milliseconds to wait between re-attempting action.")]
        public int TargetingThrottleTime { get; set; }
        #endregion

        #region Rest
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- Rest")]
        [DisplayName("Enable Rest Behavior")]
        [Description("Allow IWM to handle resting (i.e. eating food, drinking for mana)")]
        public bool EnableRest { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(80)]
        [Category("- Rest")]
        [DisplayName("Eat Health Percent")]
        [Description("HP percentage before eating food")]
        public int EatPercent { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(60)]
        [Category("- Rest")]
        [DisplayName("Drink Mana Percent")]
        [Description("HP percentage before drinking")]
        public int DrinkPercent { get; set; }

        #endregion

        #region Pull Behavior

        public const bool ForceCombat = true;

        [Setting]
        [Styx.Helpers.DefaultValue("Stealth")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability 1")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpell1 { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Cheap Shot")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability 2")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpell2 { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Sinister Strike")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability 3")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpell3 { get; set; }


        #endregion
    }
}
