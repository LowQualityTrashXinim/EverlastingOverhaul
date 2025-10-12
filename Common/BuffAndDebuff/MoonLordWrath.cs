using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;


namespace EverlastingOverhaul.Common.BuffAndDebuff
{
	internal class MoonLordWrath : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.moonLeech = true;
			player.lifeRegen = 0;
			player.lifeRegenCount = 0;
			player.lifeRegenTime = 0;
		}
	}
}
