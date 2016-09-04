using System;
using System.Numerics;
using IWantMovement2.Helper;
using IWantMovement2.Settings;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace IWantMovement2.Managers
{
    public class WantMovementCr : CombatRoutine
    {
        public delegate Vector3 LocationRetriever(object context);

        private readonly CombatRoutine _undecoratedCr;

        public WantMovementCr(CombatRoutine undecoratedCr, CapabilityFlags supportedCapabilities)
        {
            if (undecoratedCr == null) return;
            Log.Info("Storing Combat Routine");
            _undecoratedCr = undecoratedCr;
            SupportedCapabilities = supportedCapabilities;
        }

        private static IwmSettings Settings => IwmSettings.Instance;

        private static Composite CreatePullBehavior
        {
            get
            {
                Movement.Move();

                if (Settings.EnableFacing && Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && !Me.IsMoving &&
                    !Me.IsSafelyFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance <= 50)
                {
                    Log.Info("[Facing: {0}] [Target HP: {1}] [Target Distance: {2}]", Me.CurrentTarget.SafeName,
                        Me.CurrentTarget.HealthPercent, Me.CurrentTarget.Distance);
                    Me.CurrentTarget.Face();
                }


                if (StyxWoW.Me.CurrentTarget != null && !Me.IsCasting && !Me.IsChanneling)
                {
                    Log.Info("[Pulling] [Attacking: {0}]", StyxWoW.Me.CurrentTarget.SafeName);
                }

                if (!Me.HasAura(Settings.PullSpell1))
                {
                    Cast(Settings.PullSpell1);
                }

                if (!Me.HasAura(Settings.PullSpell2))
                {
                    Cast(Settings.PullSpell2);
                }

                if (!Me.HasAura(Settings.PullSpell3))
                {
                    Cast(Settings.PullSpell3);
                }

                return
                    new PrioritySelector();
            }
        }

        public void OnEnable()
        {
        }

        public void OnDisable()
        {
            Log.Warning("Disposing IWM2 Combat Routine Hook");
            RoutineManager.Current = _undecoratedCr;
        }

        private static Composite Cast(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return
                new Decorator(
                    ret => onUnit != null && onUnit(ret) != null &&
                           ((reqs != null && reqs(ret)) || (reqs == null)) &&
                           SpellManager.CanCast(spell, onUnit(ret), true),
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(
                            ret =>
                                Log.Info(string.Format("[Casting:{0}] [Target:{1}] [Distance:{2:F1}yds]", spell,
                                    onUnit(ret).SafeName, onUnit(ret).Distance)))
                        ));
        }

        private static void Cast(string spellName)
        {
            if (StyxWoW.Me.CurrentTarget != null && !Me.IsCasting && !Me.IsChanneling)
            {
                if (SpellManager.CanCast(spellName, StyxWoW.Me.CurrentTarget))
                {
                    Log.Info("[Casting: {0}] [Target: {1}] ", spellName, StyxWoW.Me.CurrentTarget.SafeName);
                    var result = SpellManager.Cast(spellName);

                    if (!result)
                    {
                        Log.Info("[Failed to Cast: {0}] [Target: {1}] [In LoS: {2}]", spellName,
                            StyxWoW.Me.CurrentTarget.SafeName, StyxWoW.Me.CurrentTarget.InLineOfSpellSight);
                    }
                }
                {
                    if (!Me.IsCasting || !Me.IsChanneling)
                        Log.Debug("[CanCast Failed for: {0}] [Target: {1}] [In LoS: {2}]", spellName,
                            StyxWoW.Me.CurrentTarget.SafeName, StyxWoW.Me.CurrentTarget.InLineOfSpellSight);
                }
            }
        }

        private delegate T Selection<out T>(object context);

        private delegate WoWUnit UnitSelectionDelegate(object context);

        #region CR Overrides

        public override string Name => _undecoratedCr.Name;
        internal static readonly Version Version = new Version(0, 0, 1);
        public new string ButtonText => _undecoratedCr.ButtonText;
        public override bool WantButton => _undecoratedCr.WantButton;

        public override void OnButtonPress()
        {
            _undecoratedCr.OnButtonPress();
        }

        private static LocalPlayer Me => StyxWoW.Me;

        public override WoWClass Class => StyxWoW.Me.Class;
        public override double PullDistance => CharacterSettings.Instance.PullDistance;

        public override Composite CombatBehavior => _undecoratedCr.CombatBehavior;
        public override Composite CombatBuffBehavior => _undecoratedCr.CombatBuffBehavior;
        public override Composite DeathBehavior => _undecoratedCr.DeathBehavior;
        public override Composite HealBehavior => _undecoratedCr.HealBehavior;
        public override Composite MoveToTargetBehavior => _undecoratedCr.MoveToTargetBehavior;
        public override CapabilityFlags SupportedCapabilities { get; }
        public override Composite PreCombatBuffBehavior => _undecoratedCr.PreCombatBuffBehavior;
        public override Composite PullBuffBehavior => _undecoratedCr.PullBuffBehavior;
        public override Composite RestBehavior => Managers.Rest.DefaultRestBehaviour();
        public override Composite PullBehavior => CreatePullBehavior;

        public override bool NeedDeath => _undecoratedCr.NeedDeath;
        public override bool NeedHeal => _undecoratedCr.NeedHeal;
        public override bool NeedCombatBuffs => _undecoratedCr.NeedCombatBuffs;

        public override bool NeedRest
            =>
                Me.HealthPercent <= IwmSettings.Instance.EatPercent ||
                (Me.PowerType == WoWPowerType.Mana && Me.ManaPercent <= IwmSettings.Instance.DrinkPercent);

        public override bool NeedPullBuffs => _undecoratedCr.NeedPullBuffs;
        public override bool NeedPreCombatBuffs => _undecoratedCr.NeedPreCombatBuffs;

        public override void Combat()
        {
            _undecoratedCr.Combat();
        }

        public override void Death()
        {
            _undecoratedCr.Death();
        }

        public override void Heal()
        {
            _undecoratedCr.Heal();
        }

        public override void CombatBuff()
        {
            _undecoratedCr.CombatBuff();
        }

        public override void PreCombatBuff()
        {
            _undecoratedCr.PreCombatBuff();
        }

        public override void Rest()
        {
            IwmRestBehavior();
            _undecoratedCr.Rest();
        }

        public override void Pulse()
        {
            _undecoratedCr.Pulse();
        }

        public override void ShutDown()
        {
            _undecoratedCr.ShutDown();
        }

        public override void PullBuff()
        {
            _undecoratedCr.PullBuff();
        }

        public override void Pull()
        {
            IwmPullBehavior();
            _undecoratedCr.Pull();
        }

        private Composite IwmPullBehavior()
        {
            return CreatePullBehavior;
        }

        private Composite IwmRestBehavior()
        {
            return Managers.Rest.DefaultRestBehaviour();
        }

        #endregion
    }
}