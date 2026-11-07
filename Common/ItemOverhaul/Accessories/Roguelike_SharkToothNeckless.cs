using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_SharkToothNeckless : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.SharkToothNecklace;
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
		player.GetDamage<GenericDamageClass>() += .1f;
		player.ModPlayerStats().UpdateCritDamage += .2f;
		player.GetModPlayer<EverlastingOverhaul_SharkToothNeckless_ModPlayer>().SharkTooth = true;
	}
}
public class EverlastingOverhaul_SharkToothNeckless_ModPlayer : ModPlayer {
	public int CoolDown = 0;
	public bool SharkTooth = false;
	public override void ResetEffects() {
		SharkTooth = false;
		CoolDown = ModUtils.CountDown(CoolDown);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (CoolDown > 0) {
			return;
		}
		if (SharkTooth && Main.rand.NextBool(4)) {
			Player.StrikeNPCDirect(target, hit);
			CoolDown = 60;
		}
	}
}
