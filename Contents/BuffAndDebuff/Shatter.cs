using Terraria;
using Terraria.ModLoader;

using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.BuffAndDebuff;
internal class Shatter : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense *= 0;
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Multiplicative: 0);
	}
}
