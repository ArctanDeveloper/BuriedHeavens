using BuriedHeavens.Common.Systems;
using BuriedHeavens.Content.Items.Accessories;
using BuriedHeavens.Content.Items.Consumables;
using BuriedHeavens.Content.Items.Placeable;
using BuriedHeavens.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.NPCs.Azhdarchoidea {
    public enum AzhdarchoideaAIState {
        FOLLOW,
        PROJECTILE_RAIN,
        DIVE_BOMB,
        PROJECTILE_SWARM
    }

    [AutoloadBossHead]
    public class Azhdarchoidea : ModNPC {
        public override string Texture => "BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaHead";
        public override string BossHeadTexture => "BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaHead_Head";
        public Asset<Texture2D> BodyTexture;
        public Asset<Texture2D> BackgroundHindLegTexture;
        public Asset<Texture2D> ForegroundHindLegTexture;
        public Asset<Texture2D> ForegroundUpperHindLegTexture;
        public new Asset<Texture2D> HeadTexture;
        public Asset<Texture2D> WingBackgroundTexture;
        public Asset<Texture2D> WingForegroundTexture;

        static Vector2 BodyOffset = new(0f, 0f);
        static Vector2 BackgroundHindLegOffset = new(2f, 12f);
        static Vector2 ForegroundHindLegOffset = new(-12f, 8f);
        static Vector2 ForegroundUpperHindLegOffset = new(-12f, 8f);
        static Vector2 HeadOffset = new(8, -46);
        static Vector2 WingBackgroundOffset = new(8, 0);
        static Vector2 WingForegroundOffset = new(-10, -14);

        static Vector2 BodyOrigin = new(50f, 46f);
        static Vector2 BackgroundHindLegOrigin = new(8f, 6f);
        static Vector2 ForegroundHindLegOrigin = new(0f, 0f);
        static Vector2 ForegroundUpperHindLegOrigin = new(0f, 0f);
        static Vector2 HeadOrigin = new(16f, 64f);
        static Vector2 WingBackgroundOrigin = new(4f, 22f);
        static Vector2 WingForegroundOrigin = new(136, 26);

        static Rectangle BodyRect = new();
        static Rectangle BackgroundHindLegRect = new();
        static Rectangle ForegroundHindLegRect = new();
        static Rectangle ForegroundUpperHindLegRect = new();
        static Rectangle HeadRect = new();
        static Rectangle WingBackgroundRect = new();
        static Rectangle WingForegroundRect = new();


        public int AIState { get => (int)NPC.ai[0]; set => NPC.ai[0] = value; }
        public int AITimer { get => (int)NPC.ai[1]; set => NPC.ai[1] = value; }

        int timer = 0;

        public override void SetDefaults() {
            NPC.boss = true;
            NPC.damage = 100;
            NPC.defense = 25;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lifeMax = 750000;
            NPC.width = 50;
            NPC.height = 50;

            if (!Main.dedServ) {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/AncientExtinctiongg");
                SceneEffectPriority = SceneEffectPriority.BossHigh;
            }
        }

        public override void Load() {
            BodyTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaBody");
            BackgroundHindLegTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaBackgroundHindLeg");
            ForegroundHindLegTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaForegroundHindLeg");
            ForegroundUpperHindLegTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaForegroundUpperHindLeg");
            HeadTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaHead");
            WingBackgroundTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaWingBackground");
            WingForegroundTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaWingForeground");
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        const int FOLLOW_TIME = 600;
        const int DIVE_BOMB_TIME = 240;
        const int PROJECTILE_RAIN_TIME = 320;
        const int PROJECTILE_SWARM_TIME = 640;

        public override void AI() {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            Player player = Main.player[NPC.target];

            timer++;
            AITimer++;

            // It's up to you, just code something. We don't got time.
            switch (AIState) {
                case (int)AzhdarchoideaAIState.FOLLOW:
                    MoveSide(player);
                    if (AITimer >= FOLLOW_TIME) {
                        AIState = (int)AzhdarchoideaAIState.DIVE_BOMB;
                        AITimer = 0;
                    }
                    break;
                case (int)AzhdarchoideaAIState.DIVE_BOMB:
                    if (NPC.HasValidTarget && AITimer % 120 == 1) 
                        NPC.velocity = NPC.DirectionTo(player.Center);
                    NPC.velocity *= 1.1f;
                    NPC.velocity = NPC.velocity.SafeNormalize(Vector2.UnitX) * Math.Min(NPC.velocity.Length(), 12.5f);
                    if (AITimer >= DIVE_BOMB_TIME) {
                        AIState = NPC.life > NPC.lifeMax * 0.75f ? (int)AzhdarchoideaAIState.FOLLOW : (int)AzhdarchoideaAIState.PROJECTILE_RAIN;
                        AITimer = 0;
                    }
                    break;
                case (int)AzhdarchoideaAIState.PROJECTILE_RAIN:
                    MoveSide(player);
                    if (NPC.HasValidTarget && AITimer % 120 == 1) {
                        for (int i = (int)player.Center.X - 400; i < player.Center.X + 400; i += 100) {
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(i, player.Center.Y - 1000), Vector2.Zero, ModContent.ProjectileType<Ptersaur>(), 75, 0);
                        }
                    }
                    if (AITimer >= PROJECTILE_RAIN_TIME) {
                        AIState = Main.expertMode ? (int)AzhdarchoideaAIState.PROJECTILE_SWARM : (int)AzhdarchoideaAIState.FOLLOW;
                        AITimer = 0;
                    }
                    break;
                case (int)AzhdarchoideaAIState.PROJECTILE_SWARM:
                    MoveSide(player);
                    if (NPC.HasValidTarget && AITimer % 120 == 1) {
                        Vector2 target = player.Center;
                        for (int i = 0; i < 7; i++) {
                            Vector2 pos = target + Main.rand.NextVector2CircularEdge(700, 700);
                            Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), pos, pos.DirectionTo(target) * 5f, ProjectileID.AmberBolt, 75, 0);
                            proj.hostile = true;
                            proj.friendly = false;
                            proj.tileCollide = false;
                        }
                    }
                    if (AITimer >= PROJECTILE_SWARM_TIME) {
                        AIState = (int)AzhdarchoideaAIState.FOLLOW;
                        AITimer = 0;
                    }
                    break;
            }
        }

        const float MAX_Y_VEL = 12f;
        const float Y_VEL_ACCEL = 1.2f;
        const float MAX_X_VEL = 17f;
        const float X_VEL_ACCEL = 1.3f;

        private void MoveSide(Player player) {
            int amnt = (player.Center.X > NPC.Center.X) ? -128 : 128;

            if (player.Center.X + amnt > NPC.Center.X) {
                NPC.velocity.X = Math.Clamp(NPC.velocity.X + X_VEL_ACCEL, -MAX_X_VEL, MAX_X_VEL);
            } else {
                NPC.velocity.X = Math.Clamp(NPC.velocity.X - X_VEL_ACCEL, -MAX_X_VEL, MAX_X_VEL);
            }

            if (player.Center.Y - 48 > NPC.Center.Y) {
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y + Y_VEL_ACCEL, -MAX_Y_VEL, MAX_Y_VEL);
            } else {
                NPC.velocity.Y = Math.Clamp(NPC.velocity.Y - Y_VEL_ACCEL, -MAX_Y_VEL, MAX_Y_VEL);
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server) return;
            if (NPC.life <= 0) {
                var entitySource = NPC.GetSource_Death();
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            BodyTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaBody");
            BackgroundHindLegTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaBackgroundHindLeg");
            ForegroundHindLegTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaForegroundHindLeg");
            ForegroundUpperHindLegTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaForegroundUpperHindLeg");
            HeadTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaHead");
            WingBackgroundTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaWingBackground");
            WingForegroundTexture = ModContent.Request<Texture2D>("BuriedHeavens/Content/NPCs/Azhdarchoidea/AzhdarchoideaWingForeground");
            BodyRect.Width = BodyTexture.Width();
            BodyRect.Height = BodyTexture.Height();
            BackgroundHindLegRect.Width = BackgroundHindLegTexture.Width();
            BackgroundHindLegRect.Height = BackgroundHindLegTexture.Height();
            ForegroundHindLegRect.Width = ForegroundHindLegTexture.Width();
            ForegroundHindLegRect.Height = ForegroundHindLegTexture.Height();
            ForegroundUpperHindLegRect.Width = ForegroundUpperHindLegTexture.Width();
            ForegroundUpperHindLegRect.Height = ForegroundUpperHindLegTexture.Height();
            HeadRect.Width = HeadTexture.Width();
            HeadRect.Height = HeadTexture.Height();
            WingBackgroundRect.Width = WingBackgroundTexture.Width();
            WingBackgroundRect.Height = WingBackgroundTexture.Height();
            WingForegroundRect.Width = WingForegroundTexture.Width();
            WingForegroundRect.Height = WingForegroundTexture.Height();
            float wingRotation = MathF.Sin(MathF.PI * timer * 0.04f) * MathF.PI * 0.1f;

            SpriteEffects effects = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            WingBackgroundRect.X = (int)(NPC.Center.X + NPC.direction * WingBackgroundOffset.X - Main.screenPosition.X);
            WingBackgroundRect.Y = (int)(NPC.Center.Y + NPC.direction * WingBackgroundOffset.Y - Main.screenPosition.Y);
            BackgroundHindLegRect.X = (int)(NPC.Center.X + NPC.direction * BackgroundHindLegOffset.X - Main.screenPosition.X);
            BackgroundHindLegRect.Y = (int)(NPC.Center.Y + NPC.direction * BackgroundHindLegOffset.Y - Main.screenPosition.Y);
            BodyRect.X = (int)(NPC.Center.X + NPC.direction * BodyOffset.X - Main.screenPosition.X);
            BodyRect.Y = (int)(NPC.Center.Y + NPC.direction * BodyOffset.Y - Main.screenPosition.Y);
            HeadRect.X = (int)(NPC.Center.X + NPC.direction * HeadOffset.X - Main.screenPosition.X);
            HeadRect.Y = (int)(NPC.Center.Y + NPC.direction * HeadOffset.Y - Main.screenPosition.Y);
            ForegroundUpperHindLegRect.X = (int)(NPC.Center.X + NPC.direction * ForegroundUpperHindLegOffset.X - Main.screenPosition.X);
            ForegroundUpperHindLegRect.Y = (int)(NPC.Center.Y + NPC.direction * ForegroundUpperHindLegOffset.Y - Main.screenPosition.Y);
            ForegroundHindLegRect.X = (int)(ForegroundUpperHindLegRect.X + NPC.direction * ForegroundHindLegOffset.X);
            ForegroundHindLegRect.Y = (int)(ForegroundUpperHindLegRect.Y + NPC.direction * ForegroundHindLegOffset.Y);
            WingForegroundRect.X = (int)(NPC.Center.X + NPC.direction * WingForegroundOffset.X - Main.screenPosition.X);
            WingForegroundRect.Y = (int)(NPC.Center.Y + NPC.direction * WingForegroundOffset.Y - Main.screenPosition.Y);
            spriteBatch.Draw(WingBackgroundTexture.Value, WingBackgroundRect, null, Color.White, wingRotation, WingBackgroundRect.OriginFlip(WingBackgroundOrigin, effects), effects, 0);
            spriteBatch.Draw(BackgroundHindLegTexture.Value, BackgroundHindLegRect, null, Color.White, 0, BackgroundHindLegRect.OriginFlip(BackgroundHindLegOrigin, effects), effects, 0);
            spriteBatch.Draw(BodyTexture.Value, BodyRect, null, Color.White, 0, BodyRect.OriginFlip(BodyOrigin, effects), effects, 0);
            spriteBatch.Draw(HeadTexture.Value, HeadRect, null, Color.White, 0, HeadRect.OriginFlip(HeadOrigin, effects), effects, 0);
            spriteBatch.Draw(ForegroundUpperHindLegTexture.Value, ForegroundUpperHindLegRect, null, Color.White, 0, ForegroundUpperHindLegRect.OriginFlip(ForegroundUpperHindLegOrigin, effects), effects, 0);
            spriteBatch.Draw(ForegroundHindLegTexture.Value, ForegroundHindLegRect, null, Color.White, 0, ForegroundHindLegRect.OriginFlip(ForegroundHindLegOrigin, effects), effects, 0);
            spriteBatch.Draw(WingForegroundTexture.Value, WingForegroundRect, null, Color.White, -wingRotation, WingForegroundRect.OriginFlip(WingForegroundOrigin, effects), effects, 0);
            return false;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AzhdarchoideaTrophy>(), 10));

			LeadingConditionRule notExpertRule = new(new Conditions.NotExpert());

			// notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AzhdarchoideaMask>(), 7));
            
			npcLoot.Add(ItemDropRule.ExpertGetsRerolls(ModContent.ItemType<PrehestoricTooth>(), 5, 2));

			npcLoot.Add(notExpertRule);

			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AzhdarchoideaBag>()));

			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<AzhdarchoideaRelic>()));

			//npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<AzhdarchoideaPetItem>(), 4));
        }

        public override void OnKill() {
            TreeSystem tree = ModContent.GetInstance<TreeSystem>();
            if (!tree.KetherCheck()) {
                tree.pathway = [.. tree.pathway, 3];
            }
        }
    }
}