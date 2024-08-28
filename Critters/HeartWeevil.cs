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

namespace Bugdom.Critters
{
	public class HeartWeevil : ModNPC
	{

        public static Vector3 LightColor = new Vector3(0.3f, 0.075f, 0.15f);

        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Bird];
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
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            NPC.catchItem = ModContent.ItemType<CritterItems.HeartWeevilItem>();
            NPC.knockBackResist = 0.5f;
			NPC.aiStyle = NPCAIStyleID.Bird;

			//AIType = NPCID.Butterfly; // Use vanilla Squirrel's type when executing AI code. (This also means it will try to despawn during daytime)
			AnimationType = NPCID.Bird; // Use vanilla Squirrel's type when executing animation code. Important to also match Main.npcFrameCount[NPC.type] in SetStaticDefaults.
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("These weevils use their long mouths to drill into heart crystals and extract the vital insides. There are even crystals growing in their body."),
                new RareSpawnBestiaryInfoElement(4));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            //return SpawnCondition.Underground.Chance * 0.05f;
            for (int i = 0; i < 12; i += 2)
            {
                for (int j = 0;j < 12; j += 2)
                {
                    if (Main.tile[spawnInfo.SpawnTileX - 6 + j, spawnInfo.SpawnTileY - 3 + j].TileType == TileID.Heart)
                    {
                        return 100f;
                    }
                }
            }

            return SpawnCondition.Cavern.Chance * 0.03f;
        }

		public override void OnCaughtBy(Player player, Item item, bool failed) {
            Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.LifeCrystal, 1, false, 0, false, false);
		}

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                // Retrieve the gore types
                //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Body").Type, NPC.scale);
                //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Orb").Type, NPC.scale);
                //Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>($"{Name}_Gore_Head").Type, NPC.scale);
            }
        }

        public override void DrawEffects(ref Color drawColor)
		{
            var rand = new Random();
            if (rand.Next(60) < 1)
            {
                Dust.NewDustDirect((Entity.position), 1, 1, DustID.LifeCrystal, 0f, 0f, 0, drawColor, 1f);
            }

            Lighting.AddLight(Entity.position, LightColor);
        }
    }
}