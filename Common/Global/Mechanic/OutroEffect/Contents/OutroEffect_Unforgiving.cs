using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Unforgiving;
using EverlastingOverhaul.Common.Utils;
using Terraria;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_Unforgiving : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(6);
	}
	public override void Update(Player player) {
		player.GetModPlayer<Unforgiving_ModPlayer>().OutroAttack = true;
	}
}
