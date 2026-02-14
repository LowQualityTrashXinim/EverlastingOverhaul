using Terraria;
using Terraria.ModLoader;
 
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.Items.Consumable.Potion;
internal class ShadowPotion : ModItem {
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<ShadowBuff>(), ModUtils.ToMinute(1.5f));
	}
}
public class ShadowBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
		modplayer.DodgeChance += 0.12f;
		modplayer.AddStatsToPlayer(PlayerStats.Iframe, 1.1f);
	}
}
