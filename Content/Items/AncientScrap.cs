using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items {
    public class AncientScrap : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Type] = Type;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.value = 100;
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
            switch (Main.rand.Next(0, 12)) {
                case < 2:
                    resultType = ItemID.IronBar;
                    resultStack = Main.rand.Next(0, 12);
                    break;
                case < 5:
                    resultType = ItemID.LeadBar;
                    resultStack = Main.rand.Next(0, 5);
                    break;
                case < 8:
                    resultType = ItemID.TungstenBar;
                    resultStack = Main.rand.Next(0, 16);
                    break;
                case < 12:
                    resultType = ItemID.SilverBar;
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