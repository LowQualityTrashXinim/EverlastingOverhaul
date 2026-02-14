using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
internal class Roguelike_OreBow : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.PlatinumBow || entity.type == ItemID.GoldBow;
	}
	public override void SetDefaults(Item entity) {
		entity.useTime = entity.useAnimation = 22;
		entity.damage += 40;
		entity.shootSpeed += 3;
		entity.crit += 6;
	}
	public override bool AltFunctionUse(Item item, Player player) {
		return true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, $"Roguelike_{item.Name}", ModUtils.LocalizationText("RoguelikeRework", "OreBow")));
	}
	public override void HoldItem(Item item, Player player) {
		if (WeaponEffect_ModPlayer.Check_ValidForIntroEffect(player) && player.Check_SwitchedWeapon(item.type)) {
			WeaponEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(15));
		}
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (WeaponEffect_ModPlayer.Check_IntroEffect(player, item.type)) {
			damage += 10;
		}
		if (player.GetModPlayer<Roguelike_PlatinumBow_ModPlayer>().Counter >= 60) {
			damage = (int)(damage * 1.5f) + 1;
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		var modplayer = player.GetModPlayer<Roguelike_PlatinumBow_ModPlayer>();
		int counter = modplayer.Counter;
		modplayer.Counter = 0;
		if (counter >= 180) {
			for (int i = 0; i < 3; i++) {
				var projectile = Projectile.NewProjectileDirect(source, position, velocity.Vector2DistributeEvenlyPlus(3, 40, i), type, damage, knockback, player.whoAmI);
				if (projectile.arrow) {
					projectile.extraUpdates += 1;
				}
			}
		}
		else {
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			if (projectile.arrow) {
				projectile.extraUpdates += 1;
			}
		}
		return false;
	}
}
public class Roguelike_PlatinumBow_ModPlayer : ModPlayer {
	public int Counter = 0;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++Counter >= 180) {
			Counter = 180;
		}
	}
}
