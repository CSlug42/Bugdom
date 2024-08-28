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
using Terraria.DataStructures;

namespace Bugdom.Critters.Orollers
{
	public class CopperOroller : ModNPC
	{
        public bool hasOrb = true;

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GlowingSnail];
        }

		public override void SetDefaults() {
			NPC.width = 24;
			NPC.height = 10;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath16;
            // These three are typical critter values
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            
            NPC.knockBackResist = 0.5f;
			

            // Oroller specific traits
            

            NPC.catchItem = ModContent.ItemType<CopperOrollerItem>();

            // function like snail
            AnimationType = NPCID.GlowingSnail;
            NPC.aiStyle = NPCAIStyleID.Snail;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("These beetles roll balls of ore, and can push up to 70 times their own body weight. These ores are delicately taken care of by the orollers, so be gentle! These have an affinity for copper."),
                new RareSpawnBestiaryInfoElement(3));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {

            float spawnModifier = 0.5f;
            if (Critters.Orollers.OreSaver.copperTier == TileID.Copper)
            {
                spawnModifier = 0.9f;
            }
            else if ((Critters.Orollers.OreSaver.copperTier == TileID.Tin))
            {
                spawnModifier = 0.1f;
            }

            return SpawnCondition.Cavern.Chance * 0.2f * spawnModifier + SpawnCondition.Underground.Chance * 0.2f * spawnModifier;
        }

        public override void OnSpawn(IEntitySource source)
        {
            hasOrb = !(source is EntitySource_Parent);
        }

		public override void OnCaughtBy(Player player, Item item, bool failed) {
            if (hasOrb)
            {
                Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.CopperOre, (int)Math.Ceiling((1 + item.pick * 0.01) * new Random().Next(20, 32)), false, 0, false, false);
            }
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                // Retrieve the gore types
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Body").Type, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Orb").Type, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Head").Type, NPC.scale);
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

    public class CopperOrollerItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 25000;
            Item.rare = 1;
            Item.bait = 20;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.makeNPC = ModContent.NPCType<CopperOroller>();
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.SetNameOverride("Copper Oroller");
            Item.rare = ItemRarityID.White;
        }
    }

    public class CopperOrexcavator : ModItem
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
            recipe.AddIngredient(ItemID.CopperBar, 6);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddIngredient(ModContent.ItemType<CopperOrollerItem>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}