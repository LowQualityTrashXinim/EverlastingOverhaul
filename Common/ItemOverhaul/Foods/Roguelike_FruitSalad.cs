using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_FruitSalad : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.FruitSalad;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(15);
	public override int LifeAmount() => 100;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(5);
		SetBuff(item, ModContent.BuffType<Roguelike_FruitSalad_ModBuff>(), ModUtils.ToMinute(12));
	}
	public override void OnConsumeFood(Item item, Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Set_TemporaryLife(50, 120);
		handler.TemporaryLife += 50;
	}
}
public class Roguelike_FruitSalad_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.FruitSalad;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateDefenseBase.Flat += 5;
		handler.MeleeAtkSpeed += .05f;
		handler.UpdateMovement += .2f;
		handler.UpdateHPRegen.Base += 2;
		handler.Set_TemporaryLife(50, 120);
		player.pickSpeed += .05f;
		player.GetDamage(DamageClass.Generic) += .05f;
		player.GetCritChance(DamageClass.Generic) += 2;
	}
}
