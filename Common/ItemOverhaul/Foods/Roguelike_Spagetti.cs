using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_Spagetti : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Spaghetti;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(7);
	public override int LifeAmount() {
		return 140 + Main.rand.Next(-30, 90);
	}
	public override int EnergyAmount() => 295;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.5f);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_Spagetti_Buff>(), ModUtils.ToMinute(37));
	}
}
public class EverlastingOverhaul_Spagetti_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Apple;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<EverlastingOverhaul_Spagetti_ModPlayer>().Spagetti = true;
	}
}
public class EverlastingOverhaul_Spagetti_ModPlayer : ModPlayer {
	public bool Spagetti = false;
	public int Timer = 0;
	public override void ResetEffects() {
		Spagetti = false;
		Timer = ModUtils.CountDown(Timer);
	}
	public override void UpdateEquips() {
		if (Spagetti) {
			if (Main.rand.NextBool(1000)) {
				Player.statLife = Player.statLifeMax2;
			}
			if (Main.rand.NextBool(1000)) {
				Player.immune = true;
				Player.AddImmuneTime(-1, 300);
			}
			if (Main.rand.NextBool(5000)) {
				Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<EverlastingOverhaul_Spagetti_Buff>()));
			}
		}
	}
	public override float UseTimeMultiplier(Item item) {
		if (Spagetti) {
			if (Main.rand.NextBool(1000)) {
				Timer = 600 + Main.rand.Next(1, 10) * 60;
			}
			if (Timer > 0) {
				return 4;
			}
		}
		return base.UseTimeMultiplier(item);
	}
}
