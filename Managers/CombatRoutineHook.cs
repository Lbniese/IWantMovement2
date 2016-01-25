using System;
using CommonBehaviors.Actions;
using IWantMovement.Helper;
using IWantMovement.Settings;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace IWantMovement.Managers
{
    public class IWantMovementCR : CombatRoutine
    {

        private static IWMSettings Settings { get { return IWMSettings.Instance; } }

        private delegate T Selection<out T>(object context);
        private delegate WoWUnit UnitSelectionDelegate(object context);
        public delegate WoWPoint LocationRetriever(object context);

        #region CR Overrides
        public override string Name { get { return _undecoratedCr.Name; } }
        internal static readonly Version Version = new Version(0, 0, 1);
        public new string ButtonText { get { return _undecoratedCr.ButtonText; } }
        public override bool WantButton { get { return _undecoratedCr.WantButton; } }
        public override void OnButtonPress() { _undecoratedCr.OnButtonPress(); }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public override WoWClass Class { get { return StyxWoW.Me.Class; } }
        public override double? PullDistance { get { return Styx.Helpers.CharacterSettings.Instance.PullDistance; } }

        public override Composite CombatBehavior { get { return _undecoratedCr.CombatBehavior; } }
        public override Composite CombatBuffBehavior { get { return _undecoratedCr.CombatBuffBehavior; } }
        public override Composite DeathBehavior { get { return _undecoratedCr.DeathBehavior; } }
        public override Composite HealBehavior { get { return _undecoratedCr.HealBehavior; } }
        public override Composite MoveToTargetBehavior { get { return _undecoratedCr.MoveToTargetBehavior; } }
        public override Composite PreCombatBuffBehavior { get { return _undecoratedCr.PreCombatBuffBehavior; } }
        public override Composite PullBuffBehavior { get { return _undecoratedCr.PullBuffBehavior; } }
        public override Composite RestBehavior { get { return Managers.Rest.DefaultRestBehaviour(); } }
        public override Composite PullBehavior { get { return CreatePullBehavior; } }

        public override bool NeedDeath { get { return _undecoratedCr.NeedDeath; } }
        public override bool NeedHeal { get { return _undecoratedCr.NeedHeal; } }
        public override bool NeedCombatBuffs { get { return _undecoratedCr.NeedCombatBuffs; } }
        public override bool NeedRest { get { return Me.HealthPercent <= IWMSettings.Instance.EatPercent || (Me.PowerType == WoWPowerType.Mana && Me.ManaPercent <= IWMSettings.Instance.DrinkPercent); } }
        public override bool NeedPullBuffs { get { return _undecoratedCr.NeedPullBuffs; } }
        public override bool NeedPreCombatBuffs { get { return _undecoratedCr.NeedPreCombatBuffs; } }

        public override void Combat() { _undecoratedCr.Combat(); }
        public override void Death() { _undecoratedCr.Death(); }
        public override void Heal() { _undecoratedCr.Heal(); }
        public override void CombatBuff() { _undecoratedCr.CombatBuff(); }
        public override void PreCombatBuff() { _undecoratedCr.PreCombatBuff(); }
        public override void Rest() { IwmRestBehavior(); _undecoratedCr.Rest(); }
        public override void Pulse() { _undecoratedCr.Pulse(); }
        public override void ShutDown() { _undecoratedCr.ShutDown(); }
        public override void PullBuff() { _undecoratedCr.PullBuff(); }
        public override void Pull() { IwmPullBehavior(); _undecoratedCr.Pull(); }

        private Composite IwmPullBehavior() { return CreatePullBehavior; }
        private Composite IwmRestBehavior() { return Managers.Rest.DefaultRestBehaviour(); }
        #endregion

        readonly CombatRoutine _undecoratedCr;
        public IWantMovementCR(CombatRoutine undecoratedCr)
        {
            if (undecoratedCr == null) return;
            Log.Info("Storing Combat Routine");
            _undecoratedCr = undecoratedCr;
        }

        public void OnEnable()
        {

        }

        public void OnDisable()
        {
            Log.Warning("Disposing IWM2 Combat Routine Hook");
            RoutineManager.Current = _undecoratedCr;
        }

        private static Composite CreatePullBehavior
        {
            get
            {
                Movement.Move();

                if (Settings.EnableFacing && Me.CurrentTarget != null && !Me.CurrentTarget.IsDead && !Me.IsMoving && !Me.IsSafelyFacing(Me.CurrentTarget) && Me.CurrentTarget.Distance <= 50)
                {
                    Log.Info("[Facing: {0}] [Target HP: {1}] [Target Distance: {2}]", Me.CurrentTarget.SafeName, Me.CurrentTarget.HealthPercent, Me.CurrentTarget.Distance);
                    Me.CurrentTarget.Face();
                }


                if (StyxWoW.Me.CurrentTarget != null && !Me.IsCasting && !Me.IsChanneling) { Log.Info("[Pulling] [Attacking: {0}]", StyxWoW.Me.CurrentTarget.SafeName); }

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

        private static Composite Cast(string spell, UnitSelectionDelegate onUnit, Selection<bool> reqs = null)
        {
            return
                new Decorator(
                    ret => (onUnit != null && onUnit(ret) != null &&
                        ((reqs != null && reqs(ret)) || (reqs == null)) &&
                        SpellManager.CanCast(spell, onUnit(ret), true)),
                    new Sequence(
                        new Action(ret => SpellManager.Cast(spell, onUnit(ret))),
                        new Action(ret => Log.Info(String.Format("[Casting:{0}] [Target:{1}] [Distance:{2:F1}yds]", spell, onUnit(ret).SafeName, onUnit(ret).Distance)))
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
                        Log.Info("[Failed to Cast: {0}] [Target: {1}] [In LoS: {2}]", spellName, StyxWoW.Me.CurrentTarget.SafeName, StyxWoW.Me.CurrentTarget.InLineOfSpellSight);
                    }
                }
                {
                    if (!Me.IsCasting || !Me.IsChanneling)
                        Log.Debug("[CanCast Failed for: {0}] [Target: {1}] [In LoS: {2}]", spellName, StyxWoW.Me.CurrentTarget.SafeName, StyxWoW.Me.CurrentTarget.InLineOfSpellSight);
                }
            }
        }

    }
}
