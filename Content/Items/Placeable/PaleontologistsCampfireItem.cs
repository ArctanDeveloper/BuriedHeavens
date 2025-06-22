using BuriedHeavens.Content.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Placeable {
    public class PaleontologistsCampfireItem : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<PaleontologistsCampfire>());
        }
    }
}