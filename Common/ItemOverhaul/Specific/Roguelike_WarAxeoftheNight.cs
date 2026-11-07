using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using EverlastingOverhaul.Common .ItemOverhaul.Common;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_WarAxeoftheNight : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.WarAxeoftheNight;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 66;
		entity.scale += .45f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
		if (target.HasBuff<NightInfection>()) {
			modifiers.SourceDamage += .5f;
			modifiers.ArmorPenetration += 30;
		}
	}
}
