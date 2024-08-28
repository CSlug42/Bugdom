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

namespace Bugdom.World.Termites
{
    public class TermiteQueen : ModNPC
    {
        public bool hasOrb = true;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Worm];
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 10;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath16;
            // These three are typical critter values
            NPCID.Sets.CountsAsCritter[Type] = true;

            NPC.knockBackResist = 0.5f;

            NPC.catchItem = ModContent.ItemType<TermiteQueenItem>();

            // function like snail
            AnimationType = NPCID.Worm;
            NPC.aiStyle = NPCAIStyleID.CritterWorm;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement("Termite queens lie at the center of their industrious spires."),
                new RareSpawnBestiaryInfoElement(3));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                // Retrieve the gore types
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("TermiteQueen_Gore_1").Type, NPC.scale);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("TermiteQueen_Gore_2").Type, NPC.scale);
            }
        }

        public override void PostAI()
        {
            if (NPC.velocity.X > 0)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }
        }
    }

    public class TermiteQueenItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 16000;
            Item.rare = 1;
            Item.bait = 40;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.makeNPC = ModContent.NPCType<TermiteQueen>();
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.SetNameOverride("Termite Queen");
            Item.rare = ItemRarityID.White;
        }
    }
}