using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Apricot : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Apricot;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override int LifeAmount() => 44;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_BlackCurrent_Buff>(), ModUtils.ToMinute(3));
	}
}
public class Roguelike_Apricot_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Apricot;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		//handler.DebuffBuffTime *= GlobalFoodItem.FoodValue(player, .93f);
		//handler.BuffTime *= GlobalFoodItem.FoodValue(player, 1.06f);
		//handler.FoodValue += .04f;
	}
}
