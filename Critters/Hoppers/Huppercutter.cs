using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bugdom;
using Bugdom.Critters;
using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace Bugdom.Critters.Hoppers
{
    public class Huppercutter : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.damage = 5;
            Item.width = 18;
            Item.height = 18;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.autoReuse = false; // Whether or not you can hold click to automatically use it again.
            Item.UseSound = SoundID.Item43;
            Item.knockBack = 14;
            Item.value = 10000;
            Item.rare = 2;
            Item.useTurn = true;
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;

            Item.crit = 0;
            Item.mana = 55;
            Item.shootSpeed = 15f; // The speed of the projectile (measured in pixels per frame.)

            Item.shoot = ModContent.ProjectileType<HopperAura>();
            Item.shootsEveryUse = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Sickle, 5);
            recipe.AddIngredient(ItemID.DeathweedSeeds, 3);
            recipe.AddIngredient(ModContent.ItemType<SickleMembracidItem>(), 1);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (!player.HasBuff(BuffID.ManaSickness))
            {
                player.velocity.X = player.velocity.X / 4 + velocity.X * 0.8f;
                player.velocity.Y = player.velocity.Y / 4 + velocity.Y * 0.9f;
            }
            return true;
        }

    }
}