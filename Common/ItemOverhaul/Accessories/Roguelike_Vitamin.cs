using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_Vitamin : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Vitamins;
	}
	public override void UpdateEquip(Item item, Player player) {
		EverlastingOverhaul_Vitamin_ModPlayer vitaPlayer = player.GetModPlayer<EverlastingOverhaul_Vitamin_ModPlayer>();
		vitaPlayer.Vitamin = true;
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		modplayer.LifeSteal += .1f + vitaPlayer.Counter * .01f;
		modplayer.DodgeChance += .1f + vitaPlayer.Counter * .01f;
		modplayer.UpdateDefenseBase += .1f + vitaPlayer.Counter * .01f;
		modplayer.DebuffBuffTime -= .1f + vitaPlayer.Counter * .01f;
		modplayer.AttackSpeed += .05f + vitaPlayer.Counter * .01f;
		modplayer.UpdateCritDamage += .15f + vitaPlayer.Counter * .01f;
		player.GetDamage(DamageClass.Generic) += .1f + vitaPlayer.Counter * .01f;
		player.GetCritChance(DamageClass.Generic) += 5 + vitaPlayer.Counter;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
}
public class EverlastingOverhaul_Vitamin_ModPlayer : ModPlayer {
	public int Counter = 0;
	public int TimerDecay = 0;
	public bool Vitamin = false;
	public override void ResetEffects() {
		Vitamin = false;
		if (++TimerDecay >= 180) {
			Counter = ModUtils.CountDown(Counter);
			TimerDecay = 0;
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Vitamin)
			Counter++;
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (Vitamin)
			Counter++;
	}
}
