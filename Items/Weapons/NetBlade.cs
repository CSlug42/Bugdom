using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bugdom;
using Bugdom.Critters;

namespace Bugdom.Items.Weapons
{
    public class NetBlade : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 5;
            Item.useAnimation = 20;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = 10000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            ItemID.Sets.CatchingTool[Item.type] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        //public override bool? UseItem(Player player)
        //{
        //    float xCoord = player.headPosition.X;
        //    float yCoord = player.headPosition.Y;
        //    NPC npc = new NPC();
        //    npc.NewNPC(new EntitySource_Misc(""), (int)xCoord, (int)yCoord, 356, 0, 0f, 0f, 0f, 0f, 255);
        //    ModNPC zomNpc = new ExampleZombieThief();
        //    zomNpc.SpawnNPC((int)xCoord, (int)yCoord);
        //    return true;
        //}
    }
}