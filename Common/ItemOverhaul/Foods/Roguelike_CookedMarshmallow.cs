using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_CookedMarshmallow : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.CookedMarshmallow;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(2);
	public override int EnergyAmount() => 142;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_CookedMarshmallow_ModBuff>(), ModUtils.ToMinute(4.5f));
	}
}
public class Roguelike_CookedMarshmallow_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.CookedMarshmallow;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateHPRegen.Flat += 3;
		handler.UpdateManaRegen.Base += 3;
		handler.BuffTime *= 1.52f;
		handler.DebuffBuffTime *= .7f;
	}
}
