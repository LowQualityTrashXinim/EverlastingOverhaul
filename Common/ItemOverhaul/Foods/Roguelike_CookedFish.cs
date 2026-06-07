using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_CookedFish : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.CookedFish;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4);
	public override int LifeAmount() => 45;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_CookedFish_Buff>(), ModUtils.ToMinute(6));
	}
}
public class Roguelike_CookedFish_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.CookedFish;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateMovement += .15f;
		handler.UpdateDefenseBase.Base += 5;
		handler.UpdateHPRegen.Base += 2;
		player.GetDamage(DamageClass.Generic) += .15f;
		player.fishingSkill += 10;
	}
}
