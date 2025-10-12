using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.BuffAndDebuff;
internal class DeepSeaPressure : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(Cure: true);
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 10;
		npc.lifeRegen -= 20;
		if (npc.buffType.Where(b => b != Type && Main.debuff[b]).Any()) {
			npc.lifeRegen -= 10;
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 5f;
		}
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: -10);
		player.lifeRegen -= 20;
		if (player.buffType.Where(b => b != Type && Main.debuff[b]).Any()) {
			player.lifeRegen -= 10;
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.Defense, Base: -10);
		}
	}
}
