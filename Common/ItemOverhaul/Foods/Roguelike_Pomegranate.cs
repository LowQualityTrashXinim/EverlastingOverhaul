using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Pomegranate : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Pomegranate;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(2.75f);
	public override int ManaAmount() => 90;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_Pomegranate_ModBuff>(), ModUtils.ToMinute(8));
	}
	public override void OnConsumeFood(Item item, Player player) {
		player.ModPlayerStats().Set_TemporaryMana(160, 120);
		player.ModPlayerStats().TemporaryMana += 160;
	}
}
public class Roguelike_Pomegranate_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.Pomegranate;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().UpdateManaMax.Base += 40;
		player.ModPlayerStats().Set_TemporaryMana(160, 120);
		player.GetModPlayer<Roguelike_Pomegranate_ModPlayer>().Pomegranate = true;
	}
}
public class Roguelike_Pomegranate_ModPlayer : ModPlayer {
	public bool Pomegranate = false;
	public override void ResetEffects() {
		Pomegranate = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Pomegranate && Main.rand.NextBool(20)) {
			target.AddBuff(BuffID.OnFire, ModUtils.ToSecond(3));
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Pomegranate && Main.rand.NextBool(20)) {
			target.AddBuff(BuffID.OnFire, ModUtils.ToSecond(3));
		}
	}
}
