using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_PeachSangria : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.PeachSangria;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(8);
	public override int EnergyAmount() => 95;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_PeachSangria_Buff>(), ModUtils.ToMinute(7));
	}
}
public class Roguelike_PeachSangria_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.PeachSangria;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		//handler.EnergyCap.Base += 250;
		//handler.EnergyRegen += .13f;
		//handler.SkillDuration += .15f;
		//player.GetModPlayer<SkillHandlePlayer>().skilldamage += .17f;
	}
}
