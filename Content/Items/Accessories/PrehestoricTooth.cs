using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Accessories {
    public class PrehestoricTooth : ModItem {
        public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.width = 30;
            Item.height = 32;
			Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            player.GetArmorPenetration(DamageClass.Generic) *= 1.5f;
            player.endurance *= 0.6f;
        }
    }
}