using System;
using System.Security.AccessControl;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bugdom.Accessories.Attractants.BoneInTheChamber
{
    public class Chambered : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            Main.player[Main.myPlayer].buffTime[buffIndex] = 600;

            return true;
        }

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            base.ModifyBuffText(ref buffName, ref tip, ref rare);
            tip = $"Bonebug chambered and ready to fire!";
        }
    }
}