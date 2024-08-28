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
    public class SickleMembracid : TreeHopper
    {

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 24;
            NPC.defense = 0;
            NPC.lifeMax = 5;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath16;
            // These three are typical critter values
            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = false;
            NPCID.Sets.TownCritter[Type] = true;

            NPC.catchItem = ModContent.ItemType<SickleMembracidItem>();
            NPC.knockBackResist = 0.5f;

            NPC.aiStyle = NPCAIStyleID.Slime;
            AnimationType = NPCID.Stinkbug;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneGraveyard)
            {
                return 0.2f;
            }
            else 
            { 
                return 0f;  
            }
        }
    }
}
