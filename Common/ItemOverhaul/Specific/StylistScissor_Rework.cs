using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class Roguelike_StylistScissor : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.StylistKilLaKillScissorsIWish;
	}
	public override void SetDefaults(Item entity) {
		entity.noMelee = true;
		entity.noUseGraphic = true;
		entity.damage = 45;
		entity.crit = 10;
		entity.DamageType = DamageClass.Ranged;
		entity.shoot = ModContent.ProjectileType<Roguelike_StylistScissor_ModProjectile>();
		entity.shootSpeed = 6;
		entity.useAnimation = entity.useTime = 24;
	}
}
public class Roguelike_StylistScissor_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.StylistKilLaKillScissorsIWish);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = 5;
		Projectile.timeLeft = 6000;
		Projectile.tileCollide = true;
		Projectile.friendly = true;
		Projectile.extraUpdates = 5;
	}
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		if (++Projectile.ai[0] >= 20) {
			Projectile.velocity.Y += .01f;
			if (Projectile.velocity.Y > 16) {
				Projectile.velocity.Y = 16;
			}
		}
		Projectile.velocity.X *= 0.995f;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), target.Center, Main.rand.NextVector2CircularEdge(1, 1) * .1f,
			ModContent.ProjectileType<SimplePiercingProjectile2>(), Projectile.damage / 2 + 1, 3f, Projectile.owner, .1f, 0, 3);
	}
}
