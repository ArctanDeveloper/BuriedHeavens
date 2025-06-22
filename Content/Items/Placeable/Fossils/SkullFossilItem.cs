using BuriedHeavens.Content.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Placeable.Fossils {
    public class SkullFossilItem : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.value = 100;
            Item.DefaultToPlaceableTile(ModContent.TileType<SkullFossil>());
        }
    }
}