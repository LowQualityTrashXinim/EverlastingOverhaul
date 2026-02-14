using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;

using EverlastingOverhaul.Texture;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Projectiles;
internal class HitScanBullet : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 10;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 4;
		Projectile.scale = 2;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		if (Projectile.timeLeft < 9) {
			return false;
		}
		return ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center, Projectile.Center.IgnoreTilePositionOFFSET(ToMouseDirection, 1000));
	}
	public override void AI() {
		if (Projectile.timeLeft == 10) {
			Vector2 toMouse = Projectile.velocity.SafeNormalize(Vector2.Zero);
			Projectile.velocity = Vector2.Zero;
			Player player = Main.player[Projectile.owner];
			Projectile.ai[0] = toMouse.X;
			Projectile.ai[1] = toMouse.Y;
			Projectile.Center = player.Center;
			Projectile.rotation = toMouse.ToRotation() - MathHelper.PiOver2;
		}
		Projectile.scale -= .2f;
	}
	Vector2 ToMouseDirection => new(Projectile.ai[0], Projectile.ai[1]);
	public override bool PreDraw(ref Color lightColor) {
		//Ain't the best way
		Vector2 drawpos = Projectile.Center.IgnoreTilePositionOFFSET(ToMouseDirection, 0) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
		Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawpos, null, Color.White, Projectile.rotation, Vector2.One * .5f, Projectile.scale, SpriteEffects.None);

		return false;
	}
}
internal class HitScanShotv2 : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.penetrate = 2;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 120;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 4;
		Projectile.stopsDealingDamageAfterPenetrateHits = true;
		Projectile.scale = 4;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		if (Projectile.timeLeft < 15) {
			return false;
		}
		if (!Collision.CanHitLine(Projectile.Center, 1, 1, targetHitbox.Center(), 1, 1)) {
			return false;
		}
		return ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center, Projectile.Center.IgnoreTilePositionOFFSET(ToMouseDirection, 3000));
	}
	Vector2 scaleVec = Vector2.One * 5;
	public override void AI() {
		if (Projectile.timeLeft == 120) {
			SoundStyle ExampleGunSoundStyle = new SoundStyle("Roguelike/Assets/SFX/HallowedGaze");
			SoundEngine.PlaySound(ExampleGunSoundStyle, Projectile.Center);
			scaleVec = Vector2.One * 10;
			scaleVec *= Projectile.scale;
			Vector2 toMouse = Projectile.velocity.SafeNormalize(Vector2.Zero);
			Projectile.velocity = Vector2.Zero;
			Projectile.ai[0] = toMouse.X;
			Projectile.ai[1] = toMouse.Y;
			Projectile.rotation = toMouse.ToRotation() - MathHelper.PiOver2;
			Projectile.ai[2] = 2515;
			//When projectile can stop
			scaleVec.Y = Projectile.ai[2] * .001f;
		}

		scaleVec.X *= .9f;
		if (scaleVec.X <= 0) {
			Projectile.Kill();
		}
	}
	public override bool? CanDamage() {
		return Projectile.penetrate > 1;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		float damage = Math.Clamp(target.life * .01f, 10, 2000);

		modifiers.SourceDamage.Base += damage;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.GetGlobalNPC<RoguelikeGlobalNPC>().HallowedGaze_Count = 0;
	}
	Vector2 ToMouseDirection => new(Projectile.ai[0], Projectile.ai[1]);
	public override bool PreDraw(ref Color lightColor) {
		//Ain't the best way
		Vector2 drawpos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) + Vector2.One * .5f;
		Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawpos, null, Color.White, Projectile.rotation, Vector2.One * .5f, scaleVec, SpriteEffects.None);
		return false;
	}
}
