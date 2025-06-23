using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Items.Accessories {
	[AutoloadEquip(EquipType.Shield)]
	public class BoneBuckler : ModItem
	{
		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 28;
			Item.value = Item.buyPrice(10);
			Item.rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.accessory = true;

			Item.defense = 50;
			Item.lifeRegen = 10;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetDamage(DamageClass.Generic) += 1.5f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.5f;
            player.endurance = 1f - (0.01f * (1f - player.endurance));
			player.GetModPlayer<BoneBucklerDashPlayer>().BoneBucklerEquipped = true;
		}
	}

	public class BoneBucklerDashPlayer : ModPlayer {
		public const int DashDown = 0;
		public const int DashUp = 1;
		public const int DashRight = 2;
		public const int DashLeft = 3;

		public const int DashCooldown = 50;
		public const int DashDuration = 60;

		public const float DashVelocity = 15f;

		public int DashDir = -1;

		public bool BoneBucklerEquipped;
        public int DashDelay = 0;
        public int DashTimer = 0;

		public override void ResetEffects() {
			BoneBucklerEquipped = false;

			if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15) {
				DashDir = DashDown;
			} else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15) {
				DashDir = DashUp;
			} else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15 && Player.doubleTapCardinalTimer[DashLeft] == 0) {
				DashDir = DashRight;
			} else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15 && Player.doubleTapCardinalTimer[DashRight] == 0) {
				DashDir = DashLeft;
			} else {
				DashDir = -1;
			}
		}

		public override void PreUpdateMovement() {
			if (CanUseDash() && DashDir != -1 && DashDelay == 0) {
				Vector2 newVelocity = Player.velocity;

				switch (DashDir) {
					case DashUp when Player.velocity.Y > -DashVelocity:
					case DashDown when Player.velocity.Y < DashVelocity: {
							float dashDirection = DashDir == DashDown ? 1.5f : -2.75f;
							newVelocity.Y = dashDirection * DashVelocity;
							break;
						}
					case DashLeft when Player.velocity.X > -DashVelocity:
					case DashRight when Player.velocity.X < DashVelocity: {
							float dashDirection = DashDir == DashRight ? 1 : -1;
							newVelocity.X = dashDirection * DashVelocity;
							break;
						}
					default:
						return;
				}

				DashDelay = DashCooldown;
				DashTimer = DashDuration;
				Player.velocity = newVelocity;

                for (int i = 0; i < 25; i++) {
                    Dust.QuickDust((int)Player.Center.X, (int)Player.Center.Y, Color.White);
                }
            }

			if (DashDelay > 0)
				DashDelay--;

			if (DashTimer > 0) {
				Player.eocDash = DashTimer;
				Player.armorEffectDrawShadowEOCShield = true;
                Player.lifeRegen = 5;

                DashTimer--;
			}
		}

		private bool CanUseDash() {
            return BoneBucklerEquipped
                && Player.dashType == DashID.None
                && !Player.setSolar;
        }
	}
}