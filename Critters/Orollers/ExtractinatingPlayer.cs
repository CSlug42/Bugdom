using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;

namespace Bugdom.Critters.Orollers
{
    public class ExtractinatingPlayer : ModPlayer
    {
        public int extractTimer = 0;
        private int[] extractable = ItemID.Sets.ExtractinatorMode;

        public override bool OnPickup(Item item)
        {
            //works if (timer is on and (the item is extractable or is luckily extractable))
            if (extractTimer > 0 && (extractable[item.type] != -1 || (new Random().Next(1, 51) == 50 && (item.type == ItemID.DirtBlock || item.type == ItemID.StoneBlock))))
            {
                int dropAmount = 1;
                int dropID = ItemID.CopperCoin;

                if (item.type == ItemID.DesertFossil)
                {
                    int sturdyChance = new Random().Next(1, 11);
                    if (sturdyChance == 10)
                    {
                        Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.FossilOre, 2, false, 0, false, false);
                        return false;
                    }
                }
                int chance = new Random().Next(1, 10001);

                if (chance <= 10)
                {
                    dropID = ItemID.GoldCoin;
                    dropAmount += 9;
                }
                else if (chance <= 140)
                {
                    dropID = ItemID.SilverCoin;
                    dropAmount += 17;
                }
                else if (chance <= 165)
                {
                    dropID = ItemID.Amethyst;
                }
                else if (chance <= 190)
                {
                    dropID = ItemID.Topaz;
                }
                else if (chance <= 215)
                {
                    dropID = ItemID.Sapphire;
                }
                else if (chance <= 240)
                {
                    dropID = ItemID.Emerald;
                }
                else if (chance <= 265)
                {
                    dropID = ItemID.Ruby;
                }
                else if (chance <= 290)
                {
                    dropID = ItemID.Diamond;
                }
                else if (chance <= 400)
                {
                    dropID = ItemID.Amber;
                }
                else if (chance <= 800)
                {
                    dropID = ItemID.CopperOre;
                }
                else if (chance <= 1200)
                {
                    dropID = ItemID.TinOre;
                }
                else if (chance <= 1600)
                {
                    dropID = ItemID.IronOre;
                }
                else if (chance <= 2000)
                {
                    dropID = ItemID.LeadOre;
                }
                else if (chance <= 2400)
                {
                    dropID = ItemID.SilverOre;
                }
                else if (chance <= 2800)
                {
                    dropID = ItemID.TungstenOre;
                }
                else if (chance <= 3200)
                {
                    dropID = ItemID.GoldOre;
                }
                else if (chance <= 3600)
                {
                    dropID = ItemID.PlatinumOre;
                }
                else if (chance == 3601)
                {
                    dropID = ItemID.PlatinumCoin;
                }
                else if (chance == 3602)
                {
                    Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.AmberMosquito, 1, false, 0, false, false);
                    return false;
                }
                else
                {
                    dropAmount += 35;
                }

                if (new Random().Next(1, 11) > 7)
                {
                    dropAmount++;
                }

                Item.NewItem(null, this.Entity.Center, 0, 0, dropID, dropAmount, false, 0, false, false);

                return false;
            }
            else
            {
                return base.OnPickup(item);
            }
        }

        public override void PostUpdate()
        {
            if (extractTimer > 0)
            {
                extractTimer--;
            }
        }
    }
}