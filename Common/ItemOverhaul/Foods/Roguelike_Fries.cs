using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Fries : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Fries;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(12);
	public override int LifeAmount() => 30;
	public override int EnergyAmount() => 60;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Fries_ModBuff>(), ModUtils.ToMinute(23));
	}
}
public class Roguelike_Fries_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.Fries;
	public override void Update(Player player, ref int buffIndex) {
		var handler = player.ModPlayerStats();
		handler.UpdateMinion.Base += 1;
		handler.UpdateSentry.Base += 1;
		player.GetDamage(DamageClass.Summon) += .16f;
		player.GetDamage(DamageClass.SummonMeleeSpeed) += .35f;
		player.GetKnockback(DamageClass.SummonMeleeSpeed).Base += 1.4f;
		player.GetKnockback(DamageClass.Summon).Base += 1.4f;
	}
}
