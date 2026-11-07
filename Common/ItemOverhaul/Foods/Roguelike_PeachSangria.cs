using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Systems.Skill;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_PeachSangria : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.PeachSangria;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(8);
	public override int EnergyAmount() => 95;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_PeachSangria_Buff>(), ModUtils.ToMinute(7));
	}
}
public class EverlastingOverhaul_PeachSangria_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.PeachSangria;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		player.GetModPlayer<SkillHandlePlayer>().EnergyCapacity.Base += 250;
		player.GetModPlayer<SkillHandlePlayer>().EnergyRegen += .13f;
		handler.SkillDuration += .15f;
		player.GetModPlayer<SkillHandlePlayer>().skilldamage += .17f;
	}
}
