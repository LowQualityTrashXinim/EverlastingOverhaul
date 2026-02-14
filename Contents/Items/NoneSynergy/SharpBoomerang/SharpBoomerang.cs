using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.NoneSynergy.SharpBoomerang;
internal class SharpBoomerang : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(38, 72, 13, 5f, 15, 15, ItemUseStyleID.Swing, ModContent.ProjectileType<SharpBoomerangP>(), 40, false);
		Item.crit = 6;
		Item.scale = 0.5f;
		Item.noUseGraphic = true;
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.buyPrice(platinum: 5);
	}
	public override bool CanUseItem(Player player) {
		return player.ownedProjectileCounts[ModContent.ProjectileType<SharpBoomerangP>()] < 1;
	}
}
internal class SharpBoomerangP : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<SharpBoomerang>();
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 72;
		Projectile.scale = .5f;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 999;
		DrawOriginOffsetX = 5;
		DrawOriginOffsetY = -10;
		Projectile.usesLocalNPCImmunity = true;
	}
	float MaxLengthX = 0;
	float MaxLengthY = 0;

	int MouseXPosDirection;
	int maxProgress = 25;
	int progression = 0;
	public override void AI() {
		var player = Main.player[Projectile.owner];
		if (Projectile.timeLeft == 999) {
			if (Projectile.ai[0] == 0)
				Projectile.ai[0] = Main.rand.NextBool().ToDirectionInt();
			MaxLengthX = (Main.MouseWorld - player.Center).Length();
			MaxLengthX = Math.Clamp(MaxLengthX, 300, 500);
			maxProgress += (int)(MaxLengthX * .05f);
			progression = maxProgress;
			MouseXPosDirection = (int)Projectile.ai[0] * (Main.MouseWorld.X - player.Center.X > 0 ? 1 : -1);
			MaxLengthY = -(MaxLengthX + Main.rand.NextFloat(-10, 80)) * .25f * MouseXPosDirection;
		}
		if (player.dead || !player.active || progression <= 0) {
			Projectile.Kill();
		}
		int halfmaxProgress = (int)(maxProgress * .5f);
		int quadmaxProgress = (int)(maxProgress * .25f);
		float progress;
		if (progression > halfmaxProgress) {
			progress = (maxProgress - progression) / (float)halfmaxProgress;
		}
		else {
			progress = progression / (float)halfmaxProgress;
		}
		float X = MathHelper.SmoothStep(-30, MaxLengthX, progress);
		ProgressYHandle(progression, halfmaxProgress, quadmaxProgress, out float Y);
		var VelocityPosition = new Vector2(X, Y).RotatedBy(Projectile.velocity.ToRotation());
		Projectile.Center = player.Center + VelocityPosition;
		progression--;
		Projectile.rotation += MathHelper.ToRadians(55);
	}
	private void ProgressYHandle(int timeleft, float progressMaxHalf, float progressMaxQuad, out float Y) {
		if (timeleft > progressMaxHalf + progressMaxQuad) {
			float progressY = 1 - (timeleft - (progressMaxHalf + progressMaxQuad)) / progressMaxQuad;
			Y = MathHelper.SmoothStep(0, MaxLengthY, progressY);
			return;
		}
		if (timeleft > progressMaxQuad) {
			float progressY = 1 - (timeleft - progressMaxQuad) / progressMaxHalf;
			Y = MathHelper.SmoothStep(MaxLengthY, -MaxLengthY, progressY);
			return;
		}
		else {
			float progressY = 1 - timeleft / progressMaxQuad;
			Y = MathHelper.SmoothStep(-MaxLengthY, 0, progressY);
			return;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		var RandomPos = Projectile.Center + Main.rand.NextVector2CircularEdge(50, 50);
		var DistanceToAim = (target.Center - RandomPos).SafeNormalize(Vector2.UnitX) * 4f;
		int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), RandomPos, DistanceToAim, ProjectileID.SuperStarSlash, Projectile.damage, 0, Projectile.owner);
		Main.projectile[proj].usesIDStaticNPCImmunity = true;
		target.immune[Projectile.owner] = 3;
	}
}
