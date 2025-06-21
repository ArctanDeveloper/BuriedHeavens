using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuriedHeavens.Content.Tiles {
    public class AberrantOculi : ModTile {
        public override void SetStaticDefaults() {
			Main.tileFrameImportant[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;
            TileID.Sets.AvoidedByMeteorLanding[Type] = true;

			AdjTiles = [TileID.WorkBenches];

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinateHeights = [16, 18];
            TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

			AddMapEntry(new Color(64, 64, 96));
        }

        public override bool RightClick(int i, int j) {
            return base.RightClick(i, j);
        }
    }
}