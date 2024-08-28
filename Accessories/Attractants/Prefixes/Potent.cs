using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bugdom.Accessories.Attractants.Prefixes
{
    public class Potent : ModPrefix
    {
        public virtual float Power => 0.8f;
        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override float RollChance(Item item)
        {
            return 1.5f;
        }

        public override bool CanRoll(Item item)
        {
            return Attractant.CheckIsAttractant(item);
        }

        // Use this function to modify these stats for items which have this prefix:
        // Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            useTimeMult *= 1f * Power;
        }

        // Modify the cost of items with this modifier with this function.
        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1f + 0.05f / Power;
        }

        // This is used to modify most other stats of items which have this modifier.
        public override void Apply(Item item)
        {
            //
        }
    }
}