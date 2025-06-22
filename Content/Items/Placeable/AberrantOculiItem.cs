using BuriedHeavens.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Placeable {
    public class AberrantOculiItem : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<AberrantOculi>());
            Item.width = 32;
            Item.height = 64;
            Item.value = Item.buyPrice(silver: 10, copper: 50);
        }

        public override void AddRecipes() {
            CreateRecipe(1)
                .AddTile(TileID.Sawmill)
                .AddRecipeGroup(RecipeGroupID.Wood, 20)
                .AddRecipeGroup(nameof(ItemID.SilverBar), 6)
                .AddIngredient(ItemID.Silk, 8)
                .Register();
        }
    }
}