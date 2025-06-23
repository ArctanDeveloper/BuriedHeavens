using System;
using Terraria.ModLoader;

namespace BuriedHeavens.Content.Projectiles {
    public class Ptersaur : ModProjectile {
        public override void SetDefaults() {
            Projectile.width = 54;
            Projectile.height = 54;
            Projectile.damage = 55;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void AI() {
            Projectile.velocity.Y = MathF.Max(15f, Projectile.velocity.Y + 0.2f);
        } 
    }
}