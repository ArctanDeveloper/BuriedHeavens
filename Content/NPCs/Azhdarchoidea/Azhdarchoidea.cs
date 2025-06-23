using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.NPCs.Azhdarchoidea {
    public enum AzhdarchoideaAIState {
        FOLLOW,
        PROJECTILE_RAIN,
        DIVE_BOMB,
        PROJECTILE_SWARM
    }

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

        static Vector2 BodyOffset = new Vector2(0f, 0f);
        static Vector2 BackgroundHindLegOffset = new Vector2(0f, 0f);
        static Vector2 ForegroundHindLegOffset = new Vector2(0f, 0f);
        static Vector2 ForegroundUpperHindLegOffset = new Vector2(0f, 0f);
        static Vector2 HeadOffset = new Vector2(20, -12);
        static Vector2 WingBackgroundOffset = new Vector2(8, 0);
        static Vector2 WingForegroundOffset = new Vector2(-8, 0);

        static Vector2 BodyOrigin = new Vector2(0f, 0f);
        static Vector2 BackgroundHindLegOrigin = new Vector2(0f, 0f);
        static Vector2 ForegroundHindLegOrigin = new Vector2(0f, 0f);
        static Vector2 ForegroundUpperHindLegOrigin = new Vector2(0f, 0f);
        static Vector2 HeadOrigin = new Vector2(0f, 0f);
        static Vector2 WingBackgroundOrigin = new Vector2(0f, 0f);
        static Vector2 WingForegroundOrigin = new Vector2(0f, 0f);

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

        public override void SetStaticDefaults() {
        }

        public override void SetDefaults() {
            NPC.boss = true;
            NPC.damage = 100;
            NPC.defense = 25;
            NPC.friendly = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lifeMax = 12500;
            NPC.width = 50;
            NPC.height = 50;
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

        public override void AI() {
            timer++;
            AITimer++;

            // It's up to you, just code something. We don't got time.
            switch (AIState) {
                case (int)AzhdarchoideaAIState.FOLLOW:
                    break;
                case (int)AzhdarchoideaAIState.DIVE_BOMB:
                    break;
                case (int)AzhdarchoideaAIState.PROJECTILE_RAIN:
                    break;
                case (int)AzhdarchoideaAIState.PROJECTILE_SWARM:
                    break;
            }
        }

        private void DrawWings(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

        }

        private void DrawLegs(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

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
            WingBackgroundRect.X = (int)(NPC.Center.X + WingBackgroundOffset.X - Main.screenPosition.X);
            WingBackgroundRect.Y = (int)(NPC.Center.Y + WingBackgroundOffset.Y - Main.screenPosition.Y);
            BackgroundHindLegRect.X = (int)(NPC.Center.X + BackgroundHindLegOffset.X - Main.screenPosition.X);
            BackgroundHindLegRect.Y = (int)(NPC.Center.Y + BackgroundHindLegOffset.Y - Main.screenPosition.Y);
            BodyRect.X = (int)(NPC.Center.X + BodyOffset.X - Main.screenPosition.X);
            BodyRect.Y = (int)(NPC.Center.Y + BodyOffset.Y - Main.screenPosition.Y);
            HeadRect.X = (int)(NPC.Center.X + HeadOffset.X - Main.screenPosition.X);
            HeadRect.Y = (int)(NPC.Center.Y + HeadOffset.Y - Main.screenPosition.Y);
            ForegroundUpperHindLegRect.X = (int)(NPC.Center.X + ForegroundUpperHindLegOffset.X - Main.screenPosition.X);
            ForegroundUpperHindLegRect.Y = (int)(NPC.Center.Y + ForegroundUpperHindLegOffset.Y - Main.screenPosition.Y);
            ForegroundHindLegRect.X = (int)(NPC.Center.X + ForegroundHindLegOffset.X - Main.screenPosition.X);
            ForegroundHindLegRect.Y = (int)(NPC.Center.Y + ForegroundHindLegOffset.Y - Main.screenPosition.Y);
            WingForegroundRect.X = (int)(NPC.Center.X + WingForegroundOffset.X - Main.screenPosition.X);
            WingForegroundRect.Y = (int)(NPC.Center.Y + WingForegroundOffset.Y - Main.screenPosition.Y);
            spriteBatch.Draw(WingBackgroundTexture.Value, WingBackgroundRect, null, Color.White, wingRotation, WingBackgroundOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(BackgroundHindLegTexture.Value, BackgroundHindLegRect, null, Color.White, 0, BackgroundHindLegOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(BodyTexture.Value, BodyRect, null, Color.White, 0, BodyOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(HeadTexture.Value, HeadRect, null, Color.White, 0, HeadOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(ForegroundUpperHindLegTexture.Value, ForegroundUpperHindLegRect, null, Color.White, 0, ForegroundUpperHindLegOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(ForegroundHindLegTexture.Value, ForegroundHindLegRect, null, Color.White, 0, ForegroundHindLegOrigin, SpriteEffects.None, 0);
            spriteBatch.Draw(WingForegroundTexture.Value, WingForegroundRect, null, Color.White, -wingRotation, WingForegroundOrigin, SpriteEffects.None, 0);
            return false;
        }
    }
}