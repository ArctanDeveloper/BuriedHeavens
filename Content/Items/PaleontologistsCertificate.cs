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
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Expert;
        }
    }
}