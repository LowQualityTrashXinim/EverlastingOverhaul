using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Banana : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Banana;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(5);
	public override int EnergyAmount() => 80;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2);
		SetBuff(item, ModContent.BuffType<Roguelike_Banana_Buff>(), ModUtils.ToSecond(2.5f));
	}
}
public class Roguelike_Banana_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Banana;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.NonCriticalDamage += .13f;
		player.GetModPlayer<Roguelike_Banana_ModPlayer>().Banana = true;
	}
}
public class Roguelike_Banana_ModPlayer : ModPlayer {
	public bool Banana = false;
	public override void ResetEffects() {
		Banana = false;
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Banana && Main.rand.NextFloat() <= .07f) {
			Player.ModPlayerStats().Request_ShootExtra(1, 10);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
}
