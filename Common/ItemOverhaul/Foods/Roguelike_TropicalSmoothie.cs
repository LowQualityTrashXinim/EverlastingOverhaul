using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Systems.Skill;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_TropicalSmoothie : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.TropicalSmoothie;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(9);
	public override int EnergyAmount() => 155;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_TropicalSmoothie_ModBuff>(), ModUtils.ToMinute(11));
	}
}
public class EverlastingOverhaul_TropicalSmoothie_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.TropicalSmoothie;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		player.GetModPlayer<SkillHandlePlayer>().EnergyCapacity.Base += 100;
		handler.SkillDuration -= .17f;
		player.GetModPlayer<SkillHandlePlayer>().skilldamage += .19f;
		handler.UpdateThorn += .55f;
	}
}
