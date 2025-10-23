using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_Chik : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Chik;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 42;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_Chik_GlobalProjectile : GlobalProjectile {
	public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) {
		return entity.type == ProjectileID.Chik;
	}
	public override bool PreAI(Projectile projectile) {
		if (Main.player[projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Roguelike_Chik_ModProjectile>()] < 1) {
			Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<Roguelike_Chik_ModProjectile>(), projectile.damage, 0, projectile.whoAmI);
		}
		return base.PreAI(projectile);
	}
	public override void PostAI(Projectile projectile) {
		Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center + Main.rand.NextVector2Circular(projectile.width, projectile.height), Main.rand.NextVector2CircularEdge(1, 1), ProjectileID.CrystalShard, projectile.damage / 3 + 1, 1, projectile.owner);
		proj.penetrate = -1;
		proj.maxPenetrate = -1;
		proj.usesIDStaticNPCImmunity = true;
		proj.idStaticNPCHitCooldown = 20;
	}
}
public class Roguelike_CHik_ModPlayer : ModPlayer {
	public int HitCounter = 0;
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.type != ProjectileID.Chik) {
			return;
		}
		if (++HitCounter >= 12) {
			float rotationRand = MathHelper.ToRadians(Main.rand.Next(90));
			for (int i = 0; i < 12; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(proj.GetSource_FromAI(), proj.Center, Vector2.One.RotatedBy(rotationRand).Vector2DistributeEvenlyPlus(12, 360, i) * 6, ProjectileID.CrystalShard, hit.Damage / 3 + 1, 1, proj.owner);
				projectile.penetrate = -1;
				projectile.maxPenetrate = -1;
				projectile.usesIDStaticNPCImmunity = true;
				projectile.idStaticNPCHitCooldown = 20;
			}
			if (HitCounter >= 15) {
				HitCounter = 0;
			}
		}
	}
}
public class Roguelike_Chik_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.Chik);
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 10;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.Chik);
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 2;
	}
	public int Owner { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public override void AI() {
		if (Owner < 0 || Owner >= Main.maxProjectiles) {
			Projectile.Kill();
			return;
		}
		Projectile parentProjectile = Main.projectile[Owner];
		if (!parentProjectile.active || parentProjectile.timeLeft < 0) {
			Projectile.Kill();
			return;
		}
		Projectile.timeLeft = 2;
		Vector2 positionNeedToBe = parentProjectile.Center - Projectile.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(++Projectile.ai[1] * 3)) * 50;
		Vector2 velocity = positionNeedToBe.SafeNormalize(Vector2.Zero);
		Projectile.velocity += velocity;
		Projectile.velocity = Projectile.velocity.LimitedVelocity(positionNeedToBe.Length() / 8f);
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor);
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		Vector2 drawPos = Projectile.Center + Main.screenPosition - origin;
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, 0, origin, 1f, SpriteEffects.None);
		return false;
	}
}
