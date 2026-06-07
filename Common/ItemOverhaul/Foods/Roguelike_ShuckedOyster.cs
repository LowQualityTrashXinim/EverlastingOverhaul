using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_ShuckedOyster : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.ShuckedOyster;
	public override int LifeAmount() => 20;
	public override int ManaAmount() => 40;
	public override int CoolDownBetweenUse() => 120;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_ShuckedOyster_ModBuff>(), ModUtils.ToMinute(7));
	}
}
public class Roguelike_ShuckedOyster_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.ShuckedOyster;
	public override void Update(Player player, ref int buffIndex) {
		player.breathEffectiveness += 1;
		player.fishingSkill += 15;
		if (player.wet) {
			player.ModPlayerStats().UpdateMovement += .15f;
		}
		player.ModPlayerStats().UpdateDefenseBase.Base += 10;
	}
}
public class Roguelike_ShuckedOyster_ModPlayer : ModPlayer {
	public bool ShuckedOyster = false;
	public int CoolDown = 0;
	public int Duration = 0;
	public override void ResetEffects() {
		ShuckedOyster = false;
	}
	public override void UpdateEquips() {
		if (--Duration > 0) {
			Player.ModPlayerStats().UpdateDefenseBase.Base += 20;
		}
		else {
			Duration = 0;
			CoolDown = ModUtils.CountDown(CoolDown);
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (ShuckedOyster && CoolDown <= 0) {
			Duration = 120;
			CoolDown = 300;
		}
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (ShuckedOyster && CoolDown <= 0) {
			Duration = 120;
			CoolDown = 300;
		}
	}
}
