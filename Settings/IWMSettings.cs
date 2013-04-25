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

        public IWMSettings()
            : base(SettingsPath + ".config")
        {
        }

        public static IWMSettings Instance
        {
            get { return _instance ?? (_instance = new IWMSettings()); }
        }

        public static string SettingsPath
        {
            get
            {
                return string.Format("{0}\\Settings\\IWantMovement\\Settings_{1}", Utilities.AssemblyDirectory,
                                     Me.Name);
            }
        }

        #region Movement
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Movement")]
        [DisplayName("Enable Movement")]
        [Description("Allow IWM to handle movement")]
        public bool EnableMovement { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(35)]
        [Category("Movement")]
        [DisplayName("Max Distance")]
        [Description("Maximum distance we should be away from target before starting to move closer.")]
        public int MaxDistance { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(25)]
        [Category("Movement")]
        [DisplayName("Min Distance")]
        [Description("Minimum distance the target should be away before from us, and for us to stop moving.")]
        public int MinDistance { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Movement")]
        [DisplayName("Move Behind Target")]
        [Description("Attempt to move behind the target (i.e. for melee classes)")]
        public bool MoveBehindTarget { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(1000)]
        [Category("Movement")]
        [DisplayName("Throttle Time")]
        [Description("Duration in milliseconds to wait between re-attempting action.")]
        public int MovementThrottleTime { get; set; }

        #endregion Movement

        #region Facing
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Facing")]
        [DisplayName("Enable Facing")]
        [Description("Allow IWM to handle facing target")]
        public bool EnableFacing { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(1000)]
        [Category("Facing")]
        [DisplayName("Throttle Time")]
        [Description("Duration in milliseconds to wait between re-attempting action.")]
        public int FacingThrottleTime { get; set; }

        #endregion Facing

        #region Targeting
        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("Targeting")]
        [DisplayName("Enable Targeting")]
        [Description("Allow IWM to handle targeting")]
        public bool EnableTargeting { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(5000)]
        [Category("Targeting")]
        [DisplayName("Throttle Time")]
        [Description("Duration in milliseconds to wait between re-attempting action.")]
        public int TargetingThrottleTime { get; set; }
        #endregion

        #region Pull Behavior
        [Setting]
        [Styx.Helpers.DefaultValue("Auto Attack")]
        [Category("Pull Behavior")]
        [DisplayName("Spell Name")]
        [Description("The spell name to cast to force us to get in combat (which will trigger the combat routine).")]
        public string PullSpellName { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(true)]
        [Category("Pull Behavior")]
        [DisplayName("Force Combat")]
        [Description("Will cast the spell defined under setting 'Spell Name' to get us in combat.")]
        public bool ForceCombat { get; set; }
        #endregion
    }
}
