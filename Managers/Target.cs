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

using System;
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
        public static DateTime _targetLast;

        public static bool WantTarget()
        {
            return (DateTime.UtcNow > _targetLast.AddMilliseconds(Settings.IWMSettings.Instance.TargetingThrottleTime)) 
                && !Me.GotTarget 
                && !Me.Stunned && !Me.Rooted 
                && !Me.HasAnyAura("Food", "Drink") 
                && !Me.IsDead 
                && !Me.IsFlying && !Me.IsOnTransport;
        }


        public static void AquireTarget()
        {
            Log.Debug("[Want A Target:{0}]", WantTarget());
            if (!WantTarget()) return;
            
            WoWUnit unit;
            _targetLast = DateTime.UtcNow;
            if (Map.IsBattleground || Map.IsArena)
            {
                
                if (Me.IsActuallyInCombat || (Me.GotAlivePet && Me.Pet.PetInCombat))
                {
                    // get a pvp unit attacking me
                    unit = NearbyAttackableUnitsAttackingMe(Me.Location, 40).FirstOrDefault(u => u != null && u.IsPlayer && u.IsHostile);
                    if (unit != null) 
                    {
                        unit.Target();
                        Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.Name, unit.HealthPercent, unit.Distance);
                        return;
                    }
                    
                }

                // return closest pvp unit
                unit = NearbyAttackableUnits(Me.Location, 40).FirstOrDefault(u => u != null && u.IsPlayer && u.IsHostile && u.InLineOfSpellSight);
                if (unit != null)
                {
                    unit.Target();
                    Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.Name, unit.HealthPercent, unit.Distance);
                    return;
                }
                

            }

            if (Map.IsInstance || Map.IsDungeon || Map.IsRaid)
            {
                if (Me.IsActuallyInCombat || (Me.GotAlivePet && Me.Pet.PetInCombat))
                {
                    // get unit attacking party
                    unit = NearbyAttackableUnitsAttackingUs(Me.Location, 40).FirstOrDefault(u => u != null && u.IsHostile && u.InLineOfSpellSight);
                    if (unit != null)
                    {
                        unit.Target();
                        Log.Info("[Targetting: {0}] [Target HP: {1}] [Target Distance: {2}]", unit.Name, unit.HealthPercent, unit.Distance);
                        return;
                    }
                    
                }

            }

            unit = NearbyAttackableUnits(Me.Location, 25).FirstOrDefault(u => u != null && u.IsHostile && ((Me.IsSafelyFacing(u) || u.IsTargetingMeOrPet) && u.InLineOfSpellSight));
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
