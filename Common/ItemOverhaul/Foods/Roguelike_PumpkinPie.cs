using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_PumpkinPie : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.PumpkinPie;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(20);
	public override int LifeAmount() => 60;
	public override int ManaAmount() => 110;
	public override int EnergyAmount() => 100;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(4.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_PumpkinPie_ModBuff>(), ModUtils.ToMinute(24));
	}
}
public class Roguelike_PumpkinPie_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.PumpkinPie;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().UpdateDefenseBase.Base += 9;
		player.endurance += .07f;
		player.GetModPlayer<Roguelike_PumpkinPie_ModPlayer>().PumpkinPie = true;
	}
}
public class Roguelike_PumpkinPie_ModPlayer : ModPlayer {
	public bool PumpkinPie = false;
	public int Cooldown = 0;
	public override void ResetEffects() {
		PumpkinPie = false;
		Cooldown = ModUtils.CountDown(Cooldown);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		ExplosionEffect();
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		ExplosionEffect();
	}
	private void ExplosionEffect() {
		if (!PumpkinPie || Cooldown <= 0) {
			return;
		}
		Cooldown = ModUtils.ToSecond(5);
		Player.Center.LookForHostileNPC(out List<NPC> npclist, 300);
		int damage = (int)((2.5f + Player.endurance) * Player.statDefense.Positive);
		foreach (var target in npclist) {
			Player.StrikeNPCDirect(target, target.CalculateHitInfo(damage, ModUtils.DirectionFromPlayerToNPC(Player.Center.X, target.Center.X)));
		}
		for (int i = 0; i < 120; i++) {
			Dust dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.Pumpkin);
			dust.velocity = Main.rand.NextVector2Circular(30, 30);
			dust.scale = Main.rand.NextFloat(0.9f, 1.34f);
		}
	}
}
