using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_MedicatedBandage : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.MedicatedBandage;
	public override void UpdateEquip(Item item, Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateHPMax += .15f;
		handler.UpdateHPRegen.Base += 3;
		handler.DebuffBuffTime -= .1f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
}
public class EverlastingOverhaul_MedicatedBandage_ModPlayer : ModPlayer {
	public bool bandage = false;
	public override void ResetEffects() {
		bandage = false;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (!bandage) {
			return;
		}
		Player.Heal(Main.rand.Next(5, 26));
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (!bandage) {
			return;
		}
		Player.Heal(Main.rand.Next(5, 26));
	}
}
