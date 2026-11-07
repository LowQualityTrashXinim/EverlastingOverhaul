using EverlastingOverhaul.Common .ItemOverhaul.Common;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_ObsidianSkull : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.ObsidianSkull;
	public override void UpdateEquip(Item item, Player player) {
		player.ModPlayerStats().UpdateHPMax += .1f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
}
public class EverlastingOverhaul_ObsidianSkull_ModPlayer : ModPlayer {
	public bool skull = false;
	public override void ResetEffects() {
		skull = false;
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		ModifyHit(ref modifiers, npc);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		ModifyHit(ref modifiers);
	}
	private void ModifyHit(ref Player.HurtModifiers mod, NPC npc = null) {
		if (!skull) {
			return;
		}
		mod.SourceDamage -= .25f;
		if (npc == null) {
			return;
		}
		npc.AddBuff<FuryOfTheSun>(ModUtils.ToSecond(5));
		Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(100, ModUtils.DirectionFromEntityAToEntityB(Player.Center.X, npc.Center.X)));
	}
}
