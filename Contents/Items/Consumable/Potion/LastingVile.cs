using Terraria;
using Terraria.ModLoader;
 
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.Items.Consumable.Potion;
internal class LastingVilePotion : ModItem {
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<LastingVileBuff>(), ModUtils.ToMinute(5));
	}
	public override bool? UseItem(Player player) {
		if (player.itemAnimation == player.itemAnimationMax - 1) {
			for (int i = 0; i < player.buffTime.Length; i++) {
				if (player.buffTime[i] != 0 || ModItemLib.CanBeAffectByLastingVile[player.buffType[i]]) {
					player.buffTime[i] += ModUtils.ToMinute(2);
				}
			}
		}
		return base.UseItem(player);
	}
}
public class LastingVileBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff(true);
	}
	public override void Update(Player player, ref int buffIndex) {
		if (player.buffTime[buffIndex] > 0) {
			player.GetModPlayer<LastingVilePlayer>().IsActive = true;
		}
	}
}
public class LastingVilePlayer : ModPlayer {
	public bool IsActive = false;
	public override void ResetEffects() {
		IsActive = false;
	}
	public override bool CanUseItem(Item item) {
		if (IsActive && item.type == ModContent.ItemType<LastingVilePotion>()) {
			return false;
		}
		return base.CanUseItem(item);
	}
}
