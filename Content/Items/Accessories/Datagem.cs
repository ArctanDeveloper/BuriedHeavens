using BuriedHeavens.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Accessories {
    public class Datagem : ModItem {
        public override void SetDefaults() {
            Item.CloneDefaults(ItemID.Radar);
            Item.width = 30;
            Item.height = 32;
        }

        public override void UpdateInfoAccessory(Player player) {
            player.GetModPlayer<TreePlayer>().hasData = true;
        }
    }
}