using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.BuffAndDebuff;
internal class TheUnderworldWrath : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 180;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.lifeRegen -= 60;
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.DefenseEffectiveness, Additive: .23f);
	}
}
