using System.Collections.Generic;
using System.Linq;
using BuriedHeavens.Content.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuriedHeavens.Content.TileEntities {
    public class GeneSplicerTileEntity : ModTileEntity {
        public List<Item> inventory = [];

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];
			return tile.HasTile && tile.TileType == ModContent.TileType<GeneSplicer>();
        }

        public override void SaveData(TagCompound tag) {
            tag.Set("inventory", inventory);
        }

        public override void LoadData(TagCompound tag) {
			if (tag.ContainsKey("inventory")) {
                inventory = tag.GetList<Item>("inventory").ToList();
            }
		}
    }
}