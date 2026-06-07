using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_FastClock : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.FastClock;
	}
	public override void UpdateEquip(Item item, Player player) {
		//player.ModPlayerStats().EnchantmentCoolDown -= .5f;
		player.ModPlayerStats().AttackSpeed += .15f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
