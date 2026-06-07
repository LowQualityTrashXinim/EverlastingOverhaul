using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Pho : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Pho;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(10);
	public override int LifeAmount() => 180;
	public override int ManaAmount() => 280;
	public override int EnergyAmount() => 380;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(6);
		SetBuff(item, ModContent.BuffType<Roguelike_Pho_ModBuff>(), ModUtils.ToMinute(28));
	}
	public override void OnConsumeFood(Item item, Player player) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Set_TemporaryLife(110, 60);
		handler.Set_TemporaryMana(220, 60);
		//handler.Set_TemporaryEnergy(550, 60);
		handler.TemporaryLife += 110;
		handler.TemporaryMana += 220;
		//handler.TemporaryEnergy += 550;
	}
}
public class Roguelike_Pho_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.Pho;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Set_TemporaryLife(110, 60);
		handler.Set_TemporaryMana(220, 60);
		//handler.Set_TemporaryEnergy(550, 60);
		handler.TemporaryLife_CounterLimit += 60;
		handler.TemporaryMana_CounterLimit += 60;
		//handler.TemporaryEnergy_CounterLimit += 60;
		handler.UpdateHPRegen *= 1.12f;
		handler.UpdateManaRegen *= 1.12f;
		//handler.EnergyRegen += .37f;
	}
}
