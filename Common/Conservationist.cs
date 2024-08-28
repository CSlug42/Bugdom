using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bugdom.Common
{
    /**
     * Contains ModPlayer info pertaining to the ranged class, particularly ammo cconsumption
     * Related buffs are paired with their respective items
     */
    public class Conservationist : ModPlayer
    {
        public int slugCount = 0; // for slugThrower
        public bool free = false; // was the previous shot free?

        public override void PostUpdate()
        {
            // Paired with SlugThrower family of weapons
            if (!Player.HasBuff(ModContent.BuffType<Items.Weapons.SlugThrower.PumpedUpSlug>()))
            {
                slugCount = 0;
            }
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            if (Player.HasBuff(ModContent.BuffType<Accessories.Attractants.BoneInTheChamber.Chambered>()))
            {
                return false;
            }
            return base.CanConsumeAmmo(weapon, ammo);
        }

        // done to check Vanilla effects(only way to hook into code)
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.DamageType != DamageClass.Ranged)
            {
                return base.Shoot(item, source, position, velocity, type, damage, knockback);
            }

            free = Player.IsAmmoFreeThisShot(source.Item, new Item(ItemID.MusketBall, 1, 0), ModContent.ProjectileType<Items.Weapons.SlugThrower.FullMetalSlugBall>());

            if (free && Player.GetModPlayer<Common.Conservationist>().slugCount < 5 && item.type == ModContent.ItemType<Items.Weapons.SlugThrower.SlugThrower>())
            {
                Player.AddBuff(ModContent.BuffType<Items.Weapons.SlugThrower.PumpedUpSlug>(), 3600);
                slugCount++;
            }

            Player.ClearBuff(ModContent.BuffType<Accessories.Attractants.BoneInTheChamber.Chambered>());

            return base.Shoot(item, source, position, velocity, type, 0, knockback);
        }

        //changes properties of the shot projectile
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Bone in the Chamber Effect
            if (Player.HasBuff(ModContent.BuffType<Accessories.Attractants.BoneInTheChamber.Chambered>()))
            {
                Item specialAmmo = ChooseLastAmmoSlot(item);
                if (specialAmmo != null)
                {
                    StatModifier statMod = Player.GetDamage(DamageClass.Ranged);
                    type = specialAmmo.shoot;
                    damage = (int) statMod.ApplyTo((float) (specialAmmo.damage + item.damage));
                }
            }
            base.ModifyShootStats(item, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        private Item ChooseLastAmmoSlot(Item weapon)
        {
            Item ammo = Player.inventory[57];

            if (weapon.useAmmo == ammo.ammo)
            {
                return ammo;
            }

            return null;
        }
    }
}