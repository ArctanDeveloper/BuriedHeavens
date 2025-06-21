using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using BuriedHeavens.Content.TileEntities;
using Terraria.DataStructures;
using BuriedHeavens.Common.Players;

namespace BuriedHeavens.Content.Tiles {
    public class GeneSplicer : ModTile {
        public override void SetStaticDefaults() {
			Main.tileNoAttach[Type] = true;
			Main.tileObsidianKill[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileID.Sets.IgnoredByNpcStepUp[Type] = true;
			TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
			TileID.Sets.PreventsTileHammeringIfOnTopOfIt[Type] = true;
			TileID.Sets.AvoidedByMeteorLanding[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.HookPostPlaceMyPlayer = ModContent.GetInstance<GeneSplicerTileEntity>().Generic_HookPostPlaceMyPlayer;
			TileObjectData.newTile.CoordinatePadding = 0;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16];
			TileObjectData.addTile(Type);

            AddMapEntry(new Color(64, 64, 96));
        }

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			ModContent.GetInstance<GeneSplicerTileEntity>().Kill(i, j);
		}

        public override bool RightClick(int i, int j) {
			if (TileEntity.TryGet(i, j, out GeneSplicerTileEntity geneSplicer) && Main.LocalPlayer.TryGetModPlayer(out GeneSplicerPlayer geneSplicerPlayer)) {
                geneSplicerPlayer.OpenUI(geneSplicer);
            }

            return true;
        }

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
			if (!fail && TileEntity.TryGet(i, j, out GeneSplicerTileEntity tileEntity) && Main.netMode != NetmodeID.MultiplayerClient && tileEntity.inventory != null) {
				foreach (Item item in tileEntity.inventory) {
					Item.NewItem(new EntitySource_TileBreak(i, j), tileEntity.Position.X * 16, tileEntity.Position.Y * 16, 32, 32, item);
				}
                tileEntity.inventory.Clear();
            }
		}
    }
}