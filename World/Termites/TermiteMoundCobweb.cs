using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Bugdom.World.Termites
{
    public class TermiteMoundCobweb : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.addTile(Type);
            AnimationFrameHeight = 54;

            //AddMapEntry(new Color(194, 1000, 6), "Termite Mound");

            MinPick = 65; // supposed to be achieved through bombs
        }

        public override bool RightClick(int x, int y)
        {
            Item.NewItem(null, x*16, y*16, 0, 0, ItemID.Cobweb, 1, false, 0, false, false);
            return true;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            NPC queen = NPC.NewNPCDirect(null, (i+1)*16, j*16, ModContent.NPCType<TermiteQueen>(), 0, 0f, 0f, 0f, 0f, 255);
            queen.velocity.Y -= 10;
            var rand = new Random();
            queen.velocity.X = rand.Next(-100, 100) / 100;

            // Retrieve the gore types
            Gore.NewGore(null, queen.position, queen.velocity / 2, Mod.Find<ModGore>("PlaceableMound_Gore_1").Type, 1);
            Gore.NewGore(null, queen.position, queen.velocity / 2, Mod.Find<ModGore>("PlaceableMound_Gore_2").Type, 1);
            Gore.NewGore(null, queen.position, queen.velocity / 5, Mod.Find<ModGore>("PlaceableMound_Gore_3").Type, 1);
        }

        public override bool CanDrop(int x, int y)
        {
            return false;
        }

        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) 
        {
            frameCounter++;
            if (frameCounter >= 10) 
            {
                frameCounter = 0;
                frame++;
                if (frame > 4)
                {
                    frame = 0;
                }
            }
        }
    }
}
