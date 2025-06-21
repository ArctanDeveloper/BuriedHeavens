using System.Collections.Generic;
using BuriedHeavens.Common.UI;
using BuriedHeavens.Content.TileEntities;
using Terraria.ModLoader;

namespace BuriedHeavens.Common.Players {
    public class GeneSplicerPlayer : ModPlayer {
        public GeneSplicerTileEntity tileEntity;
        public bool hasUIOpen = false;
        public bool dirty = false;
        public void OpenUI(GeneSplicerTileEntity tileEntity) {
            this.tileEntity = tileEntity;
            dirty = true;
        }

        public void CloseUI(List<BetterItemSlot> itemSlots) {
            StateSave(itemSlots);
            this.tileEntity = null;
        }

        public void StateSave(List<BetterItemSlot> itemSlots) {
            tileEntity.inventory.Clear();
            foreach (BetterItemSlot itemSlot in itemSlots) {
                if (itemSlot.Item.IsAir) {
                    continue;
                }
                tileEntity.inventory.Add(itemSlot.Item);
            }
        }
    }
}