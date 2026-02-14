using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Consumable.Potion;

class MeleePotion : ModItem {
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<Melee_Buff>(), ModUtils.ToMinute(10));
	}
}
public class Melee_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handle = player.GetModPlayer<PlayerStatsHandle>();
		handle.AddStatsToPlayer(PlayerStats.MeleeDMG, Multiplicative: 1.1f);
		handle.AddStatsToPlayer(PlayerStats.MeleeCritChance, Base: 10);
		handle.AddStatsToPlayer(PlayerStats.MeleeAtkSpeed, 1.1f);
	}
}
