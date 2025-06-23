using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items {
    public class PaleontologistsCertificate : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.width = 30;
            Item.height = 20;
            Item.value = Item.buyPrice(platinum: 10, gold: 5, silver: 50, copper: 75);
            Item.rare = ItemRarityID.Expert;
        }
    }
}