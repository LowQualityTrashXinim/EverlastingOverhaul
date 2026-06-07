using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_Bezoar : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Bezoar;
	}
	public override void UpdateEquip(Item item, Player player) {
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		modplayer.DebuffBuffTime -= .2f;
		modplayer.DebuffTime += .5f;
		modplayer.DebuffDamage += .12f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
