using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific.Accessories;
internal class Roguelike_ClimbingClaws : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.ClimbingClaws;
	}
	public override void UpdateEquip(Item item, Player player) {
		player.GetDamage<MeleeDamageClass>() += .12f;
		player.GetCritChance<MeleeDamageClass>() += 10;
		var handler = player.ModPlayerStats();
		handler.Melee_CritDamage += .25f;
		if (player.sliding) {
			handler.UpdateDefenseBase.Base += 10;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
