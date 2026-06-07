using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Peach : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Peach;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4);
	public override int EnergyAmount() => 57;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2);
		SetBuff(item, ModContent.BuffType<Roguelike_Peach_Buff>(), ModUtils.ToMinute(2.5f));
	}
}
public class Roguelike_Peach_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Peach;
	public override void Update(Player player, ref int buffIndex) {
		//player.GetModPlayer<SkillHandlePlayer>().skilldamage += .23f;
		//player.ModPlayerStats().EnergyRecharge += .14f;
	}
}
