using BuriedHeavens.Content.Items;
using BuriedHeavens.Content.Items.Consumables;
using BuriedHeavens.Content.Items.Placeable.Fossils;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BuriedHeavens.Common.UI.GeneSplicerUI {
    internal partial class GeneSplicerUIState : UIState {
        private static bool RecipeCheck(List<BetterItemSlot> itemSlots, out int result)
        {
            //hardcoded recipe bullshit goooo
            //monstrosity
            List<int> currentCombo = GetCurrentCombination(itemSlots);
            List<int> bossSpawnEgg = [ModContent.ItemType<SkeletalFossilItem>(), ModContent.ItemType<SkullFossilItem>(),
            ModContent.ItemType<ToothFossilItem>(), ModContent.ItemType<HornFossilItem>(), ModContent.ItemType<AncientStarFragment>()];
            if (currentCombo.Exists(x => x == bossSpawnEgg[0]) && currentCombo.Exists(x => x == bossSpawnEgg[1])
                && currentCombo.Exists(x => x == bossSpawnEgg[2]) && currentCombo.Exists(x => x == bossSpawnEgg[3])
                && currentCombo.Exists(x => x == bossSpawnEgg[4]) && itemSlots.Count <= 6)
            {
                result = ModContent.ItemType<DubiousDinosaurEgg>();
                return true;
            }
            result = ItemID.None;
            return false;
        }
    }
}