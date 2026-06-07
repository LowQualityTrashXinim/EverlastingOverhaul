using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_BBQRibs : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BBQRibs;
	public override int LifeAmount() => 200;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(8.5f);
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(4.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_BBQRibs_ModBuff>(), ModUtils.ToMinute(43));
	}
}
public class Roguelike_BBQRibs_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.BBQRibs;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_BBQRibs_ModPlayer>().BBQRibs = true;
		player.GetDamage(DamageClass.Melee) *= 1.2f;
	}
}
public class Roguelike_BBQRibs_ModPlayer : ModPlayer {
	public bool BBQRibs = false;
	public int Counter = 0;
	public int CoolDown = 0;
	public int DecayTimer = 0;
	public override void ResetEffects() {
		BBQRibs = false;
		CoolDown = ModUtils.CountDown(CoolDown);
		if (Counter > 0) {
			if (++DecayTimer >= 120) {
				Counter--;
				DecayTimer = 0;
			}
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		OnHitByEffect();
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitByEffect();
	}
	private void OnHitByEffect() {
		if (!BBQRibs || Main.rand.NextFloat() > .15f) {
			return;
		}
		Vector2 randPos = Player.Center + Main.rand.NextVector2CircularEdge(400, 400) * Main.rand.NextFloat(.5f, 1.2f);
		Item.NewItem(Player.GetSource_FromThis(), randPos, ItemID.Heart);
	}
	public override void UpdateEquips() {
		if (BBQRibs) {
			PlayerStatsHandle handler = Player.ModPlayerStats();
			handler.UpdateDefenseBase.Base += Counter;
			handler.UpdateHPRegen.Base += Counter * .25f;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!BBQRibs || CoolDown > 0) {
			return;
		}
		CoolDown = 60; DecayTimer = 0;
		if(Counter >= 50) {
			return;
		}
		Counter++;
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.minion || !BBQRibs || CoolDown > 0) {
			return;
		}
		CoolDown = 60; DecayTimer = 0;
		if (Counter >= 50) {
			return;
		}
		Counter++;
	}
}
