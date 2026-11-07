using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Graphics;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_FrostBrand : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Frostbrand;
	public static readonly WeaponProgress progress = new() {

	};
	public override void SetStaticDefaults() {
		progress.Charge = true;
	}
	public override void SetDefaults(Item entity) {
		entity.shootsEveryUse = true;
		entity.scale += .5f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, $"EverlastingOverhaulOverhaul_{item.Name}", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void HoldItem(Item item, Player player) {
		if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player)) {
			OutroEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(7));
		}
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.SetWeaponProgress(progress);
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.barProgress = player.GetModPlayer<EverlastingOverhaul_FrostBrand_ModPlayer>().Counter / 120f;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientA = Color.Cyan;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientB = Color.AliceBlue;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		EverlastingOverhaul_FrostBrand_ModPlayer modplayer = player.GetModPlayer<EverlastingOverhaul_FrostBrand_ModPlayer>();
		modplayer.FrostBandBurst = ModUtils.Safe_SwitchValue(modplayer.FrostBandBurst, 3, 1);
		if (modplayer.FrostBandBurst >= 3) {
			for (int i = 0; i < 6; i++) {
				var vel = velocity.Vector2DistributeEvenlyPlus(6, 120, i);
				Projectile.NewProjectile(source, position.PositionOFFSET(vel, 50), vel, type, damage, knockback, player.whoAmI);
			}
		}
		Vector2 velUnit = velocity.SafeNormalize(Vector2.Zero);
		for (int i = 0; i < 3; i++) {
			var velocityToward = velocity.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).Vector2RotateByRandom(55);
			var Swordprojectile = Projectile.NewProjectileDirect(source, position + Main.rand.NextVector2Circular(50, 50) + velUnit * item.Size.Length(), velocityToward, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(damage * .85f), 2f, player.whoAmI, 2f + Main.rand.NextFloat(2));
			if (Swordprojectile.ModProjectile is SimplePiercingProjectile2 modproj) {
				modproj.ProjectileColor = SwordSlashTrail.averageColorByID[ItemID.Frostbrand] * 2;
				modproj.ScaleX = 9 + Main.rand.NextFloat();
			}
			Swordprojectile.usesIDStaticNPCImmunity = false;
			Swordprojectile.usesLocalNPCImmunity = true;
			Swordprojectile.localNPCHitCooldown = 60;
		}

		if (OutroEffect_ModPlayer.Check_IntroEffect(player, item.type)) {
			int amount = Main.rand.Next(1, 5);
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RandomSpread(Main.rand.NextFloat(1, 2), Main.rand.NextFloat(.91f, 1.1f)).Vector2RotateByRandom(10), type, damage, knockback, player.whoAmI);
			}
		}
		if (modplayer.Counter >= 120) {
			Vector2 rotate = velocity.RotatedBy(MathHelper.PiOver2);
			for (int i = 0; i < 10; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(source, position + velUnit * 25 * (1 + i), rotate.Vector2RotateByRandom(55) * Main.rand.NextBool().ToDirectionInt(), ModContent.ProjectileType<FrostBrand_Slash_Projectile>(), damage, knockback, player.whoAmI, .1f, 3, 5 + i);
				if (projectile.ModProjectile is FrostBrand_Slash_Projectile slash) {
					slash.ScaleX = 2 + i * 1.5f;
					slash.ScaleY = .5f + i * .25f;
					slash.ProjectileColor = Color.Cyan;
					slash.ExtraDelay = 10;
					slash.ExcessScaling = false;
				}
			}
			modplayer.Counter = -player.itemAnimationMax;
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class EverlastingOverhaul_FrostBrand_ModPlayer : ModPlayer {
	public int Counter = 0;
	public int FrostBandBurst = 0;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++Counter >= 120) {
			Counter = 120;
		}
		if (Player.HeldItem.type == ItemID.Frostbrand) {
			if (Player.ItemAnimationActive) {
				Counter = -Player.itemAnimationMax;
			}
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (Counter >= 120) {
			if (Player.HeldItem.type == ItemID.Frostbrand) {
				damage *= 4;
			}
		}
	}
}
/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// Ai2 : Delay before the slash appear
/// </summary>
public class FrostBrand_Slash_Projectile : SimplePiercingProjectile2 {
	public override void OnKill(int timeLeft) {
		int amount = Main.rand.Next(4, 9);
		for (int i = 0; i < amount; i++) {
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.75f, 1.2f), ProjectileID.FrostBoltSword, (int)(Projectile.damage * .65f), Projectile.knockBack, Projectile.owner);
			projectile.penetrate = 2;
			projectile.maxPenetrate = 2;
			projectile.tileCollide = true;
		}

		Vector2 vel = Main.rand.NextVector2CircularEdge(2, 2);
		Projectile project = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + Main.rand.NextVector2CircularEdge(50, 50) * Main.rand.NextFloat(.8f, 1.5f), vel, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(Projectile.damage * .56f), Projectile.knockBack, Projectile.owner, 2f, 25, 5);
		if (project.ModProjectile is SimplePiercingProjectile2 slash) {
			slash.ScaleX = Main.rand.NextFloat(26, 26.5f);
			slash.ScaleY = 1f;
			slash.ProjectileColor = Color.Cyan;
		}

	}
}
