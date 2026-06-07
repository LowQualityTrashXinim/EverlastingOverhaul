using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_BloodOrange : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BloodOrange;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(1.5f);
	public override int LifeAmount() => 25;
	public override void SetFoodDefaults(Item item) {
		item.useAnimation = item.useTime = ModUtils.ToSecond(.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_BloodOrange_Buff>(), ModUtils.ToMinute(6));
	}
}
public class Roguelike_BloodOrange_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.BloodOrange;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateHPMax.Base += 40;
		handler.UpdateHPRegen += .25f;
		player.GetModPlayer<Roguelike_BloodOrange_ModPlayer>().BloodOrange = true;
	}
}
public class Roguelike_BloodOrange_ModPlayer : ModPlayer {
	public bool BloodOrange = false;
	public override void ResetEffects() {
		BloodOrange = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		HitEffect(target);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		HitEffect(target);
	}
	private void HitEffect(NPC npc) {
		if (Main.rand.NextFloat() <= .05f) {
			npc.AddBuff(BuffID.Bleeding, ModUtils.ToSecond(Main.rand.Next(5, 11)));
		}
	}
}
