using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Placeable {
    public class AncientDebris : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Type] = Type;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
            Item.value = 100;
        }

        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack) {
            switch (Main.rand.Next(0, 8)) {
                case < 2:
                    resultType = ItemID.FossilOre;
                    resultStack = Main.rand.Next(0, 3);
                    return;
                case < 5:
                    resultType = ItemID.SiltBlock;
                    resultStack = Main.rand.Next(0, 5);
                    return;
                case < 8:
                    resultType = ItemID.MudBlock;
                    resultStack = Main.rand.Next(0, 16);
                    return;
            }
        }
    }
}