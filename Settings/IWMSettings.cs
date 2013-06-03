#region Revision info
/*
 * $Author$
 * $Date$
 * $ID: $
 * $Revision$
 * $URL$
 * $LastChangedBy$
 * $ChangesMade: $
 */
#endregion

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

        private static string SettingsPath
        {
            get
            {
                return string.Format("{0}\\Settings\\IWantMovement\\Settings_{1}_2", Utilities.AssemblyDirectory,
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
        [Styx.Helpers.DefaultValue(35)]
        [Category("- Movement")]
        [DisplayName("Max Distance")]
        [Description("Maximum distance we should be away from target before starting to move closer.")]
        public int MaxDistance { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(33)]
        [Category("- Movement")]
        [DisplayName("Stop Distance")]
        [Description("The distance from target before we should stop moving.")]
        public int StopDistance { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("- Movement")]
        [DisplayName("Move Behind Target")]
        [Description("Attempt to move behind the target (i.e. for melee classes)")]
        public bool MoveBehindTarget { get; set; }

        /*
        [Setting]
        [Styx.Helpers.DefaultValue(1000)]
        [Category("- Movement")]
        [DisplayName("Throttle Time")]
        [Description("Duration in milliseconds to wait between re-attempting action.")]
         */
        public int MovementThrottleTime = 500;

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

        /*
        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Pull Behavior")]
        [DisplayName("Force Combat (Pull)")]
        [Description("Will override the combat routines pull behavior.")]
        public bool ForceCombat { get; set; }
         */
        public const bool ForceCombat = true;
        
        [Setting]
        [Styx.Helpers.DefaultValue("Death Grip")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability: Death Knight")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellDeathKnight { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Auto Shot")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability: Hunter")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellHunter { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Judgment")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability: Paladin")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellPaladin { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Shadow Word: Pain")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability: Priest")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellPriest { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Lightning Bolt")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability: Shaman")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellShaman { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Throw")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability: Rogue")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellRogue { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Crackling Jade Lightning")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability: Monk")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellMonk { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue("Moonfire")]
        [Category("Pull Behavior")]
        [DisplayName("Pull Ability: Druid")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellDruid { get; set; }

        #endregion
    }
}
