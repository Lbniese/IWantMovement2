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
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;

namespace IWantMovement.Managers
{
    public static class Movement
    {

        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        private static int MaxDistance { get { return Settings.IWMSettings.Instance.MaxDistance; } }
        private static int MinDistance { get { return Settings.IWMSettings.Instance.MinDistance; } }
        public static bool ValidatedSettings = false;
        private static bool MoveBehindTarget { get { return Settings.IWMSettings.Instance.MoveBehindTarget; } }

        public static bool CanMove()
        {
            return !Me.Stunned && !Me.Rooted && Me.HasAnyAura("Food", "Drink") && !Me.IsDead && !Me.IsFlying && !Me.IsOnTransport;
        }

        public static void ValidateSettings()
        {
            if (!ValidatedSettings)
            {
                if (Me.IsMelee() && MaxDistance > MeleeRange)
                {
                    Log.Warning("Your max distance setting is too high for a melee class. Reduce it!");
                }

                if (MaxDistance > 40)
                {
                    Log.Warning("Max distance is {0}! This is very high for any class!", MaxDistance);
                }
                if (MinDistance <= 2)
                {
                    Log.Warning("Min distance is {0}! This is very low for any class!", MinDistance);
                }
                if (MinDistance >= MaxDistance)
                {
                    Log.Warning("Your min distance [{0}] should be lower than max distance [{1}]. Consider changing.", MinDistance, MaxDistance);
                }
            }
        }

        public static bool NeedToMove()
        {
            return (Me.CurrentTarget.Distance > MaxDistance) && !Me.IsMoving;
        }

        public static bool NeedToStop()
        {
            return (Me.CurrentTarget.Distance <= MinDistance) && Me.IsMoving;
        }

        public static void Move()
        {
            // Check we don't have bad settings
            ValidateSettings();

            // If we don't have a target to move to
            if (Me.CurrentTarget == null) return;

            // Check if we're close enough.
            if (NeedToStop()) WoWMovement.MoveStop();

            // Check if we need to move
            if (!MoveBehindTarget && NeedToMove() && CanMove())
            {
                Navigator.MoveTo(Me.CurrentTarget.Location);
            }

            // Check if need to move, and want to be behind our target.
            if (MoveBehindTarget && NeedToMove() && CanMove())
            {
                Navigator.MoveTo(PointBehindTarget());
            }
        }


        #region Extentions
        public static bool HasAnyAura(this WoWUnit unit, params string[] auraNames)
        {
            var auras = unit.GetAllAuras();
            var hashes = new HashSet<string>(auraNames);
            return auras.Any(a => hashes.Contains(a.Name));
        }

        public static bool IsMelee(this WoWUnit unit)
        {
            {
                if (unit != null)
                {
                    switch (StyxWoW.Me.Class)
                    {
                        case WoWClass.Warrior:
                            return true;
                        case WoWClass.Paladin:
                            return StyxWoW.Me.Specialization != WoWSpec.PaladinHoly;
                        case WoWClass.Hunter:
                            return false;
                        case WoWClass.Rogue:
                            return true;
                        case WoWClass.Priest:
                            return false;
                        case WoWClass.DeathKnight:
                            return true;
                        case WoWClass.Shaman:
                            return StyxWoW.Me.Specialization == WoWSpec.ShamanEnhancement;
                        case WoWClass.Mage:
                            return false;
                        case WoWClass.Warlock:
                            return false;
                        case WoWClass.Druid:
                            return StyxWoW.Me.Specialization != WoWSpec.DruidRestoration &&
                                   StyxWoW.Me.Specialization != WoWSpec.DruidBalance;
                        case WoWClass.Monk:
                            return StyxWoW.Me.Specialization != WoWSpec.MonkMistweaver;
                    }
                }
                return false;
            }
        }
        #endregion

        #region Calculations
        private static WoWPoint PointBehindTarget()
        {
            return
                StyxWoW.Me.CurrentTarget.Location.RayCast(
                    StyxWoW.Me.CurrentTarget.Rotation + WoWMathHelper.DegreesToRadians(150), MeleeRange - 2f);
        }

        /// <summary>
        ///     Returns the current Melee range for the player Unit.DistanceToTargetBoundingBox(target)
        /// </summary>
        public static float MeleeRange
        {
            get
            {
                // If we have no target... then give nothing.
                // if (StyxWoW.Me.CurrentTargetGuid == 0)  // chg to GotTarget due to non-zero vals with no target in Guid
                if (!StyxWoW.Me.GotTarget)
                    return 0f;

                if (StyxWoW.Me.CurrentTarget.IsPlayer)
                    return 3.5f;

                return Math.Max(5f, StyxWoW.Me.CombatReach + 1.3333334f + StyxWoW.Me.CurrentTarget.CombatReach);
            }
        }
        #endregion

    }
}