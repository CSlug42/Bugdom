using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bugdom.Items.Weapons.SlugThrower
{
    // This is an example gun designed to best demonstrate the various tML hooks that can be used for ammo-related specifications.
    public class SlugThrower : ModItem
    {
        public override void SetDefaults()
        {
            // Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

            // Common Properties
            Item.width = 50; // Hitbox width of the item.
            Item.height = 50; // Hitbox height of the item.
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.White; // The color that the item's name will be in-game.

            // Use Properties
            Item.useTime = 40; // The item's use time in ticks (60 ticks == 1 second.)
            Item.useAnimation = 15; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.reuseDelay = 0; // The amount of time the item waits between use animations (60 ticks == 1 second.)
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.autoReuse = true; // Whether or not you can hold click to automatically use it again.
            Item.UseSound = SoundID.Item5;

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
            Item.damage = 3; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            Item.knockBack = 3f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            Item.noMelee = true; // So the item's animation doesn't do damage.
            Item.crit = 7;

            // Gun Properties
            Item.shoot = ProjectileID.PurificationPowder; // For some reason, all the guns in the vanilla source have this.
            Item.shootSpeed = 12f; // The speed of the projectile (measured in pixels per frame.)
            Item.useAmmo = AmmoID.Bullet;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Wood, 10);
            recipe.AddIngredient(ItemID.Cobweb, 10);
            recipe.AddIngredient(ItemID.AntlionMandible, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5f, -2f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //generate new slug?
            //if ((player.IsAmmoFreeThisShot(source.Item, new Item(ItemID.MusketBall, 1, 0), ModContent.ProjectileType<FullMetalSlugBall>())
            //    || Main.rand.Next(100) < 5)
            //    && player.GetModPlayer<Common.Conservationist>().slugCount < 5)
            //{
            //    player.GetModPlayer<Common.Conservationist>().slugCount += 1;
            //    player.AddBuff(ModContent.BuffType<PumpedUpSlug>(), 3600);
            //}

            //shoot slugs if any, then set count to 0
            for (int slugs = player.GetModPlayer<Common.Conservationist>().slugCount; slugs > 0; slugs--) {
                float rotation = MathHelper.ToRadians(-8 + Main.rand.Next(16));
                Projectile.NewProjectile(source, position, velocity.RotatedBy(rotation), ModContent.ProjectileType<FullMetalSlugBall>(), damage * 4, knockback * 1.5f, player.whoAmI);
            }
            player.GetModPlayer<Common.Conservationist>().slugCount = 0;
            player.ClearBuff(ModContent.BuffType<PumpedUpSlug>());

            //normal shotgun operation
            for (int i = 0; i < 5; i++)
            {
                float rotation = MathHelper.ToRadians(-8 + Main.rand.Next(16));
                Projectile.NewProjectile(source, position, velocity.RotatedBy(rotation), type, damage, knockback, player.whoAmI);
            }

            //temporary var checking
            //Main.NewText($"Copper tier ore ID: {Critters.Orollers.OreSaver.copperTier}", (byte)30, (byte)255, (byte)10);
            //Main.NewText($"Iron tier ore ID: {Critters.Orollers.OreSaver.ironTier}", (byte)30, (byte)255, (byte)10);

            return false;
        }
    }
}