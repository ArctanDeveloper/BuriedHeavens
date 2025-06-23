using Terraria;
using Terraria.ModLoader;

namespace BuriedHeavens.Common.Players {
    public class TreePlayer : ModPlayer {
		public bool hasData;

		public override void ResetInfoAccessories() {
			hasData = false;
		}

		public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer) {
			if (otherPlayer.GetModPlayer<TreePlayer>().hasData) {
				hasData = true;
			}
		}
    }
}