using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Projectiles
{
	internal class SandProjectile : ModProjectile {
		public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.SandBallGun);
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.SandBallGun);
			Projectile.aiStyle = -1;
		}
		public override void AI() {
			if (Main.rand.NextBool(10))
				Dust.NewDust(Projectile.Center, 10, 10, DustID.Sand);
			if (Projectile.ai[0] >= 50) {
				if (Projectile.velocity.Y < 16) {
					Projectile.velocity.Y += .25f;
				}
			}
			Projectile.ai[0]++;
		}
		public override void OnKill(int timeLeft) {
			for (int i = 0; i < 10; i++) {
				int dust = Dust.NewDust(Projectile.Center, 10, 10, DustID.Sand);
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(3, 3);
			}
		}
	}
    internal class SnowBlockProjectile : ModProjectile
    {
        public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.SnowBlock);
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.SnowBlock);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
            if (Projectile.velocity.Y < 20 && ++Projectile.ai[0] >= 30)
            {
                Projectile.velocity.Y += .25f;
            }
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.Length()) * (Projectile.velocity.X > 0 ? 1 : -1);
        }
        public override void OnKill(int timeLeft)
        {
            int amount = 3 + Main.rand.NextBool().ToInt();
            for (int i = 0; i < amount; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Main.rand.NextVector2CircularEdge(6, 6), ProjectileID.SnowBallFriendly, Projectile.damage / 4, Projectile.knockBack, Projectile.owner);
            }
        }
    }

}
