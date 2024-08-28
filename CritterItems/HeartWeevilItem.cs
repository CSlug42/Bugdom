using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bugdom;
using Bugdom.Critters;

namespace Bugdom.CritterItems
{
    public class HeartWeevilItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 10000;
            Item.rare = 2;
            Item.bait = 38;
            Item.maxStack = 999;
            Item.consumable = true;
        }
    }
}