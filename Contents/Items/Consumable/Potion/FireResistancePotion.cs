using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
 
using EverlastingOverhaul.Texture;

namespace EverlastingOverhaul.Contents.Items.Consumable.Potion;
internal class FireResistancePotion : ModItem{
	public override string Texture => ModTexture.MISSINGTEXTUREPOTION;
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<FireResistanceBuff>(), ModUtils.ToMinute(1.5f));
	}
}
public class FireResistanceBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		for (int i = 0; i < TerrariaArrayID.FireBuff.Length; i++) {
			player.buffImmune[TerrariaArrayID.FireBuff[i]] = true;
		}
	}
}
