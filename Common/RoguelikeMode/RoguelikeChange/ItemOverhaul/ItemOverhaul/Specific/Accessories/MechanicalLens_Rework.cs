using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific.Accessories;
internal class Roguelike_MechanicalLens : GlobalItem {
	public override void UpdateEquip(Item item, Player player) {
		if (item.type == ItemID.MechanicalLens) {
			var handler = player.ModPlayerStats();
			handler.AddStatsToPlayer(PlayerStats.CritChance, 1, 1.33f, 5);
			handler.UpdateCritDamage += .55f;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.MechanicalLens) {
			ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
}
