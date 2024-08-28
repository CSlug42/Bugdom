using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bugdom.Items.Weapons.Slingshot
{
    // This is an example gun designed to best demonstrate the various tML hooks that can be used for ammo-related specifications.
    public class TrustySlingshot : ModItem
    {
        private int mostRecentBaitPower = 0;

        public override void SetDefaults()
        {
            // Modders can use Item.DefaultToRangedWeapon to quickly set many common properties, such as: useTime, useAnimation, useStyle, autoReuse, DamageType, shoot, shootSpeed, useAmmo, and noMelee. These are all shown individually here for teaching purposes.

            // Common Properties
            Item.width = 50; // Hitbox width of the item.
            Item.height = 50; // Hitbox height of the item.
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.White; // The color that the item's name will be in-game.

            // Use Properties
            Item.useTime = 45; // The item's use time in ticks (60 ticks == 1 second.)
            Item.useAnimation = 45; // The length of the item's use animation in ticks (60 ticks == 1 second.)
            Item.reuseDelay = 45; // The amount of time the item waits between use animations (60 ticks == 1 second.)
            Item.useStyle = ItemUseStyleID.Shoot; // How you use the item (swinging, holding out, etc.)
            Item.autoReuse = false; // Whether or not you can hold click to automatically use it again.
            Item.UseSound = SoundID.Item5;

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged; // Sets the damage type to ranged.
            Item.damage = 4; // Sets the item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage added together.
            Item.knockBack = 9f; // Sets the item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback added together.
            Item.noMelee = true; // So the item's animation doesn't do damage.
            Item.crit = 21;

            // Gun Properties
            Item.shoot = ModContent.ProjectileType<SlugBall>(); // For some reason, all the guns in the vanilla source have this.
            Item.shootSpeed = 12f; // The speed of the projectile (measured in pixels per frame.)
            Item.useAmmo = 1; // The "ammo Id" of the ammo item that this weapon uses. Ammo IDs are magic numbers that usually correspond to the item id of one item that most commonly represent the ammo type.
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

        public override bool? CanChooseAmmo(Item ammo, Player player)
        {
            mostRecentBaitPower = ammo.bait;
            return ammo.bait > 0;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage += mostRecentBaitPower;
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
    }
}