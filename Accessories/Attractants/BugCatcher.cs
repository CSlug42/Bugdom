using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bugdom.Accessories.Attractants
{
    public class BugCatcher : ModPlayer
    {
        public float bugExtra;
        public NPC[] bugs = new NPC[20];

        public override void PreUpdate()
        {
            bugExtra = 0;
            //find and set the active attractant along with disabling other ones being equipped
        }
    }
}