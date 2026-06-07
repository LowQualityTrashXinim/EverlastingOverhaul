using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_ChristmasPudding : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.ChristmasPudding;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(12.5f);
	public override int LifeAmount() => 80;
	public override int ManaAmount() => 120;
	public override int EnergyAmount() => 220;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_ChristmasPudding_ModBuff>(), ModUtils.ToMinute(45));
	}
}
public class Roguelike_ChristmasPudding_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.ChristmasPudding;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().Iframe.Base += ModUtils.ToSecond(.2f);
		player.endurance += .05f;
		player.ModPlayerStats().DebuffBuffTime -= .5f;
	}
}
public class Roguelike_ChristmasPudding_ModPlayer : ModPlayer {
	public bool ChristmasPudding = false;
	public override void ResetEffects() {
		ChristmasPudding = false;
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		HitModify(ref modifiers);
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		HitModify(ref modifiers);
	}
	private void HitModify(ref Player.HurtModifiers modifier) {
		if (!ChristmasPudding) {
			return;
		}
		modifier.Knockback *= .35f;
		if (Main.rand.NextFloat() > .15f) {
			return;
		}
		modifier.SourceDamage -= .5f;
	}
}
