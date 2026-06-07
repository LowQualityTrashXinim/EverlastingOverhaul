using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Pineapple : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Pineapple;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(5);
	public override int LifeAmount() => 37;
	public override int EnergyAmount() => 62;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_Pineapple_Buff>(), ModUtils.ToMinute(4));
	}
}
public class Roguelike_Pineapple_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Pineapple;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateThorn.Base += 7;
		handler.UpdateThorn += .12f;
		handler.UpdateDefenseBase.Base -= 5;
	}
}
