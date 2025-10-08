using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using System.Collections.Generic;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
public class Roguelike_HeatRay : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.HeatRay) {
			entity.useTime = entity.useAnimation = 4;
			entity.mana = 4;
			entity.damage = 40;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.HeatRay) {
			ModUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_HeatRay", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
}
