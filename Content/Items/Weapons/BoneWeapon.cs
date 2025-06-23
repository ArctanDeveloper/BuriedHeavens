using System.Collections.Generic;
using System.Drawing.Printing;
using BuriedHeavens.Common.Systems;
using BuriedHeavens.Content.Projectiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Weapons {
    public enum LowerPartID {
        NONE,
        ONE,
        TWO
    }

    public enum HandleID {
        ONE,
        RIB
    }

    public enum UpperPartID {
        NONE,
        MAGE,
        SUMMONER,
        RANGER,
        MELEE
    }

    public class BoneWeapon : ModItem {
        public override string Texture => "BuriedHeavens/Content/Items/AncientStarFragment";
        public HandleID upperPart;
        public HandleID handlePart;
        public HandleID lowerPart;

        public override void SetDefaults() {
            Item.width = 1;
            Item.height = 1;

			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<BoneWeaponProjectile>();
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Main.myPlayer);
			return false;
		}

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0);

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI) {
			Vector2 drawOrigin = TextureAssets.MagicPixel.Value.Size() / 2f;
			Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawPosition, null, lightColor, rotation, drawOrigin, scale, SpriteEffects.None, 0);
            return false;
        }
    }
}