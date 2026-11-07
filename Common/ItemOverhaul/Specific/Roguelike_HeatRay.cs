using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
public class EverlastingOverhaul_HeatRay : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.HeatRay) {
			entity.useTime = entity.useAnimation = 4;
			entity.mana = 4;
			entity.damage = 40;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.HeatRay) {
			ModUtils.AddTooltip(ref tooltips, new(Mod, "EverlastingOverhaul_HeatRay", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
		}
	}
}
public class EverlastingOverhaul_HeatRay_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int HeatRay_Decay = 0;
	public int HeatRay_HitCount = 0;
	public override void PostAI(NPC npc) {
		if (HeatRay_HitCount > 0) {
			HeatRay_Decay = ModUtils.CountDown(HeatRay_Decay);
			if (HeatRay_Decay <= 0) {
				HeatRay_HitCount--;
			}
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (projectile.type == ProjectileID.HeatRay) {
			modifiers.SourceDamage += HeatRay_HitCount * .02f;
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.type == ProjectileID.HeatRay) {
			HeatRay_HitCount = Math.Clamp(HeatRay_HitCount + 1, 0, 200);
			HeatRay_Decay = 30;
		}
	}
}
