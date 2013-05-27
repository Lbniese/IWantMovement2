#region Revision info
/*
 * $Author: millz $
 * $Date: 2013-04-26 12:18:24 +0100 (Fri, 26 Apr 2013) $
 * $ID: $
 * $Revision: 16 $
 * $URL: https://subversion.assembla.com/svn/iwantmovement/trunk/IWantMovement/Managers/Target.cs $
 * $LastChangedBy: millz $
 * $ChangesMade: $
 */
#endregion

using System;
using CommonBehaviors.Actions;
using IWantMovement.Helper;
using IWantMovement.Settings;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Styx.WoWInternals.WoWObjects;

namespace IWantMovement.Managers
{
// ReSharper disable InconsistentNaming
    public class IWantMovementCR : ICombatRoutine
// ReSharper restore InconsistentNaming
    {
        
        //internal static bool EnablePullSpells = true;
        private static IWMSettings Settings { get { return IWMSettings.Instance; } }

        #region CR Overrides
        public string Name { get { return _undecoratedCR.Name; } }
        internal static readonly Version Version = new Version(0, 0, 1);
        public string ButtonText { get { return _undecoratedCR.ButtonText; } }
        public bool WantButton { get { return _undecoratedCR.WantButton; } }
        public void OnButtonPress() { _undecoratedCR.OnButtonPress(); }

        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        public WoWClass Class { get { return StyxWoW.Me.Class; } }
        public double? PullDistance { get { return Styx.Helpers.CharacterSettings.Instance.PullDistance; } }
        
        public Composite CombatBehavior { get { return _undecoratedCR.CombatBehavior; } }
        public Composite CombatBuffBehavior { get { return _undecoratedCR.CombatBuffBehavior; } }
        public Composite DeathBehavior { get { return _undecoratedCR.DeathBehavior; } }
        public Composite HealBehavior { get { return _undecoratedCR.HealBehavior; } }
        public Composite MoveToTargetBehavior { get { return _undecoratedCR.MoveToTargetBehavior; } }
        public Composite PreCombatBuffBehavior { get { return _undecoratedCR.PreCombatBuffBehavior; } }
        public Composite PullBuffBehavior { get { return _undecoratedCR.PullBuffBehavior; } }
        public Composite RestBehavior { get { return Managers.Rest.DefaultRestBehaviour(); }} // Hook into rest behavior
        public Composite PullBehavior { get { return CreatePullBehavior; } } // Hook into Pull Behavior

        public bool NeedDeath { get { return _undecoratedCR.NeedDeath; } }
        public bool NeedHeal { get { return _undecoratedCR.NeedHeal; } }
        public bool NeedCombatBuffs { get { return _undecoratedCR.NeedCombatBuffs; } }
        public bool NeedRest { get { return Me.HealthPercent <= 80 || (Me.PowerType == WoWPowerType.Mana && Me.ManaPercent <= 60); } }
        public bool NeedPullBuffs { get { return _undecoratedCR.NeedPullBuffs; } }
        public bool NeedPreCombatBuffs { get { return _undecoratedCR.NeedPreCombatBuffs; } }

        public void Combat() { _undecoratedCR.Combat(); }
        public void Death() { _undecoratedCR.Death(); }
        public void Heal() { _undecoratedCR.Heal(); }
        public void CombatBuff() { _undecoratedCR.CombatBuff(); }
        public void PreCombatBuff() { _undecoratedCR.PreCombatBuff(); }
        public void Rest() { IwmRestBehavior(); _undecoratedCR.Rest(); } // Hook into rest behavior
        public void Pulse() { _undecoratedCR.Pulse(); }
        public void ShutDown() { _undecoratedCR.ShutDown(); }
        public void PullBuff() { _undecoratedCR.PullBuff(); }
        public void Pull() { IwmPullBehavior(); _undecoratedCR.Pull(); } // Hook into pull behavior

        private Composite IwmPullBehavior() { return CreatePullBehavior; }
        private Composite IwmRestBehavior() { return Managers.Rest.DefaultRestBehaviour(); }
        #endregion

        readonly ICombatRoutine _undecoratedCR;
        public IWantMovementCR(ICombatRoutine undecoratedCR)

        {
            if (undecoratedCR != null)
            {
                Log.Info("Storing Combat Routine");
                _undecoratedCR = undecoratedCR;
            }
            //{
            //    Log.Warning("Current Combat Routine is null - Override behavior may not work correctly.");
            //}
        }
        
        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            Log.Warning("Disposing IWM Combat Routine Hook");
            RoutineManager.Current = _undecoratedCR;
        }

        private Composite CreatePullBehavior
        {
            get
            {
                Movement.Move();

                if (!Settings.ForceCombat)
                {
                    Log.Info("[Pull Called - Preventing] [Reason: Setting Disabled]");
                    return new ActionAlwaysSucceed();
                }

                if (StyxWoW.Me.CurrentTarget != null && !Me.IsCasting && !Me.IsChanneling) { Log.Info("[Pulling] [Attacking: {0}]", StyxWoW.Me.CurrentTarget.Name); }


                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Paladin:
                        Cast(Settings.PullSpellPaladin);
                        break;
                    case WoWClass.Monk:
                        Cast(Settings.PullSpellMonk);
                        break;
                    case WoWClass.Rogue:
                        Cast(Settings.PullSpellRogue);
                        break;
                    case WoWClass.Priest:
                        Cast(Settings.PullSpellPriest);
                        break;
                    case WoWClass.Druid:
                        Cast(Settings.PullSpellDruid);
                        break;
                    case WoWClass.Shaman:
                        Cast(Settings.PullSpellShaman);
                        break;
                    case WoWClass.Mage:
                        Cast(Settings.PullSpellMage);
                        break;
                    case WoWClass.DeathKnight:
                        Cast(Settings.PullSpellDeathKnight);
                        break;
                    case WoWClass.Warlock:
                        Cast(Settings.PullSpellWarlock);
                        break;
                    case WoWClass.Warrior:
                        Cast(Settings.PullSpellWarrior);
                        break;
                    case WoWClass.Hunter:
                        Cast(Settings.PullSpellHunter);
                        break;
                }

                return new PrioritySelector();

            }
        }

        private static void Cast(string spellName)
        {
            if (StyxWoW.Me.CurrentTarget != null && !Me.IsCasting && !Me.IsChanneling)
            {
                if (SpellManager.CanCast(spellName, StyxWoW.Me.CurrentTarget))
                {
                    Log.Info("[Casting: {0}] [Target: {1}] ", spellName, StyxWoW.Me.CurrentTarget);
                    var result = SpellManager.Cast(spellName);

                    if (!result)
                    {
                        Log.Info("[Failed to Cast: {0}] [Target: {1}] [In LoS: {2}]", spellName, StyxWoW.Me.CurrentTarget, StyxWoW.Me.CurrentTarget.InLineOfSpellSight);
                    }
                }
                {
                    if (!Me.IsCasting || !Me.IsChanneling)
                        Log.Debug("[CanCast Failed for: {0}] [Target: {1}] [In LoS: {2}]", spellName, StyxWoW.Me.CurrentTarget, StyxWoW.Me.CurrentTarget.InLineOfSpellSight);
                }
            }
        }

    }
}