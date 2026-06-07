using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_JojaCola : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.JojaCola;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(2);
	public override int EnergyAmount() => 40;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = 30;
		SetBuff(item, ModContent.BuffType<Roguelike_JojaCola_Buff>(), ModUtils.ToMinute(3));
	}
}
public class Roguelike_JojaCola_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.JojaCola;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_JojaCola_ModPlayer>().JojaCola = true;
		player.GetDamage(DamageClass.Ranged) += .13f;
		player.GetCritChance(DamageClass.Ranged) += 9;
		player.ModPlayerStats().UpdateDefenseBase -= .08f;
	}
}
public class Roguelike_JojaCola_ModPlayer : ModPlayer {
	public bool JojaCola = false;
	public override void ResetEffects() {
		JojaCola = false;
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (JojaCola) {
			velocity = velocity.Vector2RotateByRandom(40) * Main.rand.NextFloat(.6f, 1.1f);
		}
	}
}
