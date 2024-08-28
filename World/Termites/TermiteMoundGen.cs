using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Generation;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria.IO;
using Microsoft.Xna.Framework;

namespace Bugdom.World.Termites
{
    public class TermiteMoundGen : ModSystem
    {
        // 4. We use the ModifyWorldGenTasks method to tell the game the order that our world generation code should run
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            // 5. We use FindIndex to locate the index of the vanilla world generation task called "Shinies". This ensures our code runs at the correct step.
            int ChestIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Surface Chests"));
            if (ChestIndex != -1)
            {
                // 6. We register our world generation pass by passing in an instance of our custom GenPass class below. The GenPass class will execute our world generation code.
                tasks.Insert(ChestIndex + 1, new TermiteMoundGenPass("Termite Mound Gen", 100f));
            }
        }
    }

    public class TermiteMoundGenPass : GenPass
    {
        public TermiteMoundGenPass(string name, float loadWeight) : base(name, loadWeight)
        {
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {  
            progress.Message = "Mounding Termites";

            for (int i = 0; i < 2; i++) // loop size 2 to place 2 in the world
            {
                // Want to place above underground desert - so look for sandstone
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);
                while (Main.tile[x, y].TileType != TileID.Sandstone)
                {
                    x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);
                }

                // From the correct x-position, find where the surface is
                bool foundSurface = false;
                y = 1;
                while (y < Main.worldSurface)
                {
                    if (WorldGen.SolidTile(x, y))
                    {
                        foundSurface = true;
                        break;
                    }
                    y++;
                }

                //make it sandy in case it wasn't already
                WorldUtils.Gen(new Point(x, y+2), new Shapes.Circle(4, 2), new Actions.SetTile(TileID.Sand));

                // clear out desired space
                for (int j = x-2; j < x+3; j++)
                {
                    for (int k = y-8; k < y; k++)
                    {
                        Main.tile[j, k].ClearEverything();
                    }
                }

                // Now make a place where the mound can go
                for (int j = x-2; j < x+3; j++)
                {
                    Tile tile = Main.tile[j, y];
                    tile.HasTile = true;
                    tile.BlockType = BlockType.Solid;
                    Main.tile[j, y].TileType = TileID.HardenedSand;
                }
                for (int j = x - 1; j < x + 2; j++)
                {
                    Tile tile = Main.tile[j, y+1];
                    tile.HasTile = true;
                    tile.BlockType = BlockType.Solid;
                    Main.tile[j, y+1].TileType = TileID.HardenedSand;
                }

                // make the mound itself
                // WorldGen.Place3x3(x, y-1, (ushort)ModContent.TileType<TermiteMound>());
                WorldGen.PlaceTile(x, y - 1, ModContent.TileType<TermiteMound>());
            }
        }
    }
}