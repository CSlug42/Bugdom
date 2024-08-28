using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bugdom;
using Bugdom.Critters;

namespace Bugdom.Items.Weapons.WormBatonSummon
{
    public class WormBaton : ModItem
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
        }

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.noMelee = true; // this item doesn't do any melee damage
            Item.DamageType = DamageClass.Summon;
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

        /// <summary>
        /// This method takes care of the summoning function of the weapon - something usually seen in shoot
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        /// <param name="failed"></param>
        public override void OnCatchNPC(NPC npc, Player player, bool failed)
        {
            if (!failed)
            {
                // defaults to a low power fruitfly
                int buffType = 0;
                int projType = 0;

                // go through each catchable ID to use the right sprite
                switch(npc.type)
                {
                    //case NPCID.Butterfly || NPCID.GoldButterfly || NPCID.HellButterflyy || NPCID.EmpressButterfly:
                    case NPCID.Worm:
                        buffType = ModContent.BuffType<WormMinionBuff>();
                        projType = ModContent.ProjectileType<WormMinion>();
                        break;
                    //case NPCID.RedDragonfly || NPCID.BlueDragonfly || NPCID.YellowDragonfly || NPCID.OrangeDragonfly || NPCID.GreenDragonfly || NPCID.BlackDragonfly || NPCID.GoldDragonfly:
                    case NPCID.EnchantedNightcrawler:
                        buffType = ModContent.BuffType<NightcrawlerMinionBuff>();
                        projType = ModContent.ProjectileType<NightcrawlerMinion>();
                        break;
                    default:
                        buffType = ModContent.BuffType<WormInfestedMinionBuff>();
                        projType = ModContent.ProjectileType<WormInfestedMinion>();
                        break;
                }
                // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
                player.AddBuff(buffType, 2);

                // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
                var projectile = Projectile.NewProjectileDirect(null, player.position, new Microsoft.Xna.Framework.Vector2(0, 0), projType, Item.damage, Item.knockBack, Main.myPlayer);
                projectile.originalDamage = Item.damage;
            }
        }

    }
}