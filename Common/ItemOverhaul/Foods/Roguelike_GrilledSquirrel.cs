using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_GrilledSquirrel : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.GrilledSquirrel;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(5);
	public override int EnergyAmount() => 37;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_GrilledSquirrel_ModBuff>(), ModUtils.ToMinute(13));
	}
}
public class Roguelike_GrilledSquirrel_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.GrilledSquirrel;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateMovement += .06f;
		handler.UpdateJumpBoost += .12f;
		player.extraFall += 20;
		if(player.grapCount > 0) {
			handler.DodgeChance += .035f;
			player.GetCritChance(DamageClass.Generic) += 16;
		}
	}
}
