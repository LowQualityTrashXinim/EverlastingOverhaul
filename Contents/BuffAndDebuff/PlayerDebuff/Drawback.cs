using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.BuffAndDebuff.PlayerDebuff;
internal class Drawback : ModBuff{
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(true);
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.AddStatsToPlayer(PlayerStats.RegenHP, Flat: -10);
		modplayer.AddStatsToPlayer(PlayerStats.PureDamage, .75f);
		modplayer.AddStatsToPlayer(PlayerStats.MovementSpeed, .8f);
		modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, .85f);
		modplayer.AddStatsToPlayer(PlayerStats.CritChance, Base: -10);
	}
}
