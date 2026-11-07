using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_TrifoldMap : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.TrifoldMap;
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void UpdateEquip(Item item, Player player) {
		player.GetModPlayer<EverlastingOverhaul_TrifoldMap_ModPlayer>().Map = true;
	}
}
public class EverlastingOverhaul_TrifoldMap_ModPlayer : ModPlayer {
	public bool Map = false;
	public override void ResetEffects() {
		Map = false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Map) {
			target.AddBuff(BuffID.Confused, ModUtils.ToSecond(3));
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (Map && target.buffImmune[BuffID.Confused]) {
			modifiers.SourceDamage += .3f;
		}
	}
}
