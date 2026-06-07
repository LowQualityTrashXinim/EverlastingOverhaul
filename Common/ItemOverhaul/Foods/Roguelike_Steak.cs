using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Steak : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Steak;
	public override int LifeAmount() => 220;
	public override int CoolDownBetweenUse() => 420;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Steak_Buff>(), ModUtils.ToMinute(44));
	}
}
public class Roguelike_Steak_Buff : FoodItemTier3 {
	public override int TypeID => ItemID.Steak;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Melee_CritDamage += .5f;
		handler.UpdateHPRegen.Base += 10;
		player.GetDamage(DamageClass.Melee) += .25f;
		player.GetCritChance(DamageClass.Melee) += 10;
		player.GetDamage(DamageClass.Magic) -= .9f;
		player.GetDamage(DamageClass.Ranged) -= .9f;
	}
}
