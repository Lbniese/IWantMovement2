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
using Styx;
using Styx.Common;
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

        public override string Name { get { return "I Want Movement"; } }
        public override WoWClass Class { get { return StyxWoW.Me.Class; } }

        public override Composite PullBehavior { get { return CreatePullBehavior; } }
        public static Composite PullBehaviorHook = CreatePullBehavior;

        public override void Initialize()
        {
            
            //TreeHooks.Instance.AddHook("Combat_Pull", PullBehaviorHook);
        }

        /*
        public Dispose()
        {
            TreeHooks.Instance.RemoveHook("Combat_Pull", PullBehaviorHook);
        }
        */

        public static Composite CreatePullBehavior
        {
            get
            {

                Log.Debug("CreatePullBehavior Called");
                if (!Settings.IWMSettings.Instance.ForceCombat)
                    return new ActionAlwaysSucceed();

                var prio = new PrioritySelector();
                switch (StyxWoW.Me.Class)
                {
                    case WoWClass.Paladin:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast("Judgement");
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Monk:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast("Crackling Jade Lightning");
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Rogue:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast("Throw");
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Priest:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast("Shadow Word: Pain");
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Druid:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast("Moonfire");
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Shaman:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast("Lightning Bolt");
                            return RunStatus.Success;
                        }));
                        break;
                    case WoWClass.Mage:
                        prio.AddChild(new Action(delegate
                        {
                            SpellManager.Cast("Frostbolt");
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