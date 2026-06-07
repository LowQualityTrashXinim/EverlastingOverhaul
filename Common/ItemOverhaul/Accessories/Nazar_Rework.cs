using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_Nazar : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Nazar;
	}
	public override void UpdateEquip(Item item, Player player) {
		player.ModPlayerStats().DebuffBuffTime -= .5f;
		player.GetModPlayer<Roguelike_Nazar_ModPlayer>().Nazar = true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_Nazar_ModPlayer : ModPlayer {
	public bool Nazar = false;
	public override void ResetEffects() {
		Nazar = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		AddDebuff(target);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.minion) {
			return;
		}
		AddDebuff(target);
	}
	private void AddDebuff(NPC target) {
		if (!Nazar) {
			return;
		}
		target.AddBuff(BuffID.Cursed, ModUtils.ToSecond(Main.rand.Next(7, 16)));
	}
}
