using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bugdom;
using Bugdom.Critters;
using Terraria.DataStructures;
using System;

namespace Bugdom.Critters.Hoppers
{
    public class Cresicle : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Melee;
            Item.width = 18;
            Item.height = 18;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.useStyle = 1;
            Item.knockBack = 3;
            Item.value = 10000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Melee;

            Item.shoot = ModContent.ProjectileType<MiniSickle>();
            Item.shootsEveryUse = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Sickle, 5);
            recipe.AddIngredient(ItemID.DeathweedSeeds, 3);
            recipe.AddIngredient(ModContent.ItemType<SickleMembracidItem>(), 1);
        }

        public override bool? UseItem(Player player)
        {

            // check for nearby non-blooming deathweeds and make them bloom
            for (int i = -4; i < 4; i += 1)
            {
                for (int j = -4; j < 4; j += 1)
                {
                    Terraria.Tile tile = Main.tile[(ushort)Math.Round(player.Center.X/16 + j), (ushort)Math.Round(player.Center.Y/16 + i)];
                    if (tile.TileType == TileID.MatureHerbs && tile.TileFrameX == 54)
                    {
                        tile.ResetToType(TileID.BloomingHerbs);
                        tile.TileFrameX = 54;
                    }
                    else if (tile.TileType == TileID.Plants || tile.TileType == TileID.Plants2)
                    {
                        tile.ClearTile();
                    }
                }
            }

            //spawn projectile
            //Projectile.NewProjectile(source, position, velocity.RotatedBy(rotation), ModContent.ProjectileType<FullMetalSlugBall>(), damage * 4, knockback * 1.5f, player.whoAmI);

            return base.UseItem(player);
        }

    }
}