using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_FeralCraws : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.FeralClaws;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void UpdateEquip(Item item, Player player) {
		player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
		player.GetAttackSpeed(DamageClass.Generic) += MathHelper.Lerp(0.15f, 0f, (player.statLife - 200) / 200f);
	}
}
