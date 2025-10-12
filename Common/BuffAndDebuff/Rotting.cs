using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.BuffAndDebuff
{
	internal class Rotting : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.statDefense -= 10;
			player.lifeRegen -= 12;
		}
	}
}
