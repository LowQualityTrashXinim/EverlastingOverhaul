using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.BuffAndDebuff
{
	internal class SecondChance : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
		}
	}
}
