using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuriedHeavens.Content.Tiles {
    public class PolishingTable : ModTile {
        public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.CoordinatePadding = 0;

			TileObjectData.addTile(Type);

			AddMapEntry(new Color(64, 64, 96));
        }
    }
}