using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.BuffAndDebuff;
internal class FuryOfTheSun : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 35;
		if(npc.HasBuff(BuffID.OnFire) || npc.HasBuff(BuffID.OnFire)) {
			npc.lifeRegen -= 15;
		}
		if (npc.buffTime[buffIndex] <= 0) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().FuryOfTheSun = 0;
		}
	}
}
