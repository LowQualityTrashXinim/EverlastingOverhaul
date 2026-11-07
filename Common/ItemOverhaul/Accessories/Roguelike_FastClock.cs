using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_FastClock : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.FastClock;
	}
	public override void UpdateEquip(Item item, Player player) {
		player.GetModPlayer<EverlastingOverhaul_FastClock_ModPlayer>().Fastclock = true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
}
public class EverlastingOverhaul_FastClock_ModPlayer : ModPlayer {
	public bool Fastclock = false;
	public int Counter = 0;
	public int Timer = 0;
	public override void ResetEffects() {
		Fastclock = false;
		if (--Timer <= 0) {
			Counter = 0;
			Timer = 0;
		}
	}
	public override void UpdateEquips() {
		if (Fastclock) {
			if (Player.ItemAnimationActive) {
				if (Player.ItemAnimationJustStarted) {
					Counter = Math.Min(++Counter, 10);
				}
				Timer = 240;
			}
			float increases = Counter * .01f;
			if (Counter >= 10) {
				increases += .15f;
			}
			Player.ModPlayerStats().AttackSpeed += increases;
		}
	}
}
