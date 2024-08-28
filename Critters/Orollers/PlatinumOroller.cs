using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace Bugdom.Critters.Orollers
{
	public class PlatinumOroller : CopperOroller
	{	
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GlowingSnail];
        }

		public override void SetDefaults() {
            base.SetDefaults();
            NPC.catchItem = ModContent.ItemType<PlatinumOrollerItem>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Legends say that orollers collected stardust into sparkling rock and deposited them into the ground - thus metals were spread through the world.These have an affinity for platinum."),
                new RareSpawnBestiaryInfoElement(5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {

            float spawnModifier = 0.5f;
            if (Critters.Orollers.OreSaver.goldTier == TileID.Gold)
            {
                spawnModifier = 0.1f;
            }
            else if ((Critters.Orollers.OreSaver.goldTier == TileID.Platinum))
            {
                spawnModifier = 0.9f;
            }

            return SpawnCondition.Cavern.Chance * 0.05f * spawnModifier;
        }

		public override void OnCaughtBy(Player player, Item item, bool failed) {
            if (hasOrb)
            {
                Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.PlatinumOre, (int)Math.Ceiling((1 + item.pick * 0.01) * new Random().Next(20, 32)), false, 0, false, false);
            }
        }

        public override void DrawEffects(ref Color drawColor)
		{
            var rand = new Random();   
			if (rand.Next(100) < 1) {
                //NewDustDirect (Vector2 Position, int Width, int Height, int Type, float SpeedX=0f, float SpeedY=0f, int Alpha=0, Color newColor=default(Color), float Scale=1f)
                Dust dust = Dust.NewDustDirect(Entity.position + (Entity.velocity * 60), 1, 1, 43, 0f, 0f, 0, drawColor, 0.1f);
            }
        }
    }

    public class PlatinumOrollerItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 5000000;
            Item.bait = 45;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.makeNPC = ModContent.NPCType<PlatinumOroller>();
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.SetNameOverride("Platinum Oroller");
            Item.rare = ItemRarityID.Green;
        }
    }

    public class PlatinumOrexcavator : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 23;
            Item.DamageType = DamageClass.Melee;
            Item.width = 65;
            Item.height = 65;
            Item.useTime = 19;
            Item.useAnimation = 19;
            Item.useStyle = 1;
            Item.knockBack = 6;
            Item.value = 2500000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.pick = 59; // pick power will determine extra ore from orollers
            ItemID.Sets.CatchingTool[Item.type] = true; // important!
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;

            Item.tileBoost = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 10);
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddIngredient(ModContent.ItemType<PlatinumOrollerItem>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<Orollers.ExtractinatingPlayer>().extractTimer = Item.useTime + 4;
            return base.UseItem(player);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var line = new TooltipLine(Mod, "Extractinator Reminder", "The fine net attached to this pick automatically extractinates some sediments");
            tooltips.Add(line);
        }
    }
}