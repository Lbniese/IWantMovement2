#region Revision info
/*
 * $Author: millz $
 * $Date: 2013-04-22 19:20:01 +0100 (Mon, 22 Apr 2013) $
 * $ID$
 * $Revision: 1 $
 * $URL:  $
 * $LastChangedBy: millz $
 * $ChangesMade: $
 */
#endregion

using System;
using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.Plugins;
using System.Windows.Media;
using Styx.WoWInternals.WoWObjects;

namespace IWantMovement
{
    public class IWantMovement : HBPlugin
    {
        private Form _gui;
        Targeting _previousTargetMethod;
        Targeting _thisTargetMethod;

        public static LocalPlayer Me { get { return StyxWoW.Me; } }
        //public static GUI ConfigForm = new GUI();
        private static string SvnRevision { get { return "$Rev: 1 $"; } }

        

        #region Default Overrides
        public override string Author { get { return "Millz"; }}
        public override string ButtonText { get { return "Settings"; }}
        public override string Name { get { return "I Want Movement"; }}
        public override bool WantButton { get { return true; }}
        public override Version Version { get { return new Version(1,0, Convert.ToInt32(SvnRevision)); }}
        #endregion Default Overrides

        public override void OnButtonPress()
        {
            if (_gui == null || _gui.IsDisposed || _gui.Disposing) _gui = new GUI();
            if (_gui != null || _gui.IsDisposed) _gui.ShowDialog();
        }

        public override void Initialize()
        {
            _previousTargetMethod = Targeting.Instance;
            _thisTargetMethod = new Target();
          
            Log("IWantMovement Initialized");
            base.Initialize();
        }

        public override void Dispose()
        {
            Targeting.Instance = _previousTargetMethod;
            Log("Disabling IWantMovement");
            base.Dispose();
        }

        public override void Pulse()
        {
            /*
            if ((_thisTargetMethod != Targeting.Instance) && ConfigForm.EnableTargeting)
            {
                _thisTargetMethod = new Target();
                Targeting.Instance = _thisTargetMethod;
                Log("Setting IWantMovement to control targeting.");
            }
            */

            if (true/*ConfigForm.EnableFacing*/)
            {
                if (Me.CurrentTarget != null && !Me.IsSafelyFacing(Me.CurrentTarget))
                {
                    Log("[Facing: {0}] [Target HP: {1}] [Target Distance: {2}]", Me.CurrentTarget.Name, Me.CurrentTarget.HealthPercent, Me.CurrentTarget.Distance);
                    Me.CurrentTarget.Face();
                }
            }
            /*
            if (ConfigForm.EnableMovement)
            {
                
            }
            */
        }

        public static void Log(string logText, params object[] args)
        {
            Logging.Write(LogLevel.Normal, Colors.LawnGreen, string.Format("[IWM]: {0} {1}", logText), args);
        }

    }
}
