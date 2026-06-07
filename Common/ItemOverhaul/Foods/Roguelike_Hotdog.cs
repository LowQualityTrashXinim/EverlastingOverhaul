using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Hotdog : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Hotdog;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(6);
	public override int LifeAmount() => 200;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Hotdog_ModBuff>(), ModUtils.ToMinute(38));
	}
}
public class Roguelike_Hotdog_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.Hotdog;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().TrueDamage += .1f;
		player.GetModPlayer<Roguelike_Hotdog_ModPlayer>().Hotdog = true;
	}
}
public class Roguelike_Hotdog_ModPlayer : ModPlayer {
	public bool Hotdog = false;
	public override void ResetEffects() {
		Hotdog = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!Hotdog) {
			return;
		}
		Strike(target, hit);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.minion || !proj.Check_ItemTypeSource(Player.HeldItem.type) || !Hotdog) {
			return;
		}
		Strike(target, hit);
	}
	private void Strike(NPC target, NPC.HitInfo info) {
		if (Main.rand.NextFloat() <= .05f) {
			int damage = (int)(Player.GetWeaponDamage(Player.HeldItem) * .2f);
			info.DamageType = Roguelike_DamageClass.Pure;
			info.Damage = damage;
			info.Knockback *= .2f;
			Player.StrikeNPCDirect(target, info);
		}
	}
}
