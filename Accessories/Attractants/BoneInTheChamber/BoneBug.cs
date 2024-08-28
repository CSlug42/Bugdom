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

namespace Bugdom.Accessories.Attractants.BoneInTheChamber
{
    //The ExampleSquirrelThief is essentially the same as a regular Squirrel, but it steals ExampleItems and keep them until it is killed, being saved with the world if it has enough of them.
    public class BoneBug : ModNPC
    {
        private int timer = 1000;
        private float AI_State = 0f;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.LadyBug];
        }

        public override void SetDefaults()
        {
            NPC.width = 10;
            NPC.height = 10;
            NPC.defense = 0;
            NPC.lifeMax = 1;
            NPC.HitSound = SoundID.NPCHit1;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;
            NPC.catchItem = ModContent.ItemType<Attractants.BugFragment>();

            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.CritterWorm;

            AnimationType = NPCID.LadyBug;
        }

        private enum ActionState
        {
            Walk,
            Fall
        }

        // called before AI to see if it can just behave like a normal slug or not
        public override bool PreAI()
        {

            if (timer == 1000) 
            {
                var rand = new Random();
                NPC.velocity.X = 0;
                NPC.velocity.Y = -20;
                AI_State = (float)ActionState.Fall;
            }

            if (timer < 900 && AI_State == (float)ActionState.Fall && NPC.velocity.Y == 0)
            {
                AI_State = (float)ActionState.Walk;
            }

            timer--;

            if (AI_State == (float)ActionState.Walk)
            {
                return true;
            }
            else
            {
                //hopperAI();
                PostAI();
                return false;
            }

        }


        public override void OnCaughtBy(Player player, Item item, bool failed)
        {
            player.AddBuff(ModContent.BuffType<Chambered>(), 600);
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