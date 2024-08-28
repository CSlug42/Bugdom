using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bugdom;
using Bugdom.Critters;
using System.Collections.Generic;

namespace Bugdom.Accessories.Attractants
{
    public class BugFragment : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 0;
            Item.rare = -1;
            Item.bait = 1;
            Item.maxStack = 9999;
            Item.consumable = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(Mod, "descriptor", "A leg, a scale, maybe an antennae. Whatever it is, it's not a whole bug.");
            tooltips.Add(line);
        }
    }
}