using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_SauteedFrogLegs : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.SauteedFrogLegs;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(7.5f);
	public override int EnergyAmount() => 105;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(4);
		SetBuff(item, ModContent.BuffType<Roguelike_SauteedFrogLegs_ModBuff>(), ModUtils.ToMinute(4));
	}
}
public class Roguelike_SauteedFrogLegs_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.SauteedFrogLegs;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.DodgeChance += .07f;
		handler.UpdateJumpBoost += .35f;
		handler.UpdateMovement += .15f;
	}
}
