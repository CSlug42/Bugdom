using System;
using System.Security.AccessControl;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bugdom.Items.Weapons.SlugThrower
{
    public class PumpedUpSlug : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            Main.player[Main.myPlayer].buffTime[buffIndex] = 3600;
            //Main.player[Main.myPlayer].GetModPlayer<Common.Conservationist>().slugCount += 1;

            return true;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            int count = Main.player[Main.myPlayer].GetModPlayer<Common.Conservationist>().slugCount;
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = $"You have {count} slug{(count != 1 ? "s" : "")} pumped!";
        }

        //public override void Update(Player player, ref int buffIndex)
        //{
        //    // If the minions exist reset the buff time, otherwise remove the buff from the player
        //    if (player.ownedProjectileCounts[ModContent.ProjectileType<CapturedSummons.PheremoneFlyer>()] > 0)
        //    {
        //        player.buffTime[buffIndex] = 18000;
        //    }
        //    else
        //    {
        //        player.DelBuff(buffIndex);
        //        buffIndex--;
        //    }
        //}
    }
}
