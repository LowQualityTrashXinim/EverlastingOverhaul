using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_RejuvinatingGlow : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(40);
	}
	public override void Update(Player player) {
		if (player.ModPlayerStats().Check_Heal()) {
			player.GetModPlayer<WeaponEffect_ModPlayer>().OutroEffect_RejuvinatingGlow_Counter = player.GetModPlayer<WeaponEffect_ModPlayer>().Arr_WeaponEffect[Type];
		}
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (player.GetModPlayer<WeaponEffect_ModPlayer>().OutroEffect_RejuvinatingGlow_Counter > 0) {
			damage *= 1.15f;
		}
	}
}
