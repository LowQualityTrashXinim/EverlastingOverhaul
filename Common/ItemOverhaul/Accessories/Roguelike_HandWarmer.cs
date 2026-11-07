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
internal class EverlastingOverhaul_HandWarmer : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.HandWarmer;
	public override void UpdateEquip(Item item, Player player) {
		player.GetModPlayer<EverlastingOverhaul_HandWarmer_ModPlayer>().Handwarmer = true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
}
public class EverlastingOverhaul_HandWarmer_ModPlayer : ModPlayer {
	public bool Handwarmer = false;
	public override void ResetEffects() {
		Handwarmer = false;
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (Handwarmer) {
			if (proj.type == ProjectileID.SnowBallFriendly || proj.type == ProjectileID.SnowBallHostile) {
				modifiers.FinalDamage.Flat += 10;
			}
			if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
				modifiers.SourceDamage += .2f;
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Handwarmer && Main.rand.NextBool() && proj.type == ProjectileID.SnowBallFriendly || proj.type == ProjectileID.SnowBallHostile) {
			target.AddBuff(BuffID.Frostburn, 60);
		}
	}
}
