using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

// This minion shows a few mandatory things that make it behave properly.
// Its attack pattern is simple: If an enemy is in range of 43 tiles, it will fly to it and deal contact damage
// If the player targets a certain NPC with right-click, it will fly through tiles to it
// If it isn't attacking, it will float near the player with minimal movement
namespace Bugdom.Items.Weapons.WormBatonSummon
{
    public class WormInfestedMinion : ModProjectile
    {
        private bool isReturning = false;
        private bool isInAir = false;

        public override void SetStaticDefaults()
        {
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 8;
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 48;
            Projectile.tileCollide = true; // Makes the minion go through tiles freely

            // These below are needed for a minion weapon
            Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.minion = true; // Declares this as a minion (has many effects)
            Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
            Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
        }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return true;
        }

        // The AI of this minion is split into multiple methods to avoid bloat. This method just passes values between calls actual parts of the AI.
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!CheckActive(owner))
            {
                return;
            }

            GeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            if (isReturning) { foundTarget = false; }
            Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
            if (!isReturning)
            {
                Projectile.velocity.Y += 0.4f;
            }
            Visuals();
        }

        // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<WormInfestedMinionBuff>());

                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<WormInfestedMinionBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 0f; // Go up 48 coordinates (three tiles from the center of the player)

            // If your minion doesn't aimlessly move around when it's idle, you need to "put" it into the line of other summoned minions
            // The index is projectile.minionPos
            float minionPositionOffsetX = (50 + Projectile.minionPos * 50) * -owner.direction;
            idlePosition.X += minionPositionOffsetX; // Go behind the player

            // All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

            // Teleport to player if distance is too big
            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2500f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // If your minion is flying, you want to do this independently of any conditions
            float overlapVelocity = 0.02f;

            // Fix overlap with other minions
            foreach (var other in Main.ActiveProjectiles)
            {
                if (other.whoAmI != Projectile.whoAmI && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    if (Projectile.position.X < other.position.X)
                    {
                        Projectile.velocity.X -= overlapVelocity;
                    }
                    else
                    {
                        Projectile.velocity.X += overlapVelocity;
                    }
                }
            }
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            // Starting search distance
            distanceFromTarget = 500f;
            targetCenter = Projectile.position;
            foundTarget = false;

            // This code is required if your minion weapon has the targeting feature
            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 1200f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            // friendly needs to be set to true so the minion can deal contact damage
            // friendly needs to be set to false so it doesn't damage things like target dummies while idling
            // Both things depend on if it has a target or not, so it's just one assignment here
            // You don't need this assignment if your minion is shooting things instead of dealing contact damage
            Projectile.friendly = foundTarget;
        }

        private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            // Default movement parameters (here for attacking)
            float speed = 4.2f;
            float inertia = 40f;

            // trying to handle slopes and such
            if (Projectile.velocity.Y == 0)
            {
                Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
            }

            if (foundTarget)
            {
                // Minion has a target: attack (here, fly towards the enemy)
                if (distanceFromTarget > 80f)
                {
                    // The immediate range around the target (so it doesn't latch onto it when close)
                    Vector2 direction = targetCenter - Projectile.Center;

                    if (Projectile.velocity.Y == Projectile.oldVelocity.Y && Projectile.position.Y == Projectile.oldPosition.Y && direction.Y < 2) // stuck on ground, so jump
                    {
                        Projectile.velocity.Y -= Math.Min(3f - 0.01f * direction.Y + 0.0001f * direction.Y * direction.Y, 13f);
                        isInAir = true;
                    }

                    direction.Normalize();
                    direction *= speed;

                    Projectile.velocity.X = (Projectile.velocity.X * (inertia - 1) + direction.X) / inertia;
                }
            }
            else
            {   
                if (distanceToIdlePosition > 800f || isReturning)
                {
                    // Speed up the minion if it's away from the player
                    speed = 18f;
                    inertia = 45f;
                    isReturning = true;
                    isInAir = true;
                    Projectile.tileCollide = false;
                }
                // Minion doesn't have a target: return to player and idle
                else if (distanceToIdlePosition > 200f)
                {
                    // Speed up the minion if it's away from the player
                    speed = 6f;
                    inertia = 30f;
                    // Main.NewText($"Deciding to jump because stuck and xVel: {Projectile.velocity.X}, yVel: {Projectile.velocity.Y}", (byte)255, (byte)255, (byte)255);
                    if ((Projectile.velocity.X >= -0.1 && Projectile.velocity.X <= 0.1) && Projectile.velocity.Y == 0) // stuck on ground, so jump
                    {
                        Projectile.velocity.Y -= 8f;
                        vectorToIdlePosition.X *= -1;
                        isInAir = true;
                    }
                }
                else
                {
                    // Slow down the minion if closer to the player
                    speed = 3f;
                    inertia = 40f;
                }

                if (distanceToIdlePosition > 5f)
                {
                    // This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    Projectile.velocity.X = (Projectile.velocity.X * (inertia - 1) + vectorToIdlePosition.X) / inertia;
                    if (isReturning)
                    {
                        Projectile.velocity.Y = (Projectile.velocity.Y * (inertia - 1) + vectorToIdlePosition.Y) / inertia;
                    }
                }

                if (distanceToIdlePosition < 96f && Collision.EmptyTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, false))
                {
                    isReturning = false;
                    Projectile.tileCollide = true;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // If the projectile hits the top or bottom side of the tile, make walking pose
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon && !isReturning)
            {
                isInAir = false;
            }

            return false;
        }

        private void Visuals()
        {
            int frameSpeed = 6;

            Projectile.frameCounter++;

            if (!isInAir)
            {
                if (Projectile.frameCounter >= frameSpeed)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;

                    if (Projectile.frame >= 4)
                    {
                        Projectile.frame = 0;
                        Projectile.rotation = 0;
                    }
                }
            }
            else
            {
                if (Projectile.frame < 4)
                {
                    Projectile.frame = 4;
                }
                if (Projectile.frameCounter >= frameSpeed)
                {
                    Projectile.frameCounter = 0;
                    Projectile.frame++;

                    if (Projectile.velocity.X > 0)
                    {
                        Projectile.rotation += 0.5f;
                    }
                    else
                    {
                        Projectile.rotation -= 0.5f;
                    }


                    if (Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        Projectile.frame = 4;
                    }
                }
            }

            if (Projectile.velocity.X > 0)
            {
                Projectile.spriteDirection = 1;
            }
            else
            {
                Projectile.spriteDirection = -1;
            }
        }
    }
}