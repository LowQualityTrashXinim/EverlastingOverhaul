using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.Items.Consumable.Ammo;
internal class ArcaneRound : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.Item_DefaultToAmmo(32, 32, 7, 0, 1.9f, 22, ModContent.ProjectileType<MagicBullet>(), AmmoID.Bullet);
		Item.DamageType = DamageClass.Ranged;
		Item.rare = ItemRarityID.Green;
		Item.value = 10;
	}
}
class MagicMuzzlePlayer : ModPlayer {
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (type == ModContent.ProjectileType<MagicBullet>()) {
			if (!Player.CheckMana(8, true)) {
				damage /= 2;
			}
		}
	}
}
class MagicBullet : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.DamageType = DamageClass.Magic;
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.timeLeft = 6000;
		Projectile.extraUpdates = 20;
		Projectile.penetrate = 2;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 3;
	}
	public override Color? GetAlpha(Color lightColor) {
		return new Color(0, 0, 255);
	}
	public override bool? CanDamage() {
		return Projectile.penetrate > 1;
	}
	public override void AI() {
		if (Projectile.timeLeft == 6000) {
			if (Projectile.ai[0] == 0) {
				Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(DamageClass.Magic).ApplyTo(Projectile.damage);
			}
		}
		Projectile.velocity = Projectile.velocity.LimitedVelocity(1f);
		if (Projectile.timeLeft <= 300) {
			Projectile.ProjectileAlphaDecay(300);
		}
		if (Projectile.penetrate <= 1) {
			if (Projectile.ai[1] != 1) {
				Projectile.timeLeft = 300;
				Projectile.ai[1] = 1;
			}
			Projectile.velocity = Projectile.velocity * .99f;
			return;
		}
	}
	public void DrawTrail1(Texture2D texture, Vector2 origin) {
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			var drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			var color = new Color(0, 0, 40, 1) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color * Projectile.Opacity, Projectile.oldRot[k], origin, Projectile.scale - k / 100f, SpriteEffects.None, 0);
		}
	}
	public void DrawTrail2(Texture2D texture, Vector2 origin) {
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			var drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			var color = new Color(25, 25, 25, 1) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			Main.EntitySpriteDraw(texture, drawPos, null, color * Projectile.Opacity, Projectile.oldRot[k], origin, (Projectile.scale - k / 100f) * .5f, SpriteEffects.None, 0);
		}
	}
	public override void OnKill(int timeLeft) {
		if (Projectile.ai[2] == 0 && !Projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_FromDeathScatterShot)
			if (Projectile.Center.LookForHostileNPC(out NPC npc, 250f)) {
				var vel = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 5f;
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vel, Type, (int)(Projectile.damage * .5f), Projectile.knockBack, Projectile.owner, 1, 0, 1);
				Main.projectile[proj].timeLeft = 3000;
			}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		var texture = ModContent.Request<Texture2D>(ModTexture.SMALLWHITEBALL).Value;
		var origin = Projectile.Size * .5f;
		DrawTrail1(texture, origin);
		DrawTrail2(texture, origin);
		return false;
	}
}
