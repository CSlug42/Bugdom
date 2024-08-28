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
using Terraria.Audio;

namespace Bugdom.Accessories.Attractants.BugZapper
{
	//The ExampleSquirrelThief is essentially the same as a regular Squirrel, but it steals ExampleItems and keep them until it is killed, being saved with the world if it has enough of them.
	public class StaticCloudHopper : ModNPC
	{
		int owner;
        float speed = 12f;
        float upSpeed = 18f;
        float inertia = 60f;
        float upInertia = 30f;
        float slowUpInertia = 80f;

        public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.LadyBug];
		}

		public override void SetDefaults() {
			NPC.width = 24;
			NPC.height = 10;
			NPC.defense = 6;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
            NPC.noGravity = true;
            //NPC.value = 60f;
            //NPC.damage = 1;
            NPC.catchItem = ModContent.ItemType<BugFragment>();
            NPC.knockBackResist = 0.5f;
			//NPC.aiStyle = NPCAIStyleID.AngryNimbus; // butterfly AI, important to choose the aiStyle that matches the NPCID that we want to mimic

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            //AIType = NPCID.Butterfly; // Use vanilla Squirrel's type when executing AI code. (This also means it will try to despawn during daytime)
            AnimationType = NPCID.LadyBug; // Use vanilla Squirrel's type when executing animation code. Important to a	lso match Main.npcFrameCount[NPC.type] in SetStaticDefaults.
		}

		public override void OnCaughtBy(Player player, Item item, bool failed) {
			if (!failed) {
				Projectile a = Projectile.NewProjectileDirect(null, this.Entity.position, new Vector2(0, 0), ProjectileID.MagnetSphereBolt, 50, 9f, owner);
                int target = a.FindTargetWithLineOfSight(800);
				if (target < 0) 
				{
					a.Kill();
				}
				else
				{
					a.velocity = Entity.DirectionTo(Main.npc[target].Center) * 15;
                    SoundEngine.PlaySound(SoundID.Thunder, this.Entity.position);
                }
            }
		}

        public override void AI()
        {
            if (owner == null)
            {
                owner = this.NPC.FindClosestPlayer();
            }
            Vector2 idlePosition = Main.player[owner].Center;
            idlePosition.Y -= 120f; 
            
            Vector2 vectorToIdlePosition = idlePosition - Entity.Center;
            //distanceToIdlePosition = vectorToIdlePosition.Length();

            // set facing correct
            int dir = 0;
            if (this.Entity.velocity.X > 0) { dir = 1; }
            NPC.direction = dir;
            NPC.spriteDirection = dir;


            vectorToIdlePosition = idlePosition - this.Entity.Center;

            vectorToIdlePosition.Normalize();
            //above idleposition
            if (this.Entity.Center.Y < idlePosition.Y)
            {
                vectorToIdlePosition *= speed;
                Entity.velocity = (Entity.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            }
            //below idle position
            else
            {
                vectorToIdlePosition *= upSpeed;
                //moving upwards
                if (this.Entity.velocity.Y < 0)
                {
                    Entity.velocity = (Entity.velocity * (slowUpInertia - 1) + vectorToIdlePosition) / slowUpInertia;
                }
                //not moving upwards
                else
                {
                    Entity.velocity = (Entity.velocity * (upInertia - 1) + vectorToIdlePosition) / upInertia;
                }
                
            }
            
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

    }
}