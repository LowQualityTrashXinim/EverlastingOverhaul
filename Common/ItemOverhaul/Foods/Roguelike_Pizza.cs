using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Pizza : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Pizza;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(45);
	public override int LifeAmount() => 105;
	public override int ManaAmount() => 125;
	public override int EnergyAmount() => 205;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(9);
		SetBuff(item, ModContent.BuffType<Roguelike_Pizza_ModBuff>(), ModUtils.ToMinute(55));
	}
}
public class Roguelike_Pizza_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.Pizza;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		modplayer.UpdateMinion.Base += 1;
		modplayer.UpdateSentry.Base += 1;
		modplayer.SummonTagDamage += .3f;
		modplayer.AttackSpeed -= .25f;
		player.GetDamage(DamageClass.Summon) += .2f;
		player.GetKnockback(DamageClass.Summon).Base += 3;
	}
}
