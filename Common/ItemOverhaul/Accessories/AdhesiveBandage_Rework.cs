using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_AdhesiveBandage : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.AdhesiveBandage;
	}
	public override void UpdateEquip(Item item, Player player) {
		player.ModPlayerStats().UpdateHPMax += .1f;
		player.GetModPlayer<Roguelike_AdhesiveBandage_ModPlayer>().AdhesiveBandage = true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_AdhesiveBandage_ModPlayer : ModPlayer {
	public bool AdhesiveBandage = false;
	int DamageBucket = 0;
	int HealOverTime = 0;
	public override void ResetEffects() {
		AdhesiveBandage = false;
		if (--HealOverTime <= 0) {
			if (DamageBucket <= 0) {
				HealOverTime = 0;
			}
			else {
				HealOverTime = 10;
				DamageBucket--;
				if (Player.statLife < Player.statLifeMax2) {
					Player.Heal(1);
				}
			}
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		AddIntoDamageBucket(hurtInfo.Damage);
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		AddIntoDamageBucket(hurtInfo.Damage);
	}
	private void AddIntoDamageBucket(int damage) {
		if (!AdhesiveBandage) {
			return;
		}
		DamageBucket = Math.Clamp(DamageBucket + damage, 0, 100);
	}
}
