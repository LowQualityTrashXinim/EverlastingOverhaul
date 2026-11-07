using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using System.Drawing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_ThePlan : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.ThePlan;
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void UpdateEquip(Item item, Player player) {
		player.GetModPlayer<EverlastingOverhaul_ThePlan_ModPlayer>().ThePlan = true;
	}
}
public class EverlastingOverhaul_ThePlan_ModPlayer : ModPlayer {
	public bool ThePlan = false;
	public override void Initialize() {
		Timer_1 = new Point(0, 0);
		Timer_2 = new Point(0, 0);
		ThePlan = false;
	}
	public override void ResetEffects() {
		ThePlan = false;
		Timer_1.X = ModUtils.CountDown(Timer_1.X);
		if (Timer_1.X <= 0) {
			Timer_1.Y = ModUtils.CountDown(Timer_1.Y);
		}
		Timer_2.X = ModUtils.CountDown(Timer_2.X);
		if (Timer_2.X <= 0) {
			Timer_2.Y = ModUtils.CountDown(Timer_2.Y);
		}
	}
	public Point Timer_1 = new Point(0, 0);
	public Point Timer_2 = new Point(0, 0);
	public override void UpdateEquips() {
		if (Timer_1.X > 0) {
			Player.ModPlayerStats().Rapid_LifeRegen += 1;
		}
		if (!Player.IsHealthAbovePercentage(.5f) && Timer_1.Y <= 0) {
			Timer_1.X = ModUtils.ToSecond(10);
			Timer_1.Y = ModUtils.ToSecond(30);
		}
		if (!Player.IsHealthAbovePercentage(.2f) && Timer_2.Y <= 0) {
			Timer_2.X = ModUtils.ToSecond(15);
			Timer_2.Y = ModUtils.ToSecond(30);
		}
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		Modify(ref modifiers);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		Modify(ref modifiers);
	}
	private void Modify(ref Player.HurtModifiers modifiers) {
		if (Timer_2.X > 0) {
			modifiers.SourceDamage -= .6f;
		}
	}
}
