using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_PadThai : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.PadThai;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3.5f);
	public override int EnergyAmount() => 485;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(4.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_PadThai_ModBuff>(), ModUtils.ToMinute(21));
	}
}
public class Roguelike_PadThai_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.PadThai;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		//handler.EnergyCap.Base += 420;
		//handler.EnergyRecharge += .25f;
		//handler.SkillDuration += .34f;
		//handler.EnergyRegen += .32f;
	}
}
