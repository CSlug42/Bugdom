using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bugdom;
using Bugdom.Critters;

namespace Bugdom.Critters.Hoppers
{
    public class SickleMembracidItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Grasshopper);

            Item.value = 1000;
            Item.rare = 2;
            Item.bait = 38;
            Item.makeNPC = ModContent.NPCType<SickleMembracid>();
            Item.SetNameOverride("Sickle Membracid");
        }
    }
}