using BuriedHeavens.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Placeable {
    public class PolishingTableItem : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<PolishingTable>());
            Item.width = 32;
            Item.height = 18;
            Item.value = 100;
        }

        public override void AddRecipes() {
            CreateRecipe(1)
                .AddTile(TileID.WorkBenches)
                .AddRecipeGroup(RecipeGroupID.Wood, 20)
                .AddRecipeGroup(RecipeGroupID.IronBar, 6)
                .AddIngredient(ItemID.EmptyBucket)
                .Register();
        }
    }
}