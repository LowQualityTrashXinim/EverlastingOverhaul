using EverlastingOverhaul.Common.Systems.ReviveSystem;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
public class EverlastingOverhaul_LuckyHorseshoe : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.LuckyHorseshoe;
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new TooltipLine(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void UpdateEquip(Item item, Player player) {
		player.GetModPlayer<EverlastingOverhaul_LuckyHorseshoe_ModPlayer>().LuckyHorseshoe = true;
	}
}
public class EverlastingOverhaul_LuckyHorseshoe_ModPlayer : ModPlayer {
	public bool LuckyHorseshoe = false;
	public override void ResetEffects() {
		LuckyHorseshoe = false;
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (LuckyHorseshoe) {
			if (Main.rand.NextBool()) {
				modifiers.SourceDamage *= .05f;
			}
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (LuckyHorseshoe) {
			if (Main.rand.NextBool()) {
				modifiers.SourceDamage *= .05f;
			}
		}
	}
}
public class EverlastingOverhaul_LuckyHorseshoe_Revive : Revive {
	public override void SetStaticDefaults() {
		ReviveType = ReviveType.Chance;
		Chance = .1f;
	}
	public override bool IsActive(Player player) {
		return player.GetModPlayer<EverlastingOverhaul_LuckyHorseshoe_ModPlayer>().LuckyHorseshoe;
	}
	public override void OnRevive(Player player) {
		player.Heal(100);
		player.immune = true;
		player.AddImmuneTime(-1, 90);
	}
}
