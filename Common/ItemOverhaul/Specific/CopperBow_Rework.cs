using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
public class Roguelike_CopperBow : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.CopperBow;
	}
	public override void SetDefaults(Item entity) {
		entity.damage += 15;
		entity.useTime = entity.useAnimation = 23;
		entity.shootSpeed = 15;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int Counter = player.GetModPlayer<Roguelike_CopperBow_ModPlayer>().CopperBow_Counter;
		Projectile projectile;
		if (Counter >= 90) {
			int amount = 8;
			if (Counter == 150) {
				amount += 15;
			}
			for (int i = 0; i < amount; i++) {
				projectile = Projectile.NewProjectileDirect(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.7f, 1f), ProjectileID.ThunderSpearShot, (int)(damage * 1.25f), knockback, player.whoAmI);
				projectile.DamageType = DamageClass.Ranged;
				projectile.extraUpdates = 2;
				projectile.alpha -= 1020;
			}
		}
		projectile = Projectile.NewProjectileDirect(source, position, velocity, ProjectileID.ThunderSpearShot, (int)(damage * 1.25f), knockback, player.whoAmI);
		projectile.DamageType = DamageClass.Ranged;
		projectile.extraUpdates = 2;
		projectile.alpha -= 1020;
		player.GetModPlayer<Roguelike_CopperBow_ModPlayer>().CopperBow_Counter = -player.itemAnimationMax;
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_CoperBow", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_CopperBow_ModPlayer : ModPlayer {
	public int CopperBow_Counter = 0;
	public bool PeakShot = false;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++CopperBow_Counter > 150) {
			CopperBow_Counter = 150;
		}
		else {
			PeakShot = false;
		}
		if (Player.HeldItem.type != ItemID.CopperBow) {
			return;
		}
		if (CopperBow_Counter == 90) {
			for (int i = 0; i < 60; i++) {
				var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.Electric);
				dust.noGravity = true;
				dust.velocity = -Vector2.UnitY.Vector2RotateByRandom(10) * Main.rand.NextFloat(1, 10);
				dust.scale += Main.rand.NextFloat(.5f);
			}
		}
		if (!PeakShot && CopperBow_Counter == 150) {
			SoundEngine.PlaySound(SoundID.Thunder);
			PeakEffect();
			PeakShot = true;
		}
	}
	private void PeakEffect() {
		var lightningPreSetPath = new Vector2[10];
		var velocity = -Vector2.UnitY * 10;
		var path = Player.Center;
		for (int i = 0; i < lightningPreSetPath.Length; i++) {
			lightningPreSetPath[i] = path;
			velocity = velocity.Vector2RotateByRandom(Main.rand.NextFloat(60, 80) * Main.rand.NextBool().ToDirectionInt());
			if (velocity.Y >= 0) {
				velocity.Y -= 10f;
			}
			path = path.PositionOFFSET(velocity, Main.rand.NextFloat(75, 150));
			float length = Vector2.Distance(lightningPreSetPath[i], path);
			for (int l = 0; l < 50; l++) {
				int dust = Dust.NewDust(lightningPreSetPath[i].PositionOFFSET(velocity, Main.rand.NextFloat(length)), 0, 0, DustID.Electric);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(.5f, .75f);
				Main.dust[dust].fadeIn = .1f;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(1, 1);
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(ItemID.CopperBow) && proj.type != ProjectileID.Electrosphere) {
			if (Main.rand.NextFloat() <= .15f) {
				int min = Math.Max(proj.damage / 4, 1);
				var projectile = Projectile.NewProjectileDirect(proj.GetSource_FromAI(), proj.Center, Vector2.Zero, ProjectileID.Electrosphere, min, proj.knockBack, proj.owner);
				projectile.timeLeft = 30;
			}
		}
	}
}
