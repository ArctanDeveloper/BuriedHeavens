using BuriedHeavens.Content.Items;
using BuriedHeavens.Content.Items.Accessories;
using BuriedHeavens.Content.Items.Consumables;
using BuriedHeavens.Content.Items.Placeable;
using BuriedHeavens.Content.Items.Placeable.Fossils;
using BuriedHeavens.Content.Items.Tools;
using BuriedHeavens.Content.Items.Weapons;
using BuriedHeavens.Content.Tiles;
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
            List<int> gunWeapon = [ModContent.ItemType<SkeletalFossilItem>(), ItemID.FlintlockPistol];
            if (currentCombo.Exists(x => x == gunWeapon[0]) && currentCombo.Exists(x => x == gunWeapon[1])
                 && itemSlots.Count <= 3)
            {
                result = ModContent.ItemType<BoneWeapon>();
                return true;
            }
            List<int> toothAccessory = [ModContent.ItemType<ToothFossilItem>(), ItemID.FleshBlock];
            if (currentCombo.Exists(x => x == toothAccessory[0]) && currentCombo.Exists(x => x == toothAccessory[1])
                && itemSlots.Count <= 3)
            {
                result = ModContent.ItemType<PrehestoricTooth>();
                return true;
            }
            List<int> boneShield = [ModContent.ItemType<SkeletalFossilItem>(), ModContent.ItemType<HornFossilItem>(), ItemID.EoCShield];
            if (currentCombo.Exists(x => x == boneShield[0]) && currentCombo.Exists(x => x == boneShield[1])
                && currentCombo.Exists(x => x == boneShield[2]) && itemSlots.Count <= 4)
            {
                result = ModContent.ItemType<BoneBuckler>();
                return true;
            }
            List<int> campfireSummon = [ItemID.Campfire, ModContent.ItemType<Brush>()];
            if (currentCombo.Exists(x => x == campfireSummon[0]) && currentCombo.Exists(x => x == campfireSummon[1]) && itemSlots.Count <= 3)
            {
                result = ModContent.ItemType<PaleontologistsCampfireItem>();
                return true;
            }
            result = ItemID.None;
            return false;
        }
    }
}