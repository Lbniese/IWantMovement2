using System;
using System.Windows.Forms;
using IWantMovement2.Helper;
using IWantMovement2.Managers;
using IWantMovement2.Settings;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.CommonBot.Routines;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace IWantMovement2
{
    internal class WantMovement2 : HBPlugin
    {
        private static bool _initialized;

        private CombatRoutine _decoratedCombatRoutine;

        private Form _gui;
        private Targeting _previousTargetMethod;
        private Targeting _thisTargetMethod;
        private CombatRoutine _undecoratedCombatRoutine;

        private static LocalPlayer Me => StyxWoW.Me;
        private static string SvnRevision => "$Rev$";
        private static IwmSettings Settings => IwmSettings.Instance;

        public override void OnButtonPress()
        {
            if (_gui == null || _gui.IsDisposed || _gui.Disposing) _gui = new Gui();
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
            if (Me.IsDead || Me.IsFlying || Me.IsGhost)
            {
                return;
            }

            if (Settings.EnableAutoDismount && BotManager.Current.Name != "Questing")
            {
                if (Me.Mounted &&
                    ((BotPoi.Current.Type == PoiType.Kill && BotPoi.Current.AsObject.Distance < 30) ||
                     (Me.GotTarget && Me.CurrentTarget.Distance < 30 && !Me.CurrentTarget.IsFriendly)))
                {
                    Lua.DoString("Dismount()");
                }
                else
                {
                    if (Me.Mounted && Me.Combat && !Me.IsMoving && Me.GotTarget && Me.CurrentTarget.Distance < 30 &&
                        !Me.CurrentTarget.IsFriendly)
                    {
                        Lua.DoString("Dismount()");
                    }
                }
            }

            if (Me.IsOnTransport || Me.Mounted)
            {
                return;
            }

            if ((RoutineManager.Current != null) && (RoutineManager.Current != _decoratedCombatRoutine))
            {
                Log.Info("Installing Combat Routine Hook...");
                _undecoratedCombatRoutine = RoutineManager.Current;
                _decoratedCombatRoutine = new WantMovementCr(RoutineManager.Current, CapabilityFlags.All);
                RoutineManager.Current = _decoratedCombatRoutine;
                Log.Info("Combat Routine Hook Installed!");
            }

            if ((_thisTargetMethod != Targeting.Instance) && Settings.EnableTargeting && !Me.HasAura("Food") &&
                !Me.HasAura("Drink"))
            {
                Log.Warning(
                    "Taking control of targeting. If this message is being spammed, something else is trying to take control.");
                Targeting.Instance = _thisTargetMethod;
            }

            if (Targeting.Instance == _thisTargetMethod && Settings.EnableTargeting && !Me.GotTarget &&
                !Me.HasAura("Food") && !Me.HasAura("Drink"))
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
            if (Settings.EnableFacing && Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && !Me.IsMoving &&
                !Me.IsSafelyFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance <= 50)
            {
                Log.Info("[Facing: {0}] [Target HP: {1}] [Target Distance: {2}]", Me.CurrentTarget.Name,
                    Me.CurrentTarget.HealthPercent, Me.CurrentTarget.Distance);
                Me.CurrentTarget.Face();
            }
            Movement.Move();
        }

        #region Default Overrides

        public override string Author => "Lbniese";
        public override string ButtonText => "Settings";
        public override string Name => "I Want Movement 2";
        public override bool WantButton => true;
        public override Version Version => new Version(0, 0, 1);

        #endregion Default Overrides
    }
}