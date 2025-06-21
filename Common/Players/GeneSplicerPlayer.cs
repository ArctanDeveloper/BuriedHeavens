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
            if (tileEntity != null)
                StateSave(itemSlots);
            tileEntity = null;
        }

        public void StateSave(List<BetterItemSlot> itemSlots) {
            if (tileEntity.inventory.Count > 0) {
                return;
            }
            tileEntity.inventory = [];
            foreach (BetterItemSlot itemSlot in itemSlots) {
                //Mod.Logger.Debug($"{itemSlot.Item} {itemSlot.Item.IsAir}");
                if (itemSlot.Item.IsAir) {
                    continue;
                }
                tileEntity.inventory.Add(itemSlot.Item);
            }
        }
    }
}