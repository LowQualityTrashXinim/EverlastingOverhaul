using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.Items.Consumable.Scroll;
internal class ScrollOfProtection : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultPotion(32, 32, ModContent.BuffType<ProtectionSpell>(), ModUtils.ToSecond(20));
	}
}
public class ProtectionSpell : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class ProtectionSpell_Player : ModPlayer {
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (Player.HasBuff<ProtectionSpell>()) {
			modifiers.SetMaxDamage(1);
			Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<ProtectionSpell>()));
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (Player.HasBuff<ProtectionSpell>()) {
			modifiers.SetMaxDamage(1);
			Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<ProtectionSpell>()));
		}
	}
}

