using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_Apricot : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Apricot;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override int LifeAmount() => 44;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(.5f);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_BlackCurrent_Buff>(), ModUtils.ToMinute(3));
	}
}
public class EverlastingOverhaul_Apricot_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Apricot;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.DebuffBuffTime *= .93f;
		handler.BuffTime *= 1.06f;
	}
}
