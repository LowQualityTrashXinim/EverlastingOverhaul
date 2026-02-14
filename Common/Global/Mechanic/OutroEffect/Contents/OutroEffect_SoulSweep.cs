using EverlastingOverhaul.Common.Utils;
using Terraria;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_SoulSweep : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(5);
	}
	public override void Update(Player player) {
		player.ModPlayerStats().PercentageDamage += .005f;
	}
}
