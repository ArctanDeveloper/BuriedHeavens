using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace BuriedHeavens.Content.Tiles {
    public class PolishingTable : ModTile {
        SoundStyle soundStylePolishingTableUse = new("BuriedHeavens/Assets/Sounds/Tiles/PolishingTableUse") {
			SoundLimitBehavior = SoundLimitBehavior.IgnoreNew
		};

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

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.CoordinatePadding = 0;
            TileObjectData.newTile.CoordinateHeights = [18];
            TileObjectData.addTile(Type);

			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

			AddMapEntry(new Color(64, 64, 96));
        }

        public override bool RightClick(int i, int j) {
            if (!BuriedHeavens.PolishableData.TryGetValue(Main.LocalPlayer.HeldItem.type, out PolishableData polishableData)) {
                return false;
            }

            Vector2 worldPosition = new Vector2(i, j).ToWorldCoordinates();

            SoundEngine.PlaySound(soundStylePolishingTableUse, worldPosition);

            if (polishableData.TryGetResult(out int type, out int amount)) {
                Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_TileInteraction(i, j), type, amount);
            }

            // TODO: Add a failure sound.

            return true;
        }
    }
}