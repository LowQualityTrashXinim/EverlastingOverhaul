using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.BuffAndDebuff.PlayerDebuff
{
	class LifeLoss : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MaxHP, .5f);
		}
	}
}
