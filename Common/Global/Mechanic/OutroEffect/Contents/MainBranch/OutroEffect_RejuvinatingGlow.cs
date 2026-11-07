using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents.MainBranch;
internal class OutroEffect_RejuvinatingGlow : OutroEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(60);
	}
	public override void Update(Player player) {
		if (player.ModPlayerStats().Check_Heal()) {
			player.GetModPlayer<OutroEffect_ModPlayer>().OutroEffect_RejuvinatingGlow_Counter = player.GetModPlayer<OutroEffect_ModPlayer>().Arr_OutroEffect[Type];
		}
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (player.GetModPlayer<OutroEffect_ModPlayer>().OutroEffect_RejuvinatingGlow_Counter > 0) {
			damage *= 1.15f;
		}
	}
}
