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

namespace Bugdom.Critters.Ringworm
{
	public class CosmicRingworm : ModNPC
	{
        public static Vector3 LightColor = new Vector3(0.7f, 0.2f, 0.8f);
        private int ringCount = 3;
        private int frameCounter = 0;
        private int bobCounter = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 12;
        }

        public override void SetDefaults() {
			NPC.width = 24;
			NPC.height = 24;
			NPC.defense = 0;
			NPC.lifeMax = 1;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath16;

            // These three are typical critter values
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            NPC.catchItem = ModContent.ItemType<CritterItems.HeartWeevilItem>();
            NPC.knockBackResist = 50f;		

            NPC.noGravity = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return 0.025f;
        }

        public override void AI()
        {
            UpdateFrames();

            BobUpAndDown();
        }

        private void BobUpAndDown()
        {
            bobCounter += 1;
            if (bobCounter == 360)
            {
                bobCounter = 0;
            }

            Entity.velocity.Y = 0.6f * (float)Math.Cos(MathHelper.ToRadians(bobCounter));
        }

        //handles all frame logic
        private void UpdateFrames()
        {
            int frameMod = 0;
            if (frameCounter < 25)
            {
                frameMod = 1;
            }
            else if (frameCounter < 50)
            {
                frameMod = 0;
            }
            else if (frameCounter < 75)
            {

                frameMod = 2;
            }

            NPC.frame.Y = 54 * (9 - (3 * ringCount) + frameMod);

            frameCounter++;
            if (frameCounter > 100)
            {
                frameCounter = 0;
            }
        }

		//handles behavior related to ringworm teleportation and ring removal
		public override bool? CanBeCaughtBy(Item item, Player player)
		{
            //if all rings already removed, catch normally
            if (ringCount == 0)
            {
                return base.CanBeCaughtBy(item, player);
            }

			//teleport player and reward them with a ring
			player.TeleportationPotion();
			

			//teleport ringworm on edges of players screen
			Vector2 teleCoord = player.position; // starting point
			Double angle = MathHelper.ToRadians(Main.rand.Next(360));
			float xstart = 750f;
			float ystart = 500f;
			bool teleCheck = false;
            for (int i = 0; i < 50 && !teleCheck; i++)
            {
                teleCoord = player.position + new Vector2(xstart * (float)Math.Cos(angle), ystart * (float)Math.Sin(angle));
                teleCoord.X = (int)Math.Round(teleCoord.X / 16, 0); // converting from pixel position to tilemap
                teleCoord.Y = (int)Math.Round(teleCoord.Y / 16, 0);
                teleCheck = !Main.tile[(int)teleCoord.X, (int)teleCoord.Y].HasTile; // checks if there is no foreground tile there, then for lava
                if (Main.tile[(int)teleCoord.X, (int)teleCoord.Y].LiquidAmount > 0)
                {
                    teleCheck = teleCheck && Main.tile[(int)teleCoord.X, (int)teleCoord.Y].LiquidType != LiquidID.Lava;
                }
                
                //spiral in towards player along an oblong path
                xstart -= 15;
                ystart -= 10;
                angle -= 0.25;
            }

            NPC.Teleport(teleCoord*16, TeleportationStyleID.TeleportationPotion, 0);

            //reward player and remove ring
            //give item
            ringCount--;

            return false;
		}

        public override void DrawEffects(ref Color drawColor)
        {
            Lighting.AddLight(Entity.position, LightColor);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,
                new FlavorTextBestiaryInfoElement("Unlike their terrestrial relatives, these ringworms are probably not parasites - their diet is unknown. Trying to catch one will make it slip away through a wormhole."),
                new RareSpawnBestiaryInfoElement(3));
        }
    }
}