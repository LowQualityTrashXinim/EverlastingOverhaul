using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_PotatoChips : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.PotatoChips;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(5);
	public override int LifeAmount() => 25;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_PotatoChips_ModBuff>(), ModUtils.ToMinute(11));
	}
}
public class Roguelike_PotatoChips_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.PotatoChips;
	public override void Update(Player player, ref int buffIndex) {
		player.GetDamage(DamageClass.Summon) += .25f;
		player.GetDamage(DamageClass.Melee) -= .15f;
		player.ModPlayerStats().MeleeAtkSpeed -= .2f;
	}
}
