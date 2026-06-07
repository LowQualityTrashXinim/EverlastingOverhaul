using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_MonsterLasagna : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.MonsterLasagna;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(9);
	public override int LifeAmount() => 80;
	public override int ManaAmount() => 120;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(5.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_MonsterLasagna_ModBuff>(), ModUtils.ToMinute(23));
	}
}
public class Roguelike_MonsterLasagna_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.MonsterLasagna;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateHPMax.Base += 40;
		handler.UpdateManaMax.Base += 80;
		handler.LifeSteal.Base += 2;
		handler.HealEffectiveness += .3f;
		player.GetModPlayer<Roguelike_MonsterLasagna_ModPlayer>().MonsterLasagna = true;
	}
}
public class Roguelike_MonsterLasagna_ModPlayer : ModPlayer {
	public bool MonsterLasagna = false;
	public override void ResetEffects() {
		MonsterLasagna = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (MonsterLasagna && Main.rand.NextBool(100)) {
			target.AddBuff(BuffID.ShadowFlame, ModUtils.ToSecond(Main.rand.Next(6, 12)));
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (MonsterLasagna && Main.rand.NextBool(100)) {
			target.AddBuff(BuffID.ShadowFlame, ModUtils.ToSecond(Main.rand.Next(6, 12)));
		}
	}
}
