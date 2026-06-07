using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_DragonFruit : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Dragonfruit;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(7);
	public override int LifeAmount() => 45;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_DragonFruit_ModBuff>(), ModUtils.ToMinute(11));
	}
}
public class Roguelike_DragonFruit_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.Dragonfruit;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateHPMax.Base += 60;
		handler.UpdateDefenseBase.Base += 7;
	}
}
public class Roguelike_DragonFruit_ModPlayer : ModPlayer {
	public bool DragonFruit = false;
	public override void ResetEffects() {
		DragonFruit = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (DragonFruit && Player.wingTime > 0 && Player.wingTime != Player.wingTimeMax && Main.rand.NextBool(10)) {
			target.AddBuff(BuffID.WitheredArmor, ModUtils.ToSecond(Main.rand.Next(4, 9)));
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (DragonFruit && Player.wingTime > 0 && Player.wingTime != Player.wingTimeMax && Main.rand.NextBool(10)) {
			target.AddBuff(BuffID.WitheredArmor, ModUtils.ToSecond(Main.rand.Next(4, 9)));
		}
	}
}
