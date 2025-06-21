using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items {
    public class AncientStarFragment : ModItem {
        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 30;
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
    }
}