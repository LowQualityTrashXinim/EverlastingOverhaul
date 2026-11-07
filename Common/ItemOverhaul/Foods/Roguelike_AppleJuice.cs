using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_AppleJuice : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.AppleJuice;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4.75f);
	public override int LifeAmount() => 64;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_AppleJuice_Buff>(), ModUtils.ToMinute(8));
	}
}
public class EverlastingOverhaul_AppleJuice_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.AppleJuice;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.SynergyDamage += .17f;
		player.GetDamage(DamageClass.Generic) += .07f;
		player.GetModPlayer<EverlastingOverhaul_AppleJuice_ModPlayer>().AppleJuice = true;
	}
}
public class EverlastingOverhaul_AppleJuice_ModPlayer : ModPlayer {
	public bool AppleJuice = false;
	public override void ResetEffects() {
		AppleJuice = false;
	}
	public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price) {
		if (AppleJuice) {
			price = (int)(price * .75f);
		}
	}
	public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback) {
		if (AppleJuice) {
			knockback.Base += 1.5f;
		}
	}
}
