using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Rambuten : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Rambutan;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override int LifeAmount() => 35;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_Rambuten_Buff>(), ModUtils.ToMinute(8));
	}
}
public class Roguelike_Rambuten_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Rambutan;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.LifeSteal += .01f;
		handler.HealEffectiveness += .15f;
		handler.UpdateDefenseBase.Base += .03f;
		handler.UpdateThorn.Base += 7;
	}
}
