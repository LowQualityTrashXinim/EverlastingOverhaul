using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.Items.Consumable.Scroll {
	class ScrollofStrike : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<StrikeSpell>(), ModUtils.ToMinute(1));
		}
	}
	public class StrikeSpell : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<PlayerStatsHandle>().AddStatsToPlayer(PlayerStats.PureDamage, 1.1f);
		}
	}
	public class StrikePlayer : ModPlayer {
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if (Player.HasBuff<StrikeSpell>()) {
				Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<StrikeSpell>()));
				for (int i = 0; i < 5; i++) {
					Player.StrikeNPCDirect(target, hit);
				}
			}
		}
	}
}
