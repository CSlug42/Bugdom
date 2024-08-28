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

namespace Bugdom.Items.Weapons.SlugThrower
{
    //The ExampleSquirrelThief is essentially the same as a regular Squirrel, but it steals ExampleItems and keep them until it is killed, being saved with the world if it has enough of them.
    public class FullMetalSlug : ModNPC
    {


        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Sluggy];
        }

        public override void SetDefaults()
        {
            NPC.width = 10;
            NPC.height = 10;
            NPC.defense = 0;
            NPC.lifeMax = 1;
            NPC.HitSound = SoundID.NPCHit1;
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPC.catchItem = ModContent.ItemType<Accessories.Attractants.BugFragment>();
            NPC.aiStyle = NPCAIStyleID.CritterWorm;
            AnimationType = NPCID.Sluggy;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0f; // Spawn with 1/10th the chance of a regular Squirrel.
        }

        public override void OnCaughtBy(Player player, Item item, bool failed)
        {
            if (player.GetModPlayer<Common.Conservationist>().slugCount < 5)
            {
                player.GetModPlayer<Common.Conservationist>().slugCount += 1;
            }
            player.AddBuff(ModContent.BuffType<PumpedUpSlug>(), 3600);
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