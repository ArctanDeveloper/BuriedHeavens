using BuriedHeavens.Content.TileEntities;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace BuriedHeavens.Common.Players {
    public class AberrantOculiPlayer : ModPlayer {
        public AberrantOculiTileEntity tileEntity;
        public bool hasUIOpen = false;

        public void OpenUI(AberrantOculiTileEntity tileEntity) {
            this.tileEntity = tileEntity;
            StateLoad();
        }

        public void CloseUI() {
            StateSave();
            tileEntity = null;
        }

        public void StateSave() {

        }

        public void StateLoad() {

        }
    }
}