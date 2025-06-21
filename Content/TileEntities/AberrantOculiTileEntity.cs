using BuriedHeavens.Content.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.TileEntities {
    public class AberrantOculiTileEntity : ModTileEntity {
        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
			return tile.HasTile && tile.TileType == ModContent.TileType<AberrantOculi>();
        }
    }
}