using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Nachos : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Nachos;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(8);
	public override int EnergyAmount() => 150;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_Nachos_ModBuff>(), ModUtils.ToMinute(17));
	}
}
public class Roguelike_Nachos_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.Nachos;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Melee_NonCritDmg += .2f;
		handler.Range_NonCritDmg += .2f;
		handler.Magic_NonCritDmg += .2f;
		player.GetDamage(DamageClass.Summon) += .15f;
		player.endurance += .05f;
		handler.UpdateMovement += .25f;
	}
}
