using EverlastingOverhaul.Common.Utils;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
internal class Roguelike_CommonThrowable : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return TerrariaArrayID.SpecialPreBoss.Contains(entity.type) || TerrariaArrayID.Special.Contains(entity.type);
	}
	public override void SetDefaults(Item entity) {
		if (TerrariaArrayID.SpecialPreBoss.Contains(entity.type) || TerrariaArrayID.Special.Contains(entity.type)) {
			entity.damage += 10;
			entity.crit += 4;
			entity.shootSpeed += 5;
			entity.Set_ItemCriticalDamage(.5f);
		}
	}
}
