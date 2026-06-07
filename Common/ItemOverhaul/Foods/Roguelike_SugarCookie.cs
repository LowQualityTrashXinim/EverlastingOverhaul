using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_SugarCookie : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.SugarCookie;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4.25f);
	public override int EnergyAmount() => 500;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_SugarCookie_ModBuff>(), ModUtils.ToMinute(48));
	}
}
public class Roguelike_SugarCookie_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.SugarCookie;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().UpdateMovement += .3f;
		player.ModPlayerStats().UpdateJumpBoost += .2f;
		player.GetDamage(DamageClass.Generic) += player.velocity.Length() * .4f;
		player.GetCritChance(DamageClass.Generic) -= .25f;
		player.GetModPlayer<Roguelike_SugarCookie_ModPlayer>().SugarCookie = true;
	}
}
public class Roguelike_SugarCookie_ModPlayer : ModPlayer {
	public bool SugarCookie = false;
	public override void ResetEffects() {
		SugarCookie = false;
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (SugarCookie) {
			velocity = velocity.Vector2RotateByRandom(30);
		}
	}
}
