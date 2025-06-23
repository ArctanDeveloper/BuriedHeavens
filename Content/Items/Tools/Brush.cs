using BuriedHeavens.Common.Players;
using BuriedHeavens.Common.Systems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Tools {
    public class Brush : ModItem {
        Vector2 lore = new();
        public static List<int> BrushableTiles = [];

        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
            Item.staff[Type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults() {
            Item.width = 22;
            Item.height = 22;
            Item.value = Item.buyPrice(gold: 1, silver: 50, copper: 75);
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item65;
            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override bool? UseItem(Player player) {
            if (player.ZoneDesert || player.ZoneUndergroundDesert || player.ZoneSnow) {
                Point16 pos = Main.MouseWorld.ToTileCoordinates16();
                Tile tile = Framing.GetTileSafely(pos);
                if (BrushableTiles.Contains(tile.TileType)) {
                    Point16 nearby = NotableSystem.Nearby(pos);
                    if (pos.ToVector2().Distance(nearby.ToVector2()) < 240f) {
                        player.QuickSpawnItem(player.GetSource_ItemUse(Item), Main.rand.NextBool(3) ? ModContent.ItemType<AncientDebris>() : ModContent.ItemType<AncientScrap>(), Main.rand.Next(1, 7));
                        NotableSystem.notableLocations.Remove(nearby);

                        if (!ModContent.GetInstance<TreeSystem>().MalkuthCheck())
                        {
                            ModContent.GetInstance<TreeSystem>().pathway.Append(0);
                        }
                    } else {
                        lore = pos.ToVector2().DirectionTo(nearby.ToVector2()).SafeNormalize(Vector2.UnitX);
                        Dust.QuickDustLine(player.Center + lore * 32, player.Center + lore * 48, 4, Color.Lerp(Color.Green, Color.Red, pos.ToVector2().Distance(nearby.ToVector2()) / 3200f));
                    }
                }
                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe()
                .AddIngredient(ItemID.Wood, 3)
                .AddIngredient(ItemID.GrassSeeds, 6)
                .AddTile(TileID.WorkBenches);
        }
    }
}