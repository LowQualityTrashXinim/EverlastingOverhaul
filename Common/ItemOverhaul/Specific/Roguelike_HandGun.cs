using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_HandGun : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Handgun;
	}
	public override void SetStaticDefaults() {
		progress.Charge = true;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 50;
		entity.crit = 30;
		entity.useTime = entity.useAnimation = 20;
	}
	public static readonly WeaponProgress progress = new() {

	};
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, $"EverlastingOverhaul_{item.Name}", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void HoldItem(Item item, Player player) {
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.SetWeaponProgress(progress);
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.barProgress = player.GetModPlayer<EverlastingOverhaul_HandGun_ModPlayer>().HandGun_Counter / 60f;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientA = Color.Gray;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientB = Color.White;
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		EverlastingOverhaul_HandGun_ModPlayer modplayer = player.GetModPlayer<EverlastingOverhaul_HandGun_ModPlayer>();
		if (modplayer.HandGun_Counter >= 60) {
			if (type == ProjectileID.Bullet) {
				type = ProjectileID.BulletHighVelocity;
			}
			damage += 20;
		}
		modplayer.HandGun_Counter = -player.itemAnimationMax;
	}
}
public class EverlastingOverhaul_HandGun_ModPlayer : ModPlayer {
	public int HandGun_Counter = 0;
	public bool PerfectStrike = false;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++HandGun_Counter >= 60) {
			HandGun_Counter = 60;
		}
	}
}
