using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_SpicyPepper : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.SpicyPepper;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(15);
	public override int EnergyAmount() => 120;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_SpicyPepper_Buff>(), ModUtils.ToMinute(4));
	}
}
public class Roguelike_SpicyPepper_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.SpicyPepper;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.MeleeAtkSpeed += .13f;
		handler.AttackSpeed += .09f;
		handler.UpdateDefenseBase.Base -= 7;
		player.GetModPlayer<Roguelike_SpicyPepper_ModPlayer>().SpicyPepper = true;
	}
}
public class Roguelike_SpicyPepper_ModPlayer : ModPlayer {
	public bool SpicyPepper = false;
	public override void ResetEffects() {
		SpicyPepper = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!SpicyPepper) {
			return;
		}
		if (Main.hardMode) {
			target.AddBuff(BuffID.OnFire3, ModUtils.ToSecond(Main.rand.Next(4, 9)));
		}
		else {
			target.AddBuff(BuffID.OnFire, ModUtils.ToSecond(Main.rand.Next(4, 9)));
		}
	}
}
