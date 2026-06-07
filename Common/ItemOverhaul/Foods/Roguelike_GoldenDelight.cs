using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_GoldenDelight : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.GoldenDelight;
	public override int LifeAmount() => 250;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(30);
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(10);
		SetBuff(item, ModContent.BuffType<Roguelike_GoldenDelight_ModBuff>(), ModUtils.ToMinute(60));
	}
}
public class Roguelike_GoldenDelight_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.GoldenDelight;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().UpdateCritDamage += .35f;
		player.GetCritChance(DamageClass.Magic) += 10;
		player.GetModPlayer<Roguelike_GoldenDelight_ModPlayer>().GoldenDelight = true;
	}
}
public class Roguelike_GoldenDelight_ModPlayer : ModPlayer {
	public bool GoldenDelight = false;
	public override void ResetEffects() {
		GoldenDelight = false;
	}
	public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		if (GoldenDelight) {
			modifiers.SourceDamage -= Main.rand.NextFloat(.05f, .5f);
		}
	}
	public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		if (GoldenDelight) {
			modifiers.SourceDamage -= Main.rand.NextFloat(.05f, .5f);
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (GoldenDelight) {
			target.AddBuff(BuffID.Midas, ModUtils.ToSecond(Main.rand.Next(5, 10)));
		}
	}
}
