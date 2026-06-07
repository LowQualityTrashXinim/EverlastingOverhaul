using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_FriedEgg : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.FriedEgg;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(5);
	public override int LifeAmount() => 40;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_FriedEgg_ModBuff>(), ModUtils.ToMinute(23));
	}
}
public class Roguelike_FriedEgg_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.FriedEgg;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().SynergyDamage += .12f;
		player.GetDamage(DamageClass.Melee) += .2f;
		player.GetDamage(DamageClass.Ranged) += .2f;
		player.GetModPlayer<Roguelike_FriedEgg_ModPlayer>().FriedEgg = true;
	}
}
public class Roguelike_FriedEgg_ModPlayer : ModPlayer {
	public bool FriedEgg = false;
	int counter = 0;
	int cooldown = 0;
	public override void ResetEffects() {
		FriedEgg = false;
	}
	public override void UpdateEquips() {
		if (FriedEgg) {
			Player.ModPlayerStats().SynergyDamage += counter * .01f;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (FriedEgg && cooldown <= 0) {
			cooldown = ModUtils.ToSecond(.5f);
			counter = Math.Clamp(counter++, 0, 30);
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (FriedEgg && cooldown <= 0 && !proj.minion) {
			cooldown = ModUtils.ToSecond(.5f);
			counter = Math.Clamp(counter++, 0, 30);
		}
	}
}
