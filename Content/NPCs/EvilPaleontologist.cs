using System;
using System.Collections.Generic;
using BuriedHeavens.Common.Systems;
using BuriedHeavens.Content.Items;
using BuriedHeavens.Content.Items.Placeable.Fossils;
using BuriedHeavens.Content.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace BuriedHeavens.Content.NPCs {
	[AutoloadBossHead]
    public class EvilPaleontologist : ModNPC {
        public override string HeadTexture => "BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaHead_Head";
        public override string BossHeadTexture => "BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaHead_Head";

        public int AITimer { get => (int)NPC.ai[0]; set => NPC.ai[0] = value; }
        public int AIState { get => (int)NPC.ai[1]; set => NPC.ai[1] = value; }
		public Point16 HomeCampfire { get => new Point16((int)NPC.ai[2], (int)NPC.ai[3]); set { NPC.ai[2] = value.X; NPC.ai[3] = value.Y; } }
        const int MAX_TIME = 3600;

		public override void SetDefaults() {
			NPC.friendly = true;
			NPC.width = 40;
			NPC.height = 56;
			NPC.damage = 45;
			NPC.defense = 25;
			NPC.lifeMax = 1250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
            NPC.immortal = true;
            NPC.boss = true;
        }

        public override void AI() {
            NPC.TargetClosest();
			AITimer++;
            if (AIState == 0) {
				if (AITimer % 20 == 0 && !WorldGen.IsTileNearby(HomeCampfire.X, HomeCampfire.Y, ModContent.TileType<PaleontologistsCampfire>(), 12)) {
					AIState = 1;
					NPC.friendly = false;
					AITimer = 0;
                }
			} else {
				if (AITimer > MAX_TIME) {
                    SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, NPC.Center);
                    PopupText.NewText(new AdvancedPopupRequest() {
                        Color = Color.Blue,
                        DurationInFrames = 60,
                        Text = "Ah, fine.",
                        Velocity = new Vector2(0, -0.1f)
                    }, NPC.Top);
                    NPC.Transform(ModContent.NPCType<Paleontologist>());
                    TreeSystem.UnlockedPaleontologistSpawn = true;
                    TreeSystem tree = ModContent.GetInstance<TreeSystem>();
                    if (tree.worldTree == -1) {
                        tree.pathway = [0];
                        tree.worldTree = (int)WorldTreeID.LIFE;
                    }
                } else if (AITimer % 20 == 0) {
					if (NPC.HasValidTarget) {
                        Vector2 distance = NPC.targetRect.Center.ToVector2() - NPC.Center;
                        float dist = distance.Length();
						if (dist > 2400) {
                            Main.player[NPC.target].KillMe(PlayerDeathReason.ByNPC(NPC.whoAmI), 99999, 0);
                            HandleDespawn();
                        } else {
                        	Vector2 direction = distance.SafeNormalize(Vector2.UnitX).RotatedBy(Main.rand.NextFloat(-MathF.PI * 0.1f, MathF.PI * 0.1f));
							Projectile grenade = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, direction * 15f, ProjectileID.Grenade, 65, 0f);
							grenade.hostile = true;
							grenade.friendly = false;
							grenade.timeLeft = 180;
						}
                    } else {
                        HandleDespawn();
                    }
				}
            }
        }

        public override bool NeedSaving() => true;

        public override void SaveData(TagCompound tag) {
            tag.Add("HomeCampfire", HomeCampfire);
        }

        public override void LoadData(TagCompound tag) {
            if (tag.TryGet("HomeCampfire", out Point16 homeCampfire)) {
                HomeCampfire = homeCampfire;
            }
        }

		private void HandleDespawn() {
            TreeSystem.GeneratePaleontologistSpawn();
            NPC.life = 0;
        }
    }
}