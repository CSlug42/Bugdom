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
	public class GoldOroller : CopperOroller
	{	
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.GlowingSnail];
        }

		public override void SetDefaults() {
            base.SetDefaults();
            NPC.catchItem = ModContent.ItemType<GoldOrollerItem>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
                new FlavorTextBestiaryInfoElement("Legends say that orollers collected stardust into sparkling rock and deposited them into the ground - thus metals were spread through the world. These have an affinity for gold."),
                new RareSpawnBestiaryInfoElement(5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {

            float spawnModifier = 0.5f;
            if (Critters.Orollers.OreSaver.goldTier == TileID.Platinum)
            {
                spawnModifier = 0.1f;
            }
            else if ((Critters.Orollers.OreSaver.goldTier == TileID.Gold))
            {
                spawnModifier = 0.9f;
            }

            return SpawnCondition.Cavern.Chance * 0.05f * spawnModifier;
        }

		public override void OnCaughtBy(Player player, Item item, bool failed) {
            if (hasOrb)
            {
                Item.NewItem(null, this.Entity.Center, 0, 0, ItemID.GoldOre, (int)Math.Ceiling((1 + item.pick * 0.01) * new Random().Next(20, 32)), false, 0, false, false);
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

    public class GoldOrollerItem : ModItem
    {
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.Bugdom.hjson file.

        public override void SetDefaults()
        {
            Item.value = 5000000;
            Item.bait = 45;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.makeNPC = ModContent.NPCType<GoldOroller>();
            Item.useStyle = 1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.width = 12;
            Item.height = 12;
            Item.noUseGraphic = true;
            Item.SetNameOverride("Gold Oroller");
            Item.rare = ItemRarityID.Green;
        }
    }

    public class GoldOrexcavator : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.width = 35;
            Item.height = 35;
            Item.useTime = 14;
            Item.useAnimation = 15;
            Item.useStyle = 1;
            Item.knockBack = 3;
            Item.value = 2500000;
            Item.rare = 2;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.pick = 55; // pick power will determine extra ore from orollers
            ItemID.Sets.CatchingTool[Item.type] = true; // important!
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ItemRarityID.Green;

            Item.tileBoost = -1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 10);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.Silk, 3);
            recipe.AddIngredient(ModContent.ItemType<GoldOrollerItem>(), 1);
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