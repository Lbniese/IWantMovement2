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
using Action = Styx.TreeSharp.Action;

namespace IWantMovement.Managers
{
// ReSharper disable InconsistentNaming
    public class IWantMovement : CombatRoutine
// ReSharper restore InconsistentNaming
    {
        internal static readonly Version Version = new Version(0, 0, 1);
        //internal static bool EnablePullSpells = true;
        private static IWMSettings Settings { get { return IWMSettings.Instance; } }

        public override string Name { get { return "I Want Movement"; } }
        public override WoWClass Class { get { return StyxWoW.Me.Class; } }

        public override Composite PullBehavior { get { return CreatePullBehavior; } }
        public static Composite PullBehaviorHook = CreatePullBehavior;

        public static Composite CreatePullBehavior
        {
            get
            {


                if (!Settings.ForceCombat)
                {
                    Log.Info("[Pulling] [Preventing Pull]");
                    return new ActionAlwaysSucceed();
                }

                if (StyxWoW.Me.CurrentTarget != null) { Log.Info("[Pulling] [Attacking: {0}]", StyxWoW.Me.CurrentTarget.Name); }

                var prio = new PrioritySelector();
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Paladin:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellPaladin);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Monk:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellMonk);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Rogue:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellRogue);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Priest:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellPriest);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Druid:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellDruid);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Shaman:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellShaman);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Mage:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellMage);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.DeathKnight:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellDeathKnight);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Warlock:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellWarlock);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Warrior:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellWarrior);
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Hunter:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast(Settings.PullSpellHunter);
                            return RunStatus.Success;
                        }));
                        break;
                }

                return new Sequence(
                    new Action(delegate
                    {
                        if (StyxWoW.Me.CurrentTarget != null) StyxWoW.Me.CurrentTarget.Face();
                        return RunStatus.Failure;
                    }),
                    prio
                    );
            }
        }

    }
}