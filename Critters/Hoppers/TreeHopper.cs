using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace Bugdom.Critters.Hoppers
{
	public class TreeHopper : ModNPC
	{

        private float AI_State = 0f;
        private float AI_Timer = 0f;

        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 8;
        }

		public override void SetDefaults() {
			NPC.width = 24;
			NPC.height = 24;
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

            NPC.aiStyle = NPCAIStyleID.Slime;
            AnimationType = NPCID.Stinkbug;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                new FlavorTextBestiaryInfoElement("These weevils use their long mouths to drill into heart crystals and extract the vital insides. There are even crystals growing in their body."),
                new RareSpawnBestiaryInfoElement(2));

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return SpawnCondition.OverworldDayGrassCritter.Chance * 0.2f;
        }

        //
        private enum ActionState
        {
            Walk,
            Launch,
            Fall,
            Glide
        }

        // called before hopperAI to see if it can just behave like a normal slime or not
        public override bool PreAI()
        {
            
            float dist;
            this.NPC.FindClosestPlayer(out dist);
            if (dist < 200 && AI_State == (float)ActionState.Walk && NPC.velocity.Y == 0)
            {
                AI_State = (float)ActionState.Launch;
            }

            if (AI_State == (float)ActionState.Walk)
            {
                return true;
            }
            else
            {
                hopperAI();
                PostAI();
                return false;
            }
            
        }

        /**
         *  
         */
        public void hopperAI()
        {
            switch (AI_State)
            {
                case (float)ActionState.Launch:

                    // apply an initial impulse and go to next action state
                    var rand = new Random();
                    NPC.velocity.X = (rand.Next(200, 600) / 100) * FindDirectionAway();
                    NPC.velocity.Y = -20;
                    AI_State = (float)ActionState.Fall;
                    break;
                case (float)ActionState.Fall:

                    // increase timer and spin a little while falling
                    AI_Timer += 1;
                    NPC.rotation += 0.25f;

                    // move on to gliding if timed out or stuck
                    if (AI_Timer > 130 || (NPC.velocity.X == 0 && NPC.velocity.Y == 0))
                    {
                        AI_Timer = 0;
                        NPC.velocity.Y = 2;
                        NPC.noGravity = true;
                        //NPC.frame = 4;      
                        AI_State = (float)ActionState.Glide;
                    }
                    break;
                case (float)ActionState.Glide:
                    
                    // extra calculations for inertia = 10
                    int dir = FindDirectionAway();
                    NPC.velocity.X = (Entity.velocity.X * (5) + (6 * dir)) / 6;

                    NPC.rotation = 0f;
                    NPC.direction = dir;
                    NPC.spriteDirection = dir;

                    if (NPC.velocity.Y == 0)
                    {
                        NPC.noGravity = false;
                        NPC.velocity.X = 0;
                        AI_State = (float)ActionState.Walk;
                    }
                    break;
            }
        }

        public override void FindFrame(int currentframe)
        {
            if (AI_State == (float)ActionState.Fall)
            {
                NPC.frame.Y = 0;
            }
            else
            {
                base.FindFrame(currentframe);
            }
            
        }

        // returns -1 if the closest player is to the right of this NPC, otherwise returns 1
        // these ints as X vector values lead the NPC away from the closest player
        public int FindDirectionAway()
        {
            Vector2 player = Main.player[this.NPC.FindClosestPlayer()].Center;
            if (this.Entity.DirectionFrom(player).X > 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
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
    }
}