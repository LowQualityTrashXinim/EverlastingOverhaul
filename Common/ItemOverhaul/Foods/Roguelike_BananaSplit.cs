using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_BananaSplit : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BananaSplit;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3);
	public override int LifeAmount() => 45;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_BananaSplit_ModBuff>(), ModUtils.ToMinute(19));
	}
}
public class Roguelike_BananaSplit_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.BananaSplit;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().NonCriticalDamage += .25f;
		player.GetModPlayer<Roguelike_BananaSplit_ModPlayer>().BananaSplit = true;
	}
}
public class Roguelike_BananaSplit_ModPlayer : ModPlayer {
	public bool BananaSplit = false;
	public int Counter = 0;
	public override void ResetEffects() {
		BananaSplit = false;
	}
	public override void UpdateEquips() {
		if (BananaSplit) {
			Player.ModPlayerStats().UpdateDefenseBase += 1.25f * Counter;
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (BananaSplit && Main.rand.NextFloat() <= .14f) {
			Player.ModPlayerStats().Request_ShootExtra(1, 10);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		OnHitEffect();
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitEffect();
	}
	private void OnHitEffect() {
		if (!BananaSplit) {
			return;
		}
		Counter = Math.Clamp(Counter++, 0, 40);
	}
}
