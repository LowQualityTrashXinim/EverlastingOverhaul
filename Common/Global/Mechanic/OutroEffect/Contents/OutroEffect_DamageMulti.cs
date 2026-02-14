using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_DamageMulti : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(20);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		damage *= 1.05f;
	}
}
