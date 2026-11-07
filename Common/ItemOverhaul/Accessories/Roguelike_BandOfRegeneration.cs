using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_BandOfRegeneration : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.BandofRegeneration;
	public override void UpdateEquip(Item item, Player player) {
		player.ModPlayerStats().UpdateHPMax.Base += 20;
		player.GetModPlayer<EverlastingOverhaul_BandOfRegeneration_ModPlayer>().BandEffect = true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
}
public class EverlastingOverhaul_BandOfRegeneration_ModPlayer : ModPlayer {
	public bool BandEffect = false;
	public int CounterEffect = 0;
	public int EffectCoolDown = 0;
	public override void ResetEffects() {
		BandEffect = false;
		EffectCoolDown = ModUtils.CountDown(EffectCoolDown);
	}
	public override void UpdateEquips() {
		if (CounterEffect > 0) {
			CounterEffect--;
			Player.ModPlayerStats().Rapid_LifeRegen += .25f;
			if (CounterEffect <= 0) {
				EffectCoolDown = ModUtils.ToSecond(10);
			}
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (BandEffect && EffectCoolDown < 1) {
			CounterEffect = 60;
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (BandEffect && EffectCoolDown < 1) {
			CounterEffect = 60;
		}
	}
}
