using Microsoft.Xna.Framework;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.BuffAndDebuff
{
	internal class pumpkinOverdose : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex) {
			npc.color = Color.DarkOrange;
		}
	}
}