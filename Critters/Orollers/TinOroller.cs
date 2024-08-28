using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace Bugdom.Critters.Orollers
{
	public class TinOroller : CopperOroller
	{	
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GlowingSnail];
        }

		public override void SetDefaults() {
            base.SetDefaults();
            NPC.catchItem = ModContent.ItemType<TinOrollerItem>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("These beetles roll balls of ore, and can push up to 70 times their own body weight. These ores are delicately taken care of by the orollers, so be gentle! These have an affinity for tin."),
                new RareSpawnBestiaryInfoElement(5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {

            float spawnModifier = 0.5f;
            if (Critters.Orollers.OreSaver.copperTier == TileID.Copper)
            {
                spawnModifier = 0.1f;
            }
            else if ((Critters.Orollers.OreSaver.copperTier == TileID.Tin))
            {
                spawnModifier = 0.9f;
            }

            return SpawnCondition.Cavern.Chance * 0.2f * spawnModifier + SpawnCondition.Underground.Chance * 0.2f * spawnModifier;
        }

		public override void OnCaughtBy(Player player, Item item, bool failed) {
            if (hasOrb)
            {
                Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.TinOre, (int)Math.Ceiling((1 + item.pick * 0.01) * new Random().Next(20, 32)), false, 0, false, false);
            }
        }

        public override void DrawEffects(ref Color drawColor)
		{
            var rand = new Random();   
			if (rand.Next(100) < 1) {
                //NewDustDirect (Vector2 Position, int Width, int Height, int Type, float SpeedX=0f, float SpeedY=0f, int Alpha=0, Color newColor=default(Color), float Scale=1f)
                Dust dust = Dust.NewDustDirect(Entity.position + (Entity.velocity * 60), 1, 1, 43, 0f, 0f, 0, drawColor, 0.1f);
            }
        }
    }

    public class TinOrollerItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 25000;
            Item.rare = 1;
            Item.bait = 20;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.makeNPC = ModContent.NPCType<TinOroller>();
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.SetNameOverride("Tin Oroller");
            Item.rare = ItemRarityID.White;
        }
    }

    public class TinOrexcavator : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 6;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 15;
            Item.useAnimation = 18;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 50000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.pick = 35; // pick power will determine extra ore from orollers
            ItemID.Sets.CatchingTool[Item.type] = true; // important!
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TinBar, 6);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddIngredient(ModContent.ItemType<TinOrollerItem>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}