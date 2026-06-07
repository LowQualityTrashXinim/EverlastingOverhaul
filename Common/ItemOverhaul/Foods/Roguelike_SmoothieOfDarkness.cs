using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_SmoothieOfDarkness : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.SmoothieofDarkness;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3.5f);
	public override int ManaAmount() => 80;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_SmoothieOfDarkness_ModBuff>(), ModUtils.ToMinute(10));
	}
}
public class Roguelike_SmoothieOfDarkness_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.SmoothieofDarkness;
	public override void Update(Player player, ref int buffIndex) {
		player.GetDamage(DamageClass.Magic) += .24f;
		player.GetDamage(DamageClass.Ranged) += .24f;
		player.GetCritChance(DamageClass.Generic) -= 10;
		player.GetModPlayer<Roguelike_SmoothieOfDarkness_ModPlayer>().SmoothieOfDarkness = true;
	}
}
public class Roguelike_SmoothieOfDarkness_ModPlayer : ModPlayer {
	public bool SmoothieOfDarkness = false;
	public override void ResetEffects() {
		SmoothieOfDarkness = false;
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!SmoothieOfDarkness) {
			return;
		}
		if (proj.DamageType == DamageClass.Magic || proj.DamageType == DamageClass.Ranged) {
			target.AddBuff(BuffID.CursedInferno, ModUtils.ToSecond(Main.rand.Next(4, 10)));
		}
	}
}
