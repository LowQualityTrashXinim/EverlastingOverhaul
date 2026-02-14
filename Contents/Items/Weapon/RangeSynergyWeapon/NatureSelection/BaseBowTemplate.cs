
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.NatureSelection {
	abstract class BaseBowTemplate : ModProjectile {
		public override void SetDefaults() {
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.timeLeft = 70;
			Projectile.width = 16;
			Projectile.height = 32;
		}
		public override void AI() {
			Vector2 safeAim = (Main.MouseWorld - Projectile.position).SafeNormalize(Vector2.UnitX);
			if (++Projectile.ai[1] >= 10 && Projectile.ai[1] <= 20 && Projectile.ai[1] % 4 == 0) {
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, safeAim * 15, (int)Projectile.ai[2], Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] == 30) {
				Projectile.velocity = safeAim * 15f;
			}
			if (Projectile.ai[0] > 30) {
				Projectile.rotation += 0.5f;
			}
			else {
				Projectile.rotation = safeAim.ToRotation();
			}
		}
		public override void OnKill(int timeLeft) {
			float numProj = 5 + Main.rand.Next(3);
			for (int i = 0; i < numProj; i++) {
				Vector2 Star = Projectile.velocity.Vector2DistributeEvenly(numProj, 360, i);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Star, (int)Projectile.ai[2], (int)(Projectile.damage * 0.5f), Projectile.knockBack * .5f, Projectile.owner);
			}
		}
	}
}
