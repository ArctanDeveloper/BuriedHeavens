using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Placeable {
    public class AzhdarchoideaRelic : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.AzhdarchoideaRelic>());
            Item.width = 48;
            Item.height = 64;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
            Item.value = Item.buyPrice(silver: 10, copper: 50);
        }
    }
}