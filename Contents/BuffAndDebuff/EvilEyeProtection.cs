using EverlastingOverhaul.Contents.Items.Accessories.EnragedBossAccessories.EvilEye;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.BuffAndDebuff;
internal class EvilEyeProtection : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = false;
		Main.buffNoSave[Type] = true;
	}

	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<EvilEyePlayer>().EyeProtection = false;
	}
}
