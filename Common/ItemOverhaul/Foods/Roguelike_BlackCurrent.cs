using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_BlackCurrent : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BlackCurrant;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(1.5f);
	public override int ManaAmount() => 60;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_BlackCurrent_Buff>(), ModUtils.ToMinute(6));
	}
}
public class Roguelike_BlackCurrent_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.BlackCurrant;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateHPMax.Base += 75;
		handler.UpdateManaMax += .25f;
		player.GetModPlayer<Roguelike_BlackCurrent_ModPlayer>().BlackCurrent = true;
	}
}
public class Roguelike_BlackCurrent_ModPlayer : ModPlayer {
	public bool BlackCurrent = false;
	public override void ResetEffects() {
		BlackCurrent = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		SlowChance(target);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		SlowChance(target);
	}
	private void SlowChance(NPC target) {
		if (BlackCurrent && Main.rand.NextFloat() <= .025f) {
			target.AddBuff(BuffID.Slow, ModUtils.ToSecond(Main.rand.Next(4, 9)));
		}
	}
}
