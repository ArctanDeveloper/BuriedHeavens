using System.Collections.Generic;
using System.Drawing.Printing;
using BuriedHeavens.Common.Players;
using BuriedHeavens.Common.Systems;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Tools {
    public class Monocle : ModItem {
        Vector2 lore = new();

        public override void SetStaticDefaults() {
            Item.ResearchUnlockCount = 1;
            Item.staff[Type] = true;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults() {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(gold: 1, silver: 50, copper: 75);
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item65;
            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override bool? UseItem(Player player) {
            if (!(player.ZoneDesert || player.ZoneUndergroundDesert || player.ZoneSnow)) {
                Point16 pos = Main.MouseWorld.ToTileCoordinates16();
                Point16 nearby = NotableSystem.Nearby(pos);
                if (pos.ToVector2().Distance(nearby.ToVector2()) < 240f) {
                    player.QuickSpawnItem(player.GetSource_ItemUse(Item), ModContent.ItemType<AncientStarFragment>(), Main.rand.Next(1, 7));
                    NotableSystem.notableLocations.Remove(nearby);

                    TreeSystem tree = ModContent.GetInstance<TreeSystem>();

                    if (!tree.MalkuthCheck())
                    {
                        tree.pathway = [..tree.pathway, 0];
                    }
                } else {
                    if (player.TryGetModPlayer(out TreePlayer tree) && tree.hasData) {
                        PopupText.NewText(new AdvancedPopupRequest() {
                            Color = Color.Lerp(Color.Green, Color.Red, pos.ToVector2().Distance(nearby.ToVector2()) / 3200f),
                            DurationInFrames = 120,
                            Text = $"{pos.ToVector2().Distance(nearby.ToVector2()):F0} away."
                        }, player.Top);
                    }
                    lore = pos.ToVector2().DirectionTo(nearby.ToVector2()).SafeNormalize(Vector2.UnitX);
                    Dust.QuickDustLine(player.Center + lore * 32, player.Center + lore * 48, 4, Color.Lerp(Color.Green, Color.Red, pos.ToVector2().Distance(nearby.ToVector2()) / 3200f));
                }
                return true;
            }
            return false;
        }
    }
}