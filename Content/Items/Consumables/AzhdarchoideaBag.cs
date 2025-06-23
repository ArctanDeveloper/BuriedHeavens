using BuriedHeavens.Content.Items.Accessories;
using BuriedHeavens.Content.NPCs.Azhdarchoidea;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Consumables {
	public class AzhdarchoideaBag : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.BossBag[Type] = true;

			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 32;
			Item.height = 32;
			Item.rare = ItemRarityID.Purple;
			Item.expert = true;
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
			//itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<AzhdarchoideaMask>(), 7));
			itemLoot.Add(ItemDropRule.ExpertGetsRerolls(ModContent.ItemType<PrehestoricTooth>(), 5, 2));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BoneBuckler>()));
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Azhdarchoidea>()));
		}
	}
}