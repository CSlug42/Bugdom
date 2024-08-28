using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bugdom.Accessories.Attractants.Prefixes
{
    public class Rich : Potent
    {
        public override float Power => 1f;

        // Modify the cost of items with this modifier with this function.
        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1.2f * Power/2;
        }

        // Use this function to modify these stats for items which have this prefix:
        // Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            //
        }

        // This prefix doesn't affect any non-standard stats, so these additional tooltiplines aren't actually necessary, but this pattern can be followed for a prefix that does affect other stats.
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {
            yield return new TooltipLine(Mod, "description", $"Allows {Power} additional critter{(Power != 1 ? "s" : "")} to be spawned")
            {
                IsModifier = true, // Sets the color to the positive modifier color.
            };
        }

        public override void ApplyAccessoryEffects(Player player)
        {
            player.GetModPlayer<Attractants.BugCatcher>().bugExtra += Power;
        }
    }
}