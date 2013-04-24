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

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("General")]
        [DisplayName("Movement")]
        [Description("Allow IWM to handle movement")]
        public bool EnableMovement { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("General")]
        [DisplayName("Facing")]
        [Description("Allow IWM to handle facing target")]
        public bool EnableFacing { get; set; }

        [Setting]
        [Styx.Helpers.DefaultValue(false)]
        [Category("General")]
        [DisplayName("Targeting")]
        [Description("Allow IWM to handle targeting")]
        public bool EnableTargeting { get; set; }

    }
}
