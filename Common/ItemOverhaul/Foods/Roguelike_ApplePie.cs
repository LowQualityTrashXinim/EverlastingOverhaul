using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_ApplePie : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.ApplePie;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(17.5f);
	public override int LifeAmount() => 120;
	public override int ManaAmount() => 240;
	public override int EnergyAmount() => 175;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(5);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_ApplePie_ModBuff>(), ModUtils.ToMinute(54));
	}
}
public class EverlastingOverhaul_ApplePie_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.ApplePie;
	public override void Update(Player player, ref int buffIndex) {
		player.buffImmune[BuffID.Chilled] = true;
		player.buffImmune[BuffID.Frozen] = true;
		player.endurance += .1f;
		player.ModPlayerStats().OnKillHealing.Base += player.statLifeMax2 * .05f;
	}
}
