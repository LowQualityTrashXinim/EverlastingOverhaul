using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_RoastedDuck : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.RoastedDuck;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(17);
	public override int LifeAmount() => 125;
	public override int ManaAmount() => 215;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(10);
		SetBuff(item, ModContent.BuffType<Roguelike_RoastedDuck_ModBuff>(), ModUtils.ToMinute(31));
	}
}
public class Roguelike_RoastedDuck_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.RoastedDuck;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.WingUpTime += .75f;
		if (player.wet || player.Check_IsPlayerFlying()) {
			handler.DirectItemDamage *= 1.65f;
			handler.DodgeChance += .2f;
			player.GetModPlayer<Roguelike_RoastedDuck_ModPlayer>().RoastedDuck = true;
		}
	}
}
public class Roguelike_RoastedDuck_ModPlayer : ModPlayer {
	public bool RoastedDuck = false;
	public override void ResetEffects() {
		RoastedDuck = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (RoastedDuck) {
			target.AddBuff(BuffID.CursedInferno, ModUtils.ToSecond(1));
		}
	}
}
