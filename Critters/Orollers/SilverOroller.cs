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
	public class SilverOroller : CopperOroller
	{	
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GlowingSnail];
        }

		public override void SetDefaults() {
            base.SetDefaults();
            NPC.catchItem = ModContent.ItemType<SilverOrollerItem>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Miners consider these critters good luck, and some used to keep them as pets to not even waste the dust of harvested ores. These have an affinity for silver."),
                new RareSpawnBestiaryInfoElement(5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {

            float spawnModifier = 0.5f;
            if (Critters.Orollers.OreSaver.silverTier == TileID.Tungsten)
            {
                spawnModifier = 0.1f;
            }
            else if ((Critters.Orollers.OreSaver.silverTier == TileID.Silver))
            {
                spawnModifier = 0.9f;
            }

            return SpawnCondition.Cavern.Chance * 0.1f * spawnModifier;
        }

		public override void OnCaughtBy(Player player, Item item, bool failed) {
            if (hasOrb)
            {
                Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.SilverOre, (int)Math.Ceiling((1 + item.pick * 0.01) * new Random().Next(20, 32)), false, 0, false, false);
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

    public class SilverOrollerItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 40000;
            Item.rare = 1;
            Item.bait = 35;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.makeNPC = ModContent.NPCType<SilverOroller>();
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.SetNameOverride("Silver Oroller");
            Item.rare = ItemRarityID.White;
        }
    }

    public class SilverOrexcavator : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 15;
            Item.useAnimation = 18;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 80000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.pick = 45; // pick power will determine extra ore from orollers
            ItemID.Sets.CatchingTool[Item.type] = true; // important!
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SilverBar, 8);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddIngredient(ModContent.ItemType<SilverOrollerItem>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}