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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IWantMovement.Managers;
using Styx;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;
using IWantMovement.Helper;
using IWantMovement.Settings;

namespace IWantMovement
{
    internal class IWantMovement : HBPlugin
    {
        private Form _gui;
        Targeting _previousTargetMethod;
        Targeting _thisTargetMethod;

        internal static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static string SvnRevision { get { return "$Rev$"; } }

        private static IWMSettings Settings { get { return IWMSettings.Instance; } }

        #region Default Overrides
        public override string Author { get { return "Millz"; }}
        public override string ButtonText { get { return "Settings"; }}
        public override string Name { get { return "I Want Movement"; }}
        public override bool WantButton { get { return true; }}
        public override Version Version { get { return new Version(0,0,1); }}
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

            Log.Info("IWantMovement Initialized [ {0}]", SvnRevision.Replace("$", "")); // Will print as [ Rev: 1 ]
            Log.Info("Designed to be used with PureRotation - http://tinyurl.com/purev2");
            Log.Info("~ Millz");
            
            base.Initialize();
        }

        public override void Dispose()
        {
            Targeting.Instance = _previousTargetMethod;
            Log.Info("Disabling IWantMovement");
            base.Dispose();
        }

        public override void Pulse()
        {
            
            if ((_thisTargetMethod != Targeting.Instance) && Settings.EnableTargeting)
            {
                // we don't have control, let's take it.
                Log.Warning("Taking control of targeting. If this message is being spammed, something else is trying to take control.");
                Targeting.Instance = _thisTargetMethod;
            }

            if (Targeting.Instance == _thisTargetMethod && Settings.EnableTargeting && !Me.GotTarget)
            {
                // we've got control.
                Target.AquireTarget();
            }
            

            if (Settings.EnableFacing && Me.CurrentTarget != null && !Me.IsMoving && !Me.IsSafelyFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance <= 45)
            {
                    Log.Info("[Facing: {0}] [Target HP: {1}] [Target Distance: {2}]", Me.CurrentTarget.Name, Me.CurrentTarget.HealthPercent, Me.CurrentTarget.Distance);
                    Me.CurrentTarget.Face();
            }
            
            if (Settings.EnableMovement)
            {
                Movement.Move();
            }
            
        }

    }
}
