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

namespace Bugdom.Accessories.Attractants.AppleCiderVinegar
{
    //The ExampleSquirrelThief is essentially the same as a regular Squirrel, but it steals ExampleItems and keep them until it is killed, being saved with the world if it has enough of them.
    public class FruitFly : ModNPC
    {


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
            //NPC.DeathSound = SoundID.NPCDeath2;
            //NPC.value = 60f;
            //NPC.damage = 1;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.DemonEye; // butterfly AI, important to choose the aiStyle that matches the NPCID that we want to mimic
            //AIType = NPCID.JungleBat;

            //AIType = NPCID.Butterfly; // Use vanilla Squirrel's type when executing AI code. (This also means it will try to despawn during daytime)
            AnimationType = NPCID.LadyBug; // Use vanilla Squirrel's type when executing animation code. Important to also match Main.npcFrameCount[NPC.type] in SetStaticDefaults.
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f; // Spawn with 1/10th the chance of a regular Squirrel.
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