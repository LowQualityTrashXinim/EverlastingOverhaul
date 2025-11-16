using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Common;
internal class Roguelike_BroadSword : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return Check_Sword(entity.type);
	}
	public override void SetDefaults(Item entity) {
		if (Check_Sword(entity.type)) {
			entity.scale += .45f;
			entity.damage = 40;
			entity.ArmorPenetration = 20;
		}
	}
	public static bool Check_Sword(int type) {
		switch (type) {
			case ItemID.CopperBroadsword:
			case ItemID.TinBroadsword:
			case ItemID.IronBroadsword:
			case ItemID.LeadBroadsword:
			case ItemID.SilverBroadsword:
			case ItemID.TungstenBroadsword:
			case ItemID.GoldBroadsword:
			case ItemID.PlatinumBroadsword:
				return true;
		}
		return false;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
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
public class BroadSword_Guard : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class Roguelike_BroadSword_ModPlayer : ModPlayer {
	public int Counter = 0;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (Player.ItemAnimationActive && Player.ItemAnimationEndingOrEnded) {
			Counter = -Player.itemAnimationMax;
		}
		if (++Counter >= 61) {
			Counter = 61;
		}
		if (!Roguelike_BroadSword.Check_Sword(Player.HeldItem.type)) {
			return;
		}
		if (Counter == 60) {
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
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (!Roguelike_BroadSword.Check_Sword(Player.HeldItem.type)) {
			return;
		}
		if (Counter >= 60) {
			damage *= 3;
		}
	}
	public override void ModifyWeaponCrit(Item item, ref float crit) {
		if (!Roguelike_BroadSword.Check_Sword(Player.HeldItem.type)) {
			return;
		}
		if (Counter >= 60) {
			crit += 30;
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (!Roguelike_BroadSword.Check_Sword(Player.HeldItem.type)) {
			return;
		}
		if (Player.HasBuff<BroadSword_Guard>()) {
			modifiers.SourceDamage *= .05f;
			modifiers.Knockback *= .15f;
			Player.AddImmuneTime(-1, 44);
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (!Roguelike_BroadSword.Check_Sword(Player.HeldItem.type)) {
			return;
		}
		if (Player.HasBuff<BroadSword_Guard>()) {
			modifiers.SourceDamage *= .05f;
			modifiers.Knockback *= .15f;
			Player.AddImmuneTime(-1, 44);
		}
	}
}
