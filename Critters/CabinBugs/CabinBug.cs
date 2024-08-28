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

namespace Bugdom.Critters.CabinBugs
{
    public class CabinBug : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GlowingSnail];
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 36;
            NPC.defense = 0;
            NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath16;
            // These three are typical critter values
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            NPC.scale = 2f;


            NPC.knockBackResist = 0.5f;


            // Oroller specific traits


            NPC.catchItem = ModContent.ItemType<Orollers.CopperOrollerItem>();

            // function like snail
            AnimationType = NPCID.GlowingSnail;
            NPC.aiStyle = NPCAIStyleID.Snail;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {

            return 0.05f;
        }

        public override void OnCaughtBy(Player player, Item item, bool failed)
        {
            Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.CopperOre, (int)Math.Ceiling((1 + item.pick * 0.01) * new Random().Next(20, 32)), false, 0, false, false);
        }
    }
}
