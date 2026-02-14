using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.Items.Consumable.Potion;
internal class KeenPotion : ModItem {
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<KeenBuff>(), ModUtils.ToMinute(2));
		Item.Set_AdvancedBuffItem();
	}
}
public class KeenBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.CritDamage, 1f);
	}
}
public class KeenPlayer : ModPlayer {
	public override void UpdateEquips() {
		if (Player.HasBuff<KeenBuff>()) {
			Player.ModPlayerStats().AlwaysCritValue++;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Player.HasBuff<KeenBuff>()) {
			int buffindex = Player.FindBuffIndex(ModContent.BuffType<KeenBuff>());
			Player.buffTime[buffindex] -= ModUtils.ToSecond(30);
		}
	}
}
