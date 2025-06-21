using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items {
    public class AncientDebris : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Type] = Type;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
            Item.value = Item.buyPrice(silver: 1);
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.noMelee = true;
            Item.maxStack = Item.CommonMaxStack;
        }

        public override void ExtractinatorUse(int extractinatorBlockType, ref int resultType, ref int resultStack) {
            switch (Main.rand.Next(0, 8)) {
                case < 2:
                    resultType = ItemID.FossilOre;
                    resultStack = Main.rand.Next(0, 3);
                    break;
                case < 5:
                    resultType = ItemID.SiltBlock;
                    resultStack = Main.rand.Next(0, 5);
                    break;
                case < 8:
                    resultType = ItemID.MudBlock;
                    resultStack = Main.rand.Next(0, 16);
                    break;
                default:
                    resultType = ItemID.SandBlock;
                    resultStack = Main.rand.Next(1, 10);
                    break;
            }
        }
    }
}