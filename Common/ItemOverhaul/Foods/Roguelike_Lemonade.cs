using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Lemonade : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Lemonade;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override int LifeAmount() => 45;
	public override int EnergyAmount() => 65;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_Lemonade_ModBuff>(), ModUtils.ToMinute(8));
	}
}
public class Roguelike_Lemonade_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.Lemonade;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Iframe.Base += ModUtils.ToSecond(0.25f);
		handler.DebuffTime += .13f;
		handler.TrueDamage += .13f;
		player.GetKnockback(DamageClass.Generic).Base += 1.25f;
	}
}
