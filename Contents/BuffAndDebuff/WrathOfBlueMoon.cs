using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.BuffAndDebuff;
internal class WrathOfBlueMoon : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 15;
		if (npc.buffTime[buffIndex] <= 0) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().WrathOfBlueMoon = 0;
		}
	}
}
