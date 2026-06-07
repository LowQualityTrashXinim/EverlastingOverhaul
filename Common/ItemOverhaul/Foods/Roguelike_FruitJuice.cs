using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_FruitJuice : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.FruitJuice;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(5);
	public override int LifeAmount() => 90;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_FruitJuice_ModBuff>(), ModUtils.ToMinute(8));
	}
	public override void OnConsumeFood(Item item, Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Set_TemporaryLife(150, 60);
		handler.TemporaryLife += 150;
	}
}
public class Roguelike_FruitJuice_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.FruitJuice;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateDefenseBase.Base += 2;
		handler.MeleeAtkSpeed += .05f;
		handler.UpdateMovement += .2f;
		handler.UpdateHPRegen.Base += 2;
		handler.Set_TemporaryLife(150, 60);
		player.pickSpeed += .05f;
		player.GetDamage(DamageClass.Generic) += .05f;
		player.GetCritChance(DamageClass.Generic) += 2;
	}
}
