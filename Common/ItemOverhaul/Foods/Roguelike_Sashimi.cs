using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Sashimi : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Sashimi;
	public override int EnergyAmount() => 290;
	public override int LifeAmount() => 100;
	public override int ManaAmount() => 180;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4.5f);
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Sashimi_Buff>(), ModUtils.ToMinute(28));
	}
}
public class Roguelike_Sashimi_Buff : FoodItemTier2 {
	public override int TypeID => ItemID.Sashimi;
	public override void Update(Player player, ref int buffIndex) {
		player.GetDamage(DamageClass.Generic) += .1f;
		player.GetCritChance(DamageClass.Generic) += 2;
		player.fishingSkill += 13;
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateCritDamage += .25f;
		if (player.wet || player.lavaWet) {
			handler.UpdateHPRegen += .25f;
			handler.UpdateMovement += .35f;
			player.tileSpeed += .35f;
		}
	}
}
