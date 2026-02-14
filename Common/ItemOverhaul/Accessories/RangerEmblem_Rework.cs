using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;
using System.Collections.Generic;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_RangerEmblem : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.RangerEmblem;
	}
	public override void UpdateEquip(Item item, Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Range_CritDamage += .3f;
		player.GetCritChance(DamageClass.Ranged) += 5;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
