using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using IWantMovement2.Settings;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.WoWInternals;
using Styx.WoWInternals.DBC;
using Styx.WoWInternals.WoWObjects;

namespace IWantMovement2.Managers
{
    internal class Target : Targeting
    {
        private static readonly Map Map = Me.CurrentMap;
        private static DateTime _targetLast;
        private static LocalPlayer Me => StyxWoW.Me;

        private static bool WantTarget()
        {
            return (DateTime.UtcNow > _targetLast.AddMilliseconds(IwmSettings.Instance.TargetingThrottleTime))
                   && (!Me.GotTarget || Me.CurrentTarget.IsDead)
                   && !Me.Stunned
                   && !Me.Rooted
                   && !Me.HasAnyAura("Food", "Drink")
                   && !Me.IsDead
                   && !Me.IsFlying
                   && !Me.IsOnTransport
                ;
        }


        public static void AquireTarget()
        {
            if (!WantTarget()) return;

            WoWUnit unit;
            _targetLast = DateTime.UtcNow;
            if (Map.IsBattleground || Map.IsArena)
            {
                if (Me.Combat || (Me.GotAlivePet && Me.Pet.Combat))
                {
                    unit =
                        NearbyAttackableUnitsAttackingMe(Me.Location, 40)
                            .FirstOrDefault(u => u != null && u.IsPlayer && u.IsHostile && u.Attackable);
                    if (unit != null)
                    {
                        unit.Target();
                        Helper.Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.SafeName,
                            unit.HealthPercent, unit.Distance);
                        return;
                    }
                }

                unit =
                    NearbyAttackableUnits(Me.Location, 40)
                        .FirstOrDefault(
                            u => u != null && u.IsPlayer && u.IsHostile && u.InLineOfSpellSight && u.Attackable);
                if (unit != null)
                {
                    unit.Target();
                    Helper.Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.SafeName,
                        unit.HealthPercent, unit.Distance);
                    return;
                }
            }

            if (Map.IsInstance || Map.IsDungeon || Map.IsRaid || Map.IsScenario)
            {
                if (Me.Combat || (Me.GotAlivePet && Me.Pet.Combat))
                {
                    unit =
                        NearbyAttackableUnitsAttackingUs(Me.Location, 40)
                            .FirstOrDefault(u => u != null && u.IsHostile && u.InLineOfSpellSight && u.Attackable);
                    if (unit != null)
                    {
                        unit.Target();
                        Helper.Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.SafeName,
                            unit.HealthPercent, unit.Distance);
                        return;
                    }
                }
            }

            unit =
                NearbyAttackableUnits(Me.Location, 25)
                    .FirstOrDefault(
                        u =>
                            u != null && u.IsHostile && u.Attackable && (Me.IsSafelyFacing(u) || u.IsTargetingMeOrPet) &&
                            u.InLineOfSpellSight);
            if (unit != null)
            {
                unit.Target();
                Helper.Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.SafeName,
                    unit.HealthPercent, unit.Distance);
            }
        }

        public static void ClearTarget()
        {
            if (Me.CurrentTarget == null)
            {
                return;
            }

            if (Me.CurrentTarget.IsDead && !Me.CurrentTarget.HasAura("Feign Death") && !Me.Looting &&
                BotPoi.Current.Type != PoiType.Loot)
            {
                Helper.Log.Info("[Clearing {0}] [Reason: Dead]", Me.CurrentTarget.SafeName);
                Me.ClearTarget();
            }

            if (IwmSettings.Instance.ClearTargetIfNotTargetingGroup && (Me.Combat || Me.PetInCombat) &&
                !Me.CurrentTarget.IsDead && !IsTargetingUs(Me.CurrentTarget))
            {
                Helper.Log.Info("[Clearing {0}] [Reason: In combat - target isn't targeting us or group member]",
                    Me.CurrentTarget.SafeName);
                Me.ClearTarget();
            }
        }

        private static bool IsTargetingUs(WoWUnit unit)
        {
            return unit.GotTarget && (
                unit.IsTargetingAnyMinion || unit.IsTargetingMeOrPet || unit.IsTargetingMyPartyMember ||
                unit.IsTargetingMyRaidMember);
        }

        #region Core Unit Checks

        private static IEnumerable<WoWUnit> AttackableUnits
        {
            get
            {
                return
                    ObjectManager.GetObjectsOfType<WoWUnit>(true, false)
                        .Where(
                            u =>
                                u != null && u.IsValid && u.Attackable && u.CanSelect && !u.IsFriendly && !u.IsDead &&
                                !u.IsNonCombatPet && !u.IsCritter && u.Distance <= 50);
            }
        }

        private static IEnumerable<WoWUnit> NearbyAttackableUnits(Vector3 fromLocation, double radius)
        {
            var hostile = AttackableUnits;
            var maxDistance = radius*radius;
            return hostile.Where(x => x.Location.DistanceSquared(fromLocation) < maxDistance);
        }

        private static IEnumerable<WoWUnit> NearbyAttackableUnitsAttackingUs(Vector3 fromLocation, double radius)
        {
            var hostile = AttackableUnits;
            var maxDistance = radius*radius;
            return
                hostile.Where(
                    x =>
                        x.Location.DistanceSquared(fromLocation) < maxDistance &&
                        (x.IsTargetingMyPartyMember || x.IsTargetingMeOrPet || x.IsTargetingAnyMinion ||
                         x.IsTargetingMyRaidMember || x.IsTargetingPet));
        }

        private static IEnumerable<WoWUnit> NearbyAttackableUnitsAttackingMe(Vector3 fromLocation, double radius)
        {
            var hostile = AttackableUnits;
            var maxDistance = radius*radius;
            return hostile.Where(x => x.Location.DistanceSquared(fromLocation) < maxDistance && x.IsTargetingMeOrPet);
        }

        #endregion Core Unit Checks
    }
}