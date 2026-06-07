using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_Blindfold : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Blindfold;
	}
	public override void UpdateEquip(Item item, Player player) {
		player.AddBuff(BuffID.Obstructed, 2);
		player.GetDamage(DamageClass.Generic) += .4f;
		player.ModPlayerStats().UpdateCritDamage += 1;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
