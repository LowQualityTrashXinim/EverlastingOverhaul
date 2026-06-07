using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_ArmorPolish : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.ArmorPolish;
	}
	public override void UpdateEquip(Item item, Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		int counter = 0;
		if (player.armor[0].type != ItemID.None) {
			counter++;
			handler.UpdateDefenseBase.Base += 10;
		}
		if (player.armor[1].type != ItemID.None) {
			counter++;
			handler.UpdateDefenseBase.Base += 10;
		}
		if (player.armor[2].type != ItemID.None) {
			counter++;
			handler.UpdateDefenseBase.Base += 10;
		}
		if (counter >= 3) {
			handler.UpdateDefenseBase += .05f;
			player.endurance += .05f;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
