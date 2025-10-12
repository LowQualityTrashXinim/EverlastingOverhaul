using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.BuffAndDebuff
{
	internal class RoyalAntiEscapeTm : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) {
			player.moveSpeed *= 0.75f;
			player.maxRunSpeed = 0.75f;
			player.runAcceleration *= 0.75f;
		}
	}
}
