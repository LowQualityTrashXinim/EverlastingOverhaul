using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_Minishark : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Minishark;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 19;
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 50);
	}
	public override bool AltFunctionUse(Item item, Player player) => true;
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int counter = ++player.GetModPlayer<EverlastingOverhaul_Minishark_ModPlayer>().FreezyCounter;
		if (counter >= 200 && player.altFunctionUse == 2) {
			player.GetModPlayer<EverlastingOverhaul_Minishark_ModPlayer>().FreezyCounter = 0;
			ModUtils.CombatTextRevamp(player.Hitbox, Color.Aquamarine, "Freezy!!");
			player.AddBuff<Freezy>(ModUtils.ToSecond(5));
			return false;
		}
		if (Main.rand.NextBool(6)) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10) * Main.rand.NextFloat(.9f, 1), type, damage, knockback, player.whoAmI);
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "EverlastingOverhaul_Minishark", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
}
public class Freezy : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		if (player.HeldItem.type == ItemID.Minishark) {
			player.ModPlayerStats().AttackSpeed += .5f;
		}
		else {
			player.ModPlayerStats().AttackSpeed += .15f;
		}
	}
}
public class EverlastingOverhaul_Minishark_ModPlayer : ModPlayer {
	public int FreezyCounter = 0;
	public float Recoil = 0;
	public int Recoil_CoolDown = 0;
	public override void ResetEffects() {
		if (Recoil_CoolDown <= 0 && Recoil > 0) {
			Recoil--;
			if (Recoil < 0) {
				Recoil = 0;
			}
			Recoil_CoolDown = 10;
		}
		else {
			Recoil_CoolDown = ModUtils.CountDown(Recoil_CoolDown);
		}
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (item.type == ItemID.Minishark) {
			if (Recoil > 0) {
				velocity = velocity.Vector2RotateByRandom(Recoil);
				velocity = velocity.Vector2RandomSpread(Math.Clamp(Recoil, 0, 3));
			}
			Recoil += .05f;
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Player.HasBuff<Freezy>()) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(15) * Main.rand.NextFloat(.8f, 1), type, damage, knockback, Player.whoAmI);
			if (Main.rand.NextBool(3)) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(20) * Main.rand.NextFloat(.7f, 1), type, damage, knockback, Player.whoAmI);
			}
			if (Main.rand.NextBool(6)) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.7f, 1), type, damage, knockback, Player.whoAmI);
			}
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
