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
	public class LeadOroller : CopperOroller
	{	
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GlowingSnail];
        }

		public override void SetDefaults() {
            NPC.catchItem = ModContent.ItemType<LeadOrollerItem>();
            base.SetDefaults();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("Orollers tend to live where their favorite ores are, but rarely you'll find them lost. These have an affinity for lead."),
                new RareSpawnBestiaryInfoElement(5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {

            float spawnModifier = 0.5f;
            if (Critters.Orollers.OreSaver.ironTier == TileID.Iron)
            {
                spawnModifier = 0.1f;
            }
            else if ((Critters.Orollers.OreSaver.ironTier == TileID.Lead))
            {
                spawnModifier = 0.9f;
            }

            return SpawnCondition.Cavern.Chance * 0.15f * spawnModifier + SpawnCondition.Underground.Chance * 0.15f * spawnModifier;
        }

		public override void OnCaughtBy(Player player, Item item, bool failed) {
            if (hasOrb)
            {
                Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.LeadOre, (int)Math.Ceiling((1 + item.pick * 0.01) * new Random().Next(25, 35)), false, 0, false, false);
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

    public class LeadOrollerItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 30000;
            Item.rare = 1;
            Item.bait = 22;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.makeNPC = ModContent.NPCType<LeadOroller>();
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.SetNameOverride("Lead Oroller");
            Item.rare = ItemRarityID.White;
        }
    }

    public class LeadOrexcavator : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 8;
            Item.DamageType = DamageClass.Melee;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.value = 60000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.pick = 43; // pick power will determine extra ore from orollers
            ItemID.Sets.CatchingTool[Item.type] = true; // important!
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LeadBar, 7);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient(ModContent.ItemType<LeadOrollerItem>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}