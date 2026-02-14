using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
public class Roguelike_TinBow : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.TinBow;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 26;
		entity.useTime = entity.useAnimation = 33;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_TinBow", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int counter = player.GetModPlayer<Roguelike_TinBow_ModPlayer>().Counter;
		player.GetModPlayer<Roguelike_TinBow_ModPlayer>().Counter = -player.itemAnimationMax;
		if (counter >= 120) {
			int amount = 3;
			bool randomizeYAxis = false;
			if (counter >= 240) {
				amount = 12;
				randomizeYAxis = true;
			}
			for (int i = 0; i < amount; i++) {
				var pos = position.Add(Main.rand.Next(-300, 300), 1000);
				if (randomizeYAxis) {
					pos.Y -= Main.rand.Next(0, 1000);
				}
				Projectile.NewProjectile(source, pos, (Main.MouseWorld + Main.rand.NextVector2Circular(50, 50) - pos).SafeNormalize(Vector2.Zero) * 2.5f, ModContent.ProjectileType<TinBolt>(), damage * 2, knockback, player.whoAmI);
				if (Main.rand.NextBool(3)) {
					pos = position.Add(Main.rand.Next(-300, 300), 1000);
					pos.Y -= Main.rand.Next(0, 200);
					Projectile.NewProjectile(source, pos, (Main.MouseWorld + Main.rand.NextVector2Circular(50, 50) - pos).SafeNormalize(Vector2.Zero) * 5, ModContent.ProjectileType<TinOreMeteor>(), (int)(damage * 2.5f), knockback, player.whoAmI);
				}
			}
		}
		var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
		Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero).Vector2RotateByRandom(15) * 5, ModContent.ProjectileType<TinOreMeteor>(), damage, knockback, player.whoAmI);
		proj.extraUpdates = 1;
		return false;
	}
}
public class Roguelike_TinBow_ModPlayer : ModPlayer {
	public int Counter = 0;
	public bool PeakShot = false;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++Counter >= 240) {
			Counter = 240;
		}
		else {
			PeakShot = false;
		}
		if (Player.HeldItem.type == ItemID.TinBow) {
			if (Counter == 120) {
				SemiPeakShotEffect();
			}
			if (Counter == 240 && !PeakShot) {
				PeakShot = true;
				PeakShotEffect();
			}
		}
	}
	private void SemiPeakShotEffect() {
		for (int i = 0; i < 120; i++) {
			var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(1, 5);
			dust.color = Color.White with { A = 0 };
			dust.scale += Main.rand.NextFloat(.1f, .2f);
		}
		for (int j = 0; j < 120; j++) {
			var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 1);
			dust.color = Color.White with { A = 0 };
			dust.scale += Main.rand.NextFloat(.1f, .2f);
		}
	}
	private void PeakShotEffect() {
		for (int i = 0; i < 120; i++) {
			var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(1.6f, 8).RotatedBy(MathHelper.PiOver4);
			dust.color = Color.Black;
			dust.scale += Main.rand.NextFloat(.1f, .2f);
		}
		for (int j = 0; j < 120; j++) {
			var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(8, 1.6f).RotatedBy(MathHelper.PiOver4);
			dust.color = Color.Black;
			dust.scale += Main.rand.NextFloat(.1f, .2f);
		}
		for (int i = 0; i < 120; i++) {
			var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(2, 10);
			dust.color = Color.White with { A = 0 };
			dust.scale += Main.rand.NextFloat(.1f, .2f);
		}
		for (int j = 0; j < 120; j++) {
			var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GemDiamond);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(10, 2);
			dust.color = Color.White with { A = 0 };
			dust.scale += Main.rand.NextFloat(.1f, .2f);
		}
	}
}
public class TinBolt : ModProjectile {
	public override string Texture => ModTexture.WHITEDOT;
	public Vector2 initialMousePos = Vector2.Zero;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 2;
		Projectile.scale = 1.5f;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 9999;
		Projectile.extraUpdates = 25;
		Projectile.light = 1;
	}
	public override void AI() {
		if (Projectile.timeLeft == 9999) {
			initialMousePos = Main.MouseWorld;
		}
		if (Projectile.Center.Y >= initialMousePos.Y) {
			Projectile.tileCollide = true;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor, .01f);
		return false;
	}
}
public class TinOreMeteor : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.TinOre);
	public Vector2 initialMousePos = Vector2.Zero;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 9999;
		Projectile.light = 1;
		Projectile.extraUpdates = 3;
	}
	public override void AI() {
		if (Projectile.timeLeft == 9999) {
			initialMousePos = Main.MouseWorld;
		}
		if (Projectile.Center.Y >= initialMousePos.Y) {
			Projectile.tileCollide = true;
		}
		Projectile.ai[1] = ModUtils.CountDown((int)Projectile.ai[1]);
		Projectile.rotation += MathHelper.ToRadians(20) * (Projectile.velocity.X > 0 ? 1 : -1);
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 25; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Tin);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2Circular(5, 5);
		}
	}
}
