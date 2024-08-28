using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bugdom.Accessories.Attractants.AppleCiderVinegar
{
    public class AppleCiderVinegar : Attractant
    {

        public override void SetDefaults()
        {
            bugType = ModContent.NPCType<FruitFly>();
            bugMax = 4;
            Item.value = 500;
            Item.rare = 1;
            Item.useTime = 120;

            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddIngredient(ItemID.Apple, 1);
            recipe.AddIngredient(ItemID.Deathweed, 1);
            recipe.AddTile(94);
            recipe.Register();
        }
    }
}