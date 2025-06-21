using BuriedHeavens.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Placeable {
    public class GeneSplicerItem : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults() {
            Item.DefaultToPlaceableTile(ModContent.TileType<GeneSplicer>());
            Item.width = 42;
            Item.height = 80;
            Item.value = 7500;
        }

        public override void AddRecipes() {
            CreateRecipe(1)
                .AddTile(TileID.HeavyWorkBench)
                .AddRecipeGroup(nameof(ItemID.SilverBar), 6)
                .AddRecipeGroup(RecipeGroupID.IronBar, 12)
                .AddIngredient(ItemID.Glass, 20)
                .Register();
        }
    }
}