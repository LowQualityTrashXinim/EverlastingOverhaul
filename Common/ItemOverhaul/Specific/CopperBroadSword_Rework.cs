using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.ItemOverhaul;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.ItemOverhaul.Common;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class CopperBroadSword_Rework : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.CopperBroadsword;
	}
	public static readonly WeaponProgress progress = new() {

	};
	public override void SetStaticDefaults() {
		progress.Charge = true;
	}
	public override void SetDefaults(Item entity) {
		entity.scale += .45f;
		entity.damage = 40;
		entity.ArmorPenetration = 20;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new TooltipLine(Mod, "", ModUtils.LocalizationText("RoguelikeRework", "CopperBroadSword")));
		ModUtils.AddTooltip(ref tooltips, new TooltipLine(Mod, "", ModUtils.LocalizationText("RoguelikeRework", "BroadSword")));
	}
	public override bool AltFunctionUse(Item item, Player player) => true;
	public override float UseSpeedMultiplier(Item item, Player player) {
		if (player.altFunctionUse == 2) {
			return 1f;
		}
		return base.UseSpeedMultiplier(item, player);
	}
	public override void HoldItem(Item item, Player player) {
		if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player)) {
			OutroEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(9));
		}
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.SetWeaponProgress(progress);
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.barProgress = player.GetModPlayer<Roguelike_CopperBroadSword_ModPlayer>().Counter / 60f;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientA = Color.Orange;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientB = Color.Yellow;
		if (player.altFunctionUse == 2) {
			if (player.ItemAnimationJustStarted) {
				player.AddBuff<BroadSword_Guard>(player.itemAnimationMax);
				player.velocity.X *= .1f;
			}
		}
		item.GetGlobalItem<MeleeWeaponOverhaul>().DisableAttackAnimation = player.altFunctionUse == 2;
		item.GetGlobalItem<MeleeWeaponOverhaul>().HideSwingVisual = player.altFunctionUse == 2;
	}
	public override void UseStyle(Item item, Player player, Rectangle heldItemFrame) {
		if (OutroEffect_ModPlayer.Check_IntroEffect(player, item.type)) {
			if (Main.rand.NextBool(5)) {
				Vector2 vel = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 3;
				Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), player.Center, vel, ModContent.ProjectileType<ShortCircusDischarge>(), player.GetWeaponDamage(item), 0, player.whoAmI);
			}
		}
		if (player.altFunctionUse == 2) {
			player.itemLocation = player.Center;
			if (player.direction == 1) {
				player.itemRotation = -MathHelper.PiOver4;
				player.itemLocation += Vector2.UnitX * 10;
			}
			else {
				player.itemRotation = MathHelper.PiOver4;
				player.itemLocation += Vector2.UnitX * -10;
			}
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, player.itemRotation - MathHelper.PiOver2 * player.direction);
		}
		item.GetGlobalItem<MeleeWeaponOverhaul>().DisableAttackAnimation = player.altFunctionUse == 2;
	}
	public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox) {
		if (player.altFunctionUse == 2) {
			noHitbox = true;
		}
	}
	public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target) {
		if (player.altFunctionUse == 2) {
			return false;
		}
		return base.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target);
	}
}
public class Roguelike_CopperBroadSword_ModPlayer : ModPlayer {
	public int Counter = 0;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++Counter >= 60) {
			Counter = 60;
		}
		if (Player.HeldItem.type != ItemID.CopperBroadsword) {
			return;
		}
		if (Player.ItemAnimationActive && Player.ItemAnimationEndingOrEnded && Player.altFunctionUse != 2) {
			Counter = -Player.itemAnimationMax;
		}
		if (Counter == 59) {
			SpawnSpecialDustEffect();
		}
	}
	public void SpawnSpecialDustEffect() {
		SoundEngine.PlaySound(SoundID.Item71 with { Pitch = .5f }, Player.Center);
		for (int o = 0; o < 10; o++) {
			for (int i = 0; i < 4; i++) {
				var Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * (3 + Main.rand.NextFloat()) * 5;
				for (int l = 0; l < 8; l++) {
					float multiplier = Main.rand.NextFloat();
					float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
					int dust = Dust.NewDust(Player.Center.Add(0, -60), 0, 0, DustID.GemDiamond, 0, 0, 0, Color.White, scale);
					Main.dust[dust].velocity = Toward * multiplier;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].Dust_GetDust().FollowEntity = true;
					Main.dust[dust].Dust_BelongTo(Player);
				}
			}
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Player.HeldItem.type != ItemID.CopperBroadsword) {
			return;
		}
		hit.Damage = Math.Clamp(hit.Damage / 3, 1, int.MaxValue);
		if (Main.rand.NextBool(5)) {
			Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(item), target.Center, Main.rand.NextVector2CircularEdge(5, 5), ProjectileID.ThunderSpearShot, hit.Damage, 0, Player.whoAmI);
			proj.penetrate = 10;
			proj.maxPenetrate = 10;
		}
		if (Counter >= 60) {
			for (int i = 0; i < 5; i++) {
				Projectile proj = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(item), target.Center, Main.rand.NextVector2CircularEdge(5, 5), ProjectileID.ThunderSpearShot, hit.Damage, 0, Player.whoAmI);
				proj.penetrate = 10;
				proj.maxPenetrate = 10;
			}
		}
	}
	public override void OnHitAnything(float x, float y, Entity victim) {
		if (Player.HeldItem.type != ItemID.CopperBroadsword) {
			return;
		}
		if (Player.HasBuff<BroadSword_Guard>()) {
			Counter = 59;
		}
	}
}
public class ShortCircusDischarge : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.friendly = true;
		Projectile.hide = true;
		Projectile.extraUpdates = 100;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 60;
	}
	public override void AI() {
		int dust = Dust.NewDust(Projectile.Center, 0, 0, Main.rand.Next(new int[] { DustID.Electric, DustID.GemSapphire }));
		Main.dust[dust].scale = Main.rand.NextFloat(.3f, .75f);
		Main.dust[dust].velocity = Vector2.Zero;
		if (Projectile.timeLeft % 5 == 0) {
			Projectile.velocity = Projectile.velocity.Vector2RotateByRandom(20);
			Projectile.damage += 5;
		}
	}
}
