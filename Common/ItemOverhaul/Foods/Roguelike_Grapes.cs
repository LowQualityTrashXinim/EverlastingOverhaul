using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Grapes : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Grapes;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(5);
	public override int EnergyAmount() => 200;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Grapes_ModBuff>(), ModUtils.ToMinute(12));
	}
}
public class Roguelike_Grapes_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.Grapes;
	public override void Update(Player player, ref int buffIndex) {
		player.GetDamage(DamageClass.Melee) -= .1f;
		player.GetDamage(DamageClass.Ranged) -= .1f;
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.TrueDamage_Summon += .35f;
		player.GetModPlayer<Roguelike_Grapes_ModPlayer>().Grapes = true;
	}
}
public class Roguelike_Grapes_ModPlayer : ModPlayer {
	public bool Grapes = false;
	public override void ResetEffects() {
		Grapes = false;
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Grapes && proj.minion && proj.DamageType == DamageClass.Summon && Main.rand.NextBool(10)) {
			Player.StrikeNPCDirect(target, hit);
		}
	}
}
