using BuriedHeavens.Content.Items.Accessories;
using BuriedHeavens.Content.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Common.GlobalNPCs {
	class BuriedHeavensShopNPC : GlobalNPC {
		public override void ModifyShop(NPCShop shop) {
			if (shop.NpcType == NPCID.Merchant) {
			    shop.Add<Datagem>(Condition.NpcIsPresent(ModContent.NPCType<Death>())); 
			    shop.Add<Datagem>(Condition.NpcIsPresent(ModContent.NPCType<Paleontologist>())); 
            }
		}
	}
}