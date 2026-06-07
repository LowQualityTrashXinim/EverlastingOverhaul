using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_IceCream : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.IceCream;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override int ManaAmount() => 135;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1);
		SetBuff(item, ModContent.BuffType<Roguelike_IceCream_ModBuff>(), ModUtils.ToMinute(14));
	}
	public override void OnConsumeFood(Item item, Player player) {
		player.ModPlayerStats().Set_TemporaryMana(220, 120);
		player.ModPlayerStats().TemporaryMana += 220;
	}
}
public class Roguelike_IceCream_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.IceCream;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Rapid_ManaRegen += .2f;
		handler.UpdateManaMax.Base += 180;
		handler.Set_TemporaryMana(220, 120);
		player.GetDamage(DamageClass.Magic) *= 1.12f;
		player.manaCost += .5f;
	}
}
