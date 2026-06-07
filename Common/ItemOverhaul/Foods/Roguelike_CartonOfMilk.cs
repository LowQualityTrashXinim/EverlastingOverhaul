using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_CartonOfMilk : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.MilkCarton;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4);
	public override int LifeAmount() => 85;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = 90;
		SetBuff(item, ModContent.BuffType<Roguelike_CartonOfMilk_Buff>(), ModUtils.ToMinute(4));
	}
}
public class Roguelike_CartonOfMilk_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.MilkCarton;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().UpdateDefenseBase.Base += player.statLifeMax2 / 25;
		player.GetModPlayer<Roguelike_CartonOfMilk_ModPlayer>().MilkCarton = true;
	}
}
public class Roguelike_CartonOfMilk_ModPlayer : ModPlayer {
	public bool MilkCarton = false;
	public override void ResetEffects() {
		MilkCarton = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		HitEffect(target);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		HitEffect(target);
	}
	private void HitEffect(NPC target) {
		if (Main.rand.NextFloat() <= .05f) {
			target.AddBuff(BuffID.BrokenArmor, ModUtils.ToSecond(Main.rand.Next(3, 9)));
		}
	}
}
