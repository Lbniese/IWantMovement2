using CommonBehaviors.Actions;
using IWantMovement.Helper;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;

namespace IWantMovement.Managers
{
    class IWantMovement : CombatRoutine
    {
        //internal static readonly Version Version = new Version(0, 0, 1);
        //internal static bool EnablePullSpells = true;

        public override string Name { get { return "I Want Movement"; } }
        public override WoWClass Class { get { return StyxWoW.Me.Class; } }

        public override Composite PullBehavior { get { return CreatePullBehavior; } }

        public static Composite CreatePullBehavior
        {
            get
            {

                Log.Debug("CreatePullBehavior Called");
                if (!Settings.IWMSettings.Instance.ForceCombat)
                    return new ActionAlwaysFail();

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