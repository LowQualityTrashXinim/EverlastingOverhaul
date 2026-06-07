using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_SeafoodDinner : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.SeafoodDinner;
	public override int LifeAmount() => 120;
	public override int ManaAmount() => 200;
	public override int EnergyAmount() => 270;
	public override int CoolDownBetweenUse() => 300;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_SeafoodDinner_Buff>(), ModUtils.ToMinute(29));
	}
}
public class Roguelike_SeafoodDinner_Buff : FoodItemTier2 {
	public override int TypeID => ItemID.SeafoodDinner;
	public override void Update(Player player, ref int buffIndex) {
		player.luck += .4f;
		player.fishingSkill += 25;
		player.GetKnockback(DamageClass.Generic).Base += 2.5f;
		player.endurance += .05f;
		player.ModPlayerStats().UpdateDefenseBase.Base += 20;
	}
}
public class Roguelike_SeafoodDinner_GlobalNPC : GlobalNPC {
	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
		if (player.HasBuff<Roguelike_SeafoodDinner_Buff>()) {
			spawnRate += (int)(spawnRate * .25f);
		}
	}
}
