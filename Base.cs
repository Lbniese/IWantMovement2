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
using System.Windows.Forms;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;
using IWantMovement.Helper;
using IWantMovement.Managers;
using IWantMovement.Settings;

namespace IWantMovement
{
// ReSharper disable InconsistentNaming
    internal class IWantMovement : HBPlugin
// ReSharper restore InconsistentNaming
    {
        private Form _gui;
        Targeting _previousTargetMethod;
        Targeting _thisTargetMethod;

        internal static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static string SvnRevision { get { return "$Rev$"; } }
        private static IWMSettings Settings { get { return IWMSettings.Instance; } }
        private DateTime _facingLast;
        private static bool _initialized = false;

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
            if (!_initialized) // prevent init twice.
            {
                Log.Info("Storing current targeting instance.");
                _previousTargetMethod = Targeting.Instance;
                Log.Info("Creating our targeting instance.");
                _thisTargetMethod = new Target();
                Log.Info("Adding combat routine hook to pull behavior.");
                TreeHooks.Instance.AddHook("Combat_Pull", Managers.IWantMovement.PullBehaviorHook);

                Log.Info("IWantMovement Initialized [ {0}]", SvnRevision.Replace("$", "")); // Will print as [ Rev: 1 ]
                Log.Info("Designed to be used with PureRotation - http://tinyurl.com/purev2");
                Log.Info("~ Millz");
                _initialized = true;
           
                base.Initialize();
            }
        }

        public override void Dispose()
        {
            Targeting.Instance = _previousTargetMethod;
            TreeHooks.Instance.RemoveHook("Combat_Pull", Managers.IWantMovement.PullBehaviorHook);
            Log.Info("Disabling IWantMovement");
            base.Dispose();
        }

        public override void Pulse()
        {
            //Log.Debug("[Facing Last:{0}] [Facing Throttle:{1}] [Allow Face:{2}]", _facingLast, Settings.FacingThrottleTime, DateTime.UtcNow > _facingLast.AddMilliseconds(Settings.FacingThrottleTime));
            //Log.Debug("[Targeting Last:{0}] [Targeting Throttle:{1}] [Allow Targeting:{2}]", Target._targetLast, Settings.TargetingThrottleTime, DateTime.UtcNow > Target._targetLast.AddMilliseconds(Settings.TargetingThrottleTime));
            //Log.Debug("[Movement Last:{0}] [Movement Throttle:{1}] [Allow Movement:{2}]", Movement._movementLast, Settings.MovementThrottleTime, DateTime.UtcNow > Movement._movementLast.AddMilliseconds(Settings.MovementThrottleTime));

            if ((_thisTargetMethod != Targeting.Instance) && Settings.EnableTargeting)
            {
                Log.Warning("Taking control of targeting. If this message is being spammed, something else is trying to take control.");
                Targeting.Instance = _thisTargetMethod;
            }

            if (Targeting.Instance == _thisTargetMethod && Settings.EnableTargeting && !Me.GotTarget)
            {
                Target.AquireTarget();
            }

            if (Settings.EnableFacing && (DateTime.UtcNow > _facingLast.AddMilliseconds(Settings.FacingThrottleTime)) && Me.CurrentTarget != null && !Me.IsMoving && !Me.IsSafelyFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance <= 50)
            {
                    Log.Info("[Facing: {0}] [Target HP: {1}] [Target Distance: {2}]", Me.CurrentTarget.Name, Me.CurrentTarget.HealthPercent, Me.CurrentTarget.Distance);
                    Me.CurrentTarget.Face();
                    _facingLast = DateTime.UtcNow;
            }
            
            if (Settings.EnableMovement)
            {
                Movement.Move();
            }

            
            //GetInCombat();

        }

        public static void GetInCombat()
        {/*
            if (Settings.ForceCombat && Settings.PullSpellName != "" && !Me.IsActuallyInCombat && Me.GotTarget && Me.CurrentTarget == BotPoi.Current.AsObject && Me.CurrentTarget.IsHostile && SpellManager.CanCast(Settings.PullSpellName, Me.CurrentTarget, true))
            {
                Log.Info("[Casting: {0}] [Target: {1}] [Target HP: {2}] [Target Distance: {3}]", Settings.PullSpellName, Me.CurrentTarget.Name, Me.CurrentTarget.HealthPercent, Me.CurrentTarget.Distance);
                SpellManager.Cast(Settings.PullSpellName);
            }
           */
        }
    }
}
