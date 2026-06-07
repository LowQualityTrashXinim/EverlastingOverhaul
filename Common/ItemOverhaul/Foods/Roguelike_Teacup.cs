using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Teacup : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Teacup;
	public override int ManaAmount() => 60;
	public override int EnergyAmount() => 120;
	public override int CoolDownBetweenUse() => 120;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Teacup_Buff>(), ModUtils.ToMinute(8));
	}
}
public class Roguelike_Teacup_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Teacup;
	public override void Update(Player player, ref int buffIndex) {
		player.GetCritChance(DamageClass.Generic) += 10;
		player.ModPlayerStats().UpdateCritDamage += .25f;
		player.tileSpeed += .09f;
		player.pickSpeed += .09f;
		player.blockRange += 2;
	}
}
public class Roguelike_Teacup_GlobalNPC : GlobalNPC {
	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
		if (player.HasBuff<Roguelike_Teacup_Buff>()) {
			spawnRate += spawnRate / 10;
		}
	}
}
