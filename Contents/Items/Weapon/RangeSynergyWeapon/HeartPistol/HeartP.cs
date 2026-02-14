using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using Terraria.DataStructures;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.HeartPistol {
	public class HeartP : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 22;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.light = 0.45f;
			Projectile.timeLeft = 175;
		}
		Vector2 startingVelocity = Vector2.Zero;
		public override void OnSpawn(IEntitySource source) {
			startingVelocity = Projectile.velocity;
		}
		public override void AI() {
			Projectile.rotation = startingVelocity.ToRotation() - MathHelper.PiOver2;
			if (++Projectile.ai[0] < 30) {
				Projectile.velocity = Vector2.Zero;
				return;
			}
			if (Projectile.ai[0] == 30) {
				for (int i = 0; i < 30; i++) {
					Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.WhiteTorch);
					dust.noGravity = true;
					dust.color = Color.Pink with { A = 0 };
					dust.velocity = Main.rand.NextVector2CircularEdge(5, 5);
					dust.scale = 1.5f;
				}
			}
			Projectile.velocity = startingVelocity;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<HeartPistol>(), ItemID.Vilethorn)) {
				target.AddBuff(BuffID.Venom, ModUtils.ToSecond(3));
			}
			if (target.lifeMax > 5 && !target.friendly && target.type != NPCID.TargetDummy) {
				Main.player[Projectile.owner].Heal(Main.rand.Next(1, 3));
			}
			if (Main.rand.NextBool(50) || SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<HeartPistol>(), ItemID.CandyCaneSword) && Main.rand.NextBool(20)) {
				Item.NewItem(Projectile.GetSource_OnHit(target), target.Hitbox, ItemID.Heart);
			}
		}
		public override void OnKill(int timeLeft) {
			int projectileType = ModContent.ProjectileType<smallerHeart>();
			int damage = (int)(Projectile.damage * 0.5f);
			float knockback = Projectile.knockBack;
			float Rotation = Projectile.rotation;
			Vector2 leftsideofheartshape1 = new Vector2(-5, 0);
			Vector2 leftsideofheartshape2 = new Vector2(-5, -2.5f);
			Vector2 leftsideofheartshape3 = new Vector2(-2.5f, -5);
			Vector2 leftsideofheartshape4 = new Vector2(-2.5f, 2.5f);
			Vector2 bottomheartshape = new Vector2(0, 5);
			Vector2 topheartshape = new Vector2(0, -2.5f);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, bottomheartshape.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, topheartshape.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
			for (int i = 0; i < 2; i++) {
				if (i == 1) {
					leftsideofheartshape1.X = -leftsideofheartshape1.X;
					leftsideofheartshape2.X = -leftsideofheartshape2.X;
					leftsideofheartshape3.X = -leftsideofheartshape3.X;
					leftsideofheartshape4.X = -leftsideofheartshape4.X;
				}
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftsideofheartshape1.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftsideofheartshape2.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftsideofheartshape3.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftsideofheartshape4.RotatedBy(Rotation), projectileType, damage, knockback, Projectile.owner);
			}
		}
	}
	internal class smallerHeart : ModProjectile {
		public override void SetDefaults() {
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.penetrate = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 40;
			Projectile.light = .25f;
		}
		public override void AI() {
			Dust dust = Dust.NewDustDirect(Projectile.position, 0, 0, DustID.WhiteTorch, newColor: new(255, 0, 100, 0));
			dust.noGravity = true;
			if (Projectile.ai[0] == 0) {
				Projectile.velocity -= Projectile.velocity * 0.1f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			if (Projectile.timeLeft < 10)
				Projectile.alpha += 25;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if (SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<HeartPistol>(), ItemID.Vilethorn)) {
				target.AddBuff(BuffID.Venom, ModUtils.ToSecond(3));
			}
			if (Main.rand.NextBool(150) || SynergyBonus_System.Check_SynergyBonus(ModContent.ItemType<HeartPistol>(), ItemID.CandyCaneSword) && Main.rand.NextBool(50)) {
				Item.NewItem(Projectile.GetSource_OnHit(target), target.Hitbox, ItemID.Heart);
			}
		}
	}
	public class HeartPistol_LifeCrystal : ModProjectile {
		public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.LifeCrystalBoulder);
		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.LifeCrystalBoulder);
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 30;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 240;
			Main.projFrames[Type] = 11;
		}
		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			if (++Projectile.frameCounter >= 4) {
				Projectile.frameCounter = 0;
				Projectile.frame = ModUtils.Safe_SwitchValue(Projectile.frame, 10);
			}
		}
		public override void OnKill(int timeLeft) {
			Item.NewItem(Projectile.GetSource_FromAI(), Projectile.Hitbox, ItemID.Heart);
			for (int i = 0; i < 20; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.LifeCrystal);
				dust.position += Main.rand.NextVector2Circular(Projectile.width, Projectile.height);
			}
		}
	}
}
