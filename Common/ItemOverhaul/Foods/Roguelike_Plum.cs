using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Plum : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Plum;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3.75f);
	public override int LifeAmount() => 41;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_Plum_ModBuff>(), ModUtils.ToMinute(6));
	}
}
public class Roguelike_Plum_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.Plum;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_Plum_ModPlayer>().Plum = true;
	}
}
public class Roguelike_Plum_ModPlayer : ModPlayer {
	public bool Plum = false;
	public override void ResetEffects() {
		Plum = false;
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		OnHitffect();
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitffect();
	}
	private void OnStrikeEffect() {
		if (!Plum) {
			return;
		}
		if (Main.rand.NextFloat() <= .07f) {
			for (int i = 0; i < Player.buffType.Length; i++) {
				if (Main.debuff[Player.buffType[i]]) {
					Player.buffTime[i] -= 150;
				}
				else {
					Player.buffTime[i] += 60;
				}
			}
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		OnStrikeEffect();
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		OnStrikeEffect();
	}
	private void OnHitffect() {
		if (!Plum) {
			return;
		}
		if (Main.rand.NextBool()) {
			for (int i = 0; i < Player.buffType.Length; i++) {
				if (Main.debuff[Player.buffType[i]]) {
					Player.buffTime[i] -= 150;
				}
				else {
					Player.buffTime[i] += 60;
				}
			}
		}
	}
}
