using System;
using System.Windows.Forms;
using IWantMovement.Helper;
using IWantMovement.Managers;
using IWantMovement.Settings;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Plugins;
using Styx.WoWInternals.WoWObjects;

namespace IWantMovement2
{
    internal class IWantMovement2 : HBPlugin
    {

        private Form _gui;
        Targeting _previousTargetMethod;
        Targeting _thisTargetMethod;

        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static string SvnRevision { get { return "$Rev$"; } }
        private static IWMSettings Settings { get { return IWMSettings.Instance; } }

        private static bool _initialized;

        private CombatRoutine _decoratedCombatRoutine;
        private CombatRoutine _undecoratedCombatRoutine;

        #region Default Overrides
        public override string Author { get { return "Lbniese"; } }
        public override string ButtonText { get { return "Settings"; } }
        public override string Name { get { return "I Want Movement 2"; } }
        public override bool WantButton { get { return true; }}
        public override Version Version { get { return new Version(0, 0, 1); } }
        #endregion Default Overrides

        public override void OnButtonPress()
        {
            if (_gui == null || _gui.IsDisposed || _gui.Disposing) _gui = new GUI();
            if (_gui != null || _gui.IsDisposed) _gui.ShowDialog();
        }

        public override void OnEnable()
        {
            if (_initialized) return;
            Log.Info("Storing current targeting instance.");
            _previousTargetMethod = Targeting.Instance;
            Log.Info("Creating our targeting instance.");
            _thisTargetMethod = new Target();

            Log.Info("IWantMovement2 Initialized [ {0}]", SvnRevision.Replace("$", ""));
            Log.Info("-- Originally by Millz");
            Log.Info("~ Continued by Lbniese");
            _initialized = true;

            base.OnEnable();
        }

        public override void OnDisable()
        {
            Log.Info("Removing IWantMovement2 Hooks");
            Targeting.Instance = _previousTargetMethod;
            RoutineManager.Current = _undecoratedCombatRoutine;
            Log.Info("Disabled IWantMovement2");
            base.OnDisable();
        }

        public override void Pulse()
        {
            if (Me.IsDead || Me.IsFlying || Me.IsGhost) { return; }

            if (Settings.EnableAutoDismount && BotManager.Current.Name != "Questing")
            {
                if (Me.Mounted &&
                    ((BotPoi.Current.Type == PoiType.Kill && BotPoi.Current.AsObject.Distance < 30) ||
                        (Me.GotTarget && Me.CurrentTarget.Distance < 30 && !Me.CurrentTarget.IsFriendly)))
                {
                    ActionLandAndDismount();
                }
                else
                {
                    if (Me.Mounted && Me.Combat && !Me.IsMoving && Me.GotTarget && Me.CurrentTarget.Distance < 30 &&
                        !Me.CurrentTarget.IsFriendly)
                    {
                        ActionLandAndDismount();
                    }
                }
            }

            if (Me.IsOnTransport || Me.Mounted) { return; }

            if ((RoutineManager.Current != null) && (RoutineManager.Current != _decoratedCombatRoutine))
            {
                Log.Info("Installing Combat Routine Hook...");
                _undecoratedCombatRoutine = RoutineManager.Current;
                _decoratedCombatRoutine = new IWantMovementCR(RoutineManager.Current);
                RoutineManager.Current = _decoratedCombatRoutine;
                Log.Info("Combat Routine Hook Installed!");
            }

            if ((_thisTargetMethod != Targeting.Instance) && Settings.EnableTargeting && !Me.HasAura("Food") && !Me.HasAura("Drink"))
            {
                Log.Warning("Taking control of targeting. If this message is being spammed, something else is trying to take control.");
                Targeting.Instance = _thisTargetMethod;
            }

            if (Targeting.Instance == _thisTargetMethod && Settings.EnableTargeting && !Me.GotTarget && !Me.HasAura("Food") && !Me.HasAura("Drink"))
            {
                if (!Me.GotTarget)
                {
                    Target.AquireTarget();
                }

            }

            if (Me.GotTarget)
            {
                Target.ClearTarget();
            }

            if (!Settings.EnableMovement || Me.HasAura("Food") || Me.HasAura("Drink") || (!Me.Combat && !Me.PetInCombat))
                return;
            if (Settings.EnableFacing && Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && !Me.IsMoving && !Me.IsSafelyFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance <= 50)
            {
                Log.Info("[Facing: {0}] [Target HP: {1}] [Target Distance: {2}]", Me.CurrentTarget.Name, Me.CurrentTarget.HealthPercent, Me.CurrentTarget.Distance);
                Me.CurrentTarget.Face();
            }
            Movement.Move();
        }

        public static Mount.ActionLandAndDismount ActionLandAndDismount()
        {
          var Reason = "[IWM2] Stuck on mount? Dismounting for combat.";
          var Dismount = true;
          WoWPoint? landPoint = null;
          return new Mount.ActionLandAndDismount(Reason, Dismount, landPoint);
        }

    }
}
