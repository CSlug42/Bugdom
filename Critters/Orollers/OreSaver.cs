using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using Terraria.IO;

namespace Bugdom.Critters.Orollers
{
    public class OreSaver : ModSystem
    {
        public static int copperTier = -1;
        public static int ironTier = -1;
        public static int silverTier = -1;
        public static int goldTier = -1;

        private OreNotePass noteTaker; 

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            noteTaker = new OreNotePass("Ore Notetaking Pass", 100f);
            tasks.Insert(genIndex - 1, noteTaker);
        }

        public override void PostWorldGen()
        {
            copperTier = noteTaker.copper;
            ironTier = noteTaker.iron;
            silverTier = noteTaker.silver;
            goldTier = noteTaker.gold;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["saveCopperOreTier"] = copperTier;
            tag["saveIronOreTier"] = ironTier;
            tag["saveSilverOreTier"] = silverTier;
            tag["saveGoldOreTier"] = goldTier;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            try 
            {
                copperTier = tag.GetAsInt("saveCopperOreTier");
                ironTier = tag.GetAsInt("saveIronOreTier");
                silverTier = tag.GetAsInt("saveSilverOreTier");
                goldTier = tag.GetAsInt("saveGoldOreTier");
            }
            catch 
            {
                copperTier = -1;
                ironTier = -1;
                silverTier = -1;
                goldTier = -1;
            }
        }

        private class OreNotePass : GenPass
        {
            public int copper = -1;
            public int iron = -1;
            public int silver = -1;
            public int gold = -1;

            public OreNotePass(string name, float loadWeight) : base(name, loadWeight)
            {
            }

            // 8. The ApplyPass method is where the actual world generation code is placed.
            protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
            {
                // 9. Setting a progress message is always a good idea. This is the message the user sees during world generation and can be useful to help users and modders identify passes that are stuck.      
                progress.Message = "Feeding orollers";

                copper = (int)WorldGen.SavedOreTiers.Copper;
                iron = (int)WorldGen.SavedOreTiers.Iron;
                silver = (int)WorldGen.SavedOreTiers.Silver;
                gold = (int)WorldGen.SavedOreTiers.Gold;
            }
        }
    }


    
}