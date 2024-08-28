using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Generation;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria.IO;
using Microsoft.Xna.Framework;

namespace Bugdom.World
{
    public class TileKillPermission : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            // checks related to tiles being directly above the target tile
            if (j > 0)
            {
                ushort id = Main.tile[i, j - 1].TileType;
                if (id == ModContent.TileType<Termites.TermiteMound>())
                {
                    ushort selfid = Main.tile[i, j].TileType;
                    if (selfid != ModContent.TileType<Termites.TermiteMound>())
                    {
                        return false;
                    }
                }
            }

            // base case
            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
    }
}