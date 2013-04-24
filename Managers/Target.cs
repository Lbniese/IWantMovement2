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

using System.Collections.Generic;
using System.Linq;
using IWantMovement.Helper;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.DBC;
using Styx.WoWInternals.WoWObjects;

namespace IWantMovement.Managers
{
    class Target : Targeting
    {
        public static LocalPlayer Me { get { return StyxWoW.Me; } }
        private readonly static Map Map = Me.CurrentMap;

        public static void AquireTarget()
        {
            WoWUnit unit;
            if (Map.IsBattleground || Map.IsArena)
            {
                
                if (Me.IsActuallyInCombat)
                {
                    // get a pvp unit attacking me
                    unit = NearbyAttackableUnitsAttackingMe(Me.Location, 40).FirstOrDefault(u => u != null && u.IsPlayer);
                    if (unit != null) 
                    {
                        unit.Target();
                        Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.Name, unit.HealthPercent, unit.Distance);
                    }
                    return;
                }

                // return closest pvp unit
                unit = NearbyAttackableUnits(Me.Location, 50).FirstOrDefault(u => u != null && u.IsPlayer);
                if (unit != null)
                {
                    unit.Target();
                    Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.Name, unit.HealthPercent, unit.Distance);
                }
                return;

            }

            if (Map.IsInstance || Map.IsDungeon || Map.IsRaid)
            {
                if (Me.IsActuallyInCombat)
                {
                    // get unit attacking party
                    unit = NearbyAttackableUnitsAttackingUs(Me.Location, 40).FirstOrDefault(u => u != null);
                    if (unit != null)
                    {
                        unit.Target();
                        Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.Name, unit.HealthPercent, unit.Distance);
                    }
                    return;
                }


                // return closest unit
                unit = NearbyAttackableUnits(Me.Location, 40).FirstOrDefault(u => u != null);
                if (unit != null)
                {
                    unit.Target();
                    Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.Name, unit.HealthPercent, unit.Distance);
                }
                return;

            }

            // return closest unit

            unit = NearbyAttackableUnits(Me.Location, 50).FirstOrDefault(u => u != null);
            if (unit != null)
            {
                unit.Target();
                Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.Name, unit.HealthPercent, unit.Distance);
            }

        }

        #region Core Unit Checks
        internal static IEnumerable<WoWUnit> AttackableUnits
        {
            get { return ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Where(u => u.Attackable && u.CanSelect && !u.IsFriendly && !u.IsDead && !u.IsNonCombatPet && !u.IsCritter && u.Distance <= 50); }
        }

        internal static IEnumerable<WoWUnit> NearbyAttackableUnits(WoWPoint fromLocation, double radius)
        {
            var hostile = AttackableUnits;
            var maxDistance = radius * radius;
            return hostile.Where(x => x.Location.DistanceSqr(fromLocation) < maxDistance);
        }

        internal static IEnumerable<WoWUnit> NearbyAttackableUnitsAttackingUs(WoWPoint fromLocation, double radius)
        {
            var hostile = AttackableUnits;
            var maxDistance = radius * radius;
            return hostile.Where(x => x.Location.DistanceSqr(fromLocation) < maxDistance && (x.IsTargetingMyPartyMember || x.IsTargetingMeOrPet || x.IsTargetingAnyMinion || x.IsTargetingMyRaidMember || x.IsTargetingPet));
        }

        internal static IEnumerable<WoWUnit> NearbyAttackableUnitsAttackingMe(WoWPoint fromLocation, double radius)
        {
            var hostile = AttackableUnits;
            var maxDistance = radius * radius;
            return hostile.Where(x => x.Location.DistanceSqr(fromLocation) < maxDistance && x.IsTargetingMeOrPet);
        }
        #endregion Core Unit Checks
    }
}
