using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Coffee : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.CoffeeCup;
	public override int EnergyAmount() => 200;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(20);
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2);
		SetBuff(item, ModContent.BuffType<Roguelike_Coffee_ModBuff>(), ModUtils.ToMinute(14));
	}
}
public class Roguelike_Coffee_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.CoffeeCup;
	public override void Update(Player player, ref int buffIndex) {
		player.runAcceleration += .1f;
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateMovement += .21f;
		handler.UpdateJumpBoost += .21f;
		handler.MeleeAtkSpeed += .07f;
	}
}
