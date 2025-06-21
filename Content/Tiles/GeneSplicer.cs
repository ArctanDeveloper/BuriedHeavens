using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuriedHeavens.Content.Tiles {
    public class GeneSplicer : ModTile {
        public override void SetStaticDefaults() {
			Main.tileNoAttach[Type] = true;
			Main.tileObsidianKill[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            TileID.Sets.AvoidedByMeteorLanding[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16];
			TileObjectData.addTile(Type);

            AddMapEntry(new Color(64, 64, 96));
        }

        public override bool RightClick(int i, int j) {
            return base.RightClick(i, j);
        }
    }
}