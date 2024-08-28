using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bugdom.Accessories.Attractants
{
    public class Attractant : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        private float counter = 0;
        protected float bugMax;
        protected int bugType;

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.shootSpeed = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            counter++;
            if (counter == Item.useTime)
            {
                counter = 0;

                NPC[] bugs = player.GetModPlayer<BugCatcher>().bugs;

                float max = bugMax + player.GetModPlayer<BugCatcher>().bugExtra;

                //code to clean up bugs that are too far away or caught
                for (int i = 0; i < max; i++)
                {
                    if (bugs[i] != null && bugs[i].Distance(player.Center) > 1000)
                    {
                        bugs[i].active = false;
                        bugs[i] = null;
                    }
                    if (bugs[i] != null && bugs[i].active == false)
                    {
                        bugs[i] = null;
                    }
                }

                //code to add new bugs
                for (int i = 0; i < max; i++)
                {
                    if (bugs[i] == null)
                    {
                        bugs[i] = NPC.NewNPCDirect(null, player.Left, bugType, 0, 0f, 0f, 0f, 0f, 255);
                        break;
                    }
                }
            }
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !(CheckIsAttractant(equippedItem) && CheckIsAttractant(incomingItem));
        }

        public static bool CheckIsAttractant(Item item)
        {
            return item.type == ModContent.ItemType<Attractants.AppleCiderVinegar.AppleCiderVinegar>()
                || item.type == ModContent.ItemType<Attractants.BugZapper.BugZapper>()
                || item.type == ModContent.ItemType<Attractants.BoneInTheChamber.BoneInTheChamber>();
        }
    }
}