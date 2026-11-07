using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_EnchantedSword : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.EnchantedSword;
	public static readonly WeaponProgress progress = new() {

	};
	public override void SetStaticDefaults() {
		progress.Charge = true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, $"EverlastingOverhaulOverhaul_{item.Name}", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 93;
		entity.scale += .5f;
		entity.useTime = entity.useAnimation = 42;
		entity.shootSpeed = 15;
		entity.shootsEveryUse = true;
	}
	public override void HoldItem(Item item, Player player) {
		if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player)) {
			OutroEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(4));
		}
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.SetWeaponProgress(progress);
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.barProgress = player.GetModPlayer<EverlastingOverhaul_EnchantedSword_ModPlayer>().ChargeValue / 120f;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientA = Color.RoyalBlue;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientB = Color.PaleGoldenrod;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int count = Math.Clamp(player.GetModPlayer<EverlastingOverhaul_EnchantedSword_ModPlayer>().ChargeValue, 0, 120);

		Vector2 unit = velocity.SafeNormalize(Vector2.Zero);
		Projectile proj = Projectile.NewProjectileDirect(source, position + unit * Main.rand.NextFloat(90, 150), velocity.RotatedBy(MathHelper.PiOver2).Vector2RotateByRandom(30) * Main.rand.NextBool().ToDirectionInt(), ModContent.ProjectileType<EverlastingOverhaul_EnchantedSword_Slash_Projectile>(), (int)(damage * (1 + count / 60f)), knockback, player.whoAmI, 1, 20 + count / 20);
		if (proj.ModProjectile is EverlastingOverhaul_EnchantedSword_Slash_Projectile slash) {
			slash.ScaleX = 6 + count / 10 * .5f;
			slash.ScaleY = .5f + count / 10 * .15f;
			slash.ProjectileColor = Color.Lerp(Color.Blue, Color.Yellow, count / 120f);
		}
		if (OutroEffect_ModPlayer.Check_IntroEffect(player, item.type)) {
			switch (Main.rand.Next(1, 4)) {
				case 1:
					for (int i = 0; i < 8; i++) {
						Projectile.NewProjectile(source, position + unit.Vector2DistributeEvenlyPlus(8, 160, i) * 40, velocity, type, damage, knockback, player.whoAmI);
					}
					break;
				case 2:
					for (int i = 0; i < 12; i++) {
						Projectile.NewProjectile(source, position + unit.Vector2DistributeEvenlyPlus(12, 60, i) * 40, velocity.Vector2DistributeEvenlyPlus(12, 60, i), type, damage, knockback, player.whoAmI);
					}
					break;
				case 3:
					for (int i = 0; i < 16; i++) {
						Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(16, 320, i), type, damage, knockback, player.whoAmI);
					}
					break;
			}
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class EverlastingOverhaul_EnchantedSword_ModPlayer : ModPlayer {
	public int ChargeValue = 0;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++ChargeValue >= 120) {
			ChargeValue = 120;
		}
		if (Player.HeldItem.type == ItemID.EnchantedSword) {
			if (Player.ItemAnimationActive) {
				ChargeValue = -Player.itemAnimationMax;
			}
		}
	}
}
public class EverlastingOverhaul_EnchantedSword_Slash_Projectile : SimplePiercingProjectile2 {
	public override void OnKill(int timeLeft) {
		float amount = 1 + (InitialScaleXValue - 6) * .8f;
		for (int i = 0; i < amount; i++) {
			Vector2 position = Projectile.Center + Main.rand.NextVector2Circular(18 * InitialScaleXValue / 2, 18 * InitialScaleYValue).RotatedBy(Projectile.rotation);
			Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.Pi));
			for (int l = 0; l < 3; l++) {
				Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, vel.Vector2DistributeEvenly(3, 360, l), ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(Projectile.damage * .33f), Projectile.knockBack, Projectile.owner, 4, 3, i * 5);
				if (projectile.ModProjectile is SimplePiercingProjectile2 slash) {
					slash.ScaleX = 1;
					slash.ScaleY = .25f;
					slash.ProjectileColor = Main.rand.Next([Color.Blue, Color.Yellow]);
					slash.ExtraDelay = 5;
					slash.ExcessScaling = false;
				}
			}
		}
	}
}
