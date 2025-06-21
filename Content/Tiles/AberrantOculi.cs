using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using BuriedHeavens.Content.TileEntities;
using Terraria.DataStructures;
using BuriedHeavens.Common.Players;

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
			TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
			TileID.Sets.PreventsTileHammeringIfOnTopOfIt[Type] = true;
			TileID.Sets.AvoidedByMeteorLanding[Type] = true;

			AdjTiles = [TileID.WorkBenches];

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.HookPostPlaceMyPlayer = ModContent.GetInstance<AberrantOculiTileEntity>().Generic_HookPostPlaceMyPlayer;
			TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinateHeights = [16, 18];
            TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

			AddMapEntry(new Color(64, 64, 96));
        }

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			ModContent.GetInstance<AberrantOculiTileEntity>().Kill(i, j);
		}

        public override bool RightClick(int i, int j) {
			if (TileEntity.TryGet(i, j, out AberrantOculiTileEntity aberrantOculi) && Main.LocalPlayer.TryGetModPlayer(out AberrantOculiPlayer aberrantOculiPlayer)) {
                aberrantOculiPlayer.OpenUI(aberrantOculi);
            }

            return true;
        }
    }
}