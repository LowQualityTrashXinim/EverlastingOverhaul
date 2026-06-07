using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_FroggieBunwich : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.FroggleBunwich;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(8);
	public override int EnergyAmount() => 270;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(4.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_FroggieBunwich_ModBuff>(), ModUtils.ToMinute(17));
	}
}
public class Roguelike_FroggieBunwich_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.FroggleBunwich;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.DodgeChance += .17f;
		handler.UpdateMovement += .35f;
		handler.UpdateJumpBoost += 1.25f;
		handler.CurrentDashType = "FroggieBunwich";

	}
}
public class Roguelike_FroggieBunwich_ModPlayer : ModPlayer {
	public bool FroggieBunwich = false;
	public override void ResetEffects() {
		FroggieBunwich = false;
		if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15) {
			DashDir = DashRight;
		}
		else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15) {
			DashDir = DashLeft;
		}
		else {
			DashDir = -1;
		}
	}
	public override void PreUpdateMovement() {
		if (CanUseDash() && DashDir != -1 && DashDelay == 0) {
			var newVelocity = Player.velocity;

			switch (DashDir) {
				case DashLeft when Player.velocity.X > -DashVelocity:
				case DashRight when Player.velocity.X < DashVelocity: {
						float dashDirection = DashDir == DashRight ? 1 : -1;
						newVelocity.X = dashDirection * DashVelocity;
						break;
					}
				default:
					return;
			}
			DashDelay = DashCooldown;
			DashTimer = DashDuration;
			Player.velocity = newVelocity;
		}

		if (DashDelay > 0)
			DashDelay--;

		if (DashTimer > 0) {
			Player.eocDash = DashTimer;
			Player.armorEffectDrawShadowEOCShield = true;
			DashTimer--;
		}
	}
	private bool CanUseDash() {
		return Player.CheckDashType("FroggieBunwich")
			&& !Player.setSolar
			&& !Player.mount.Active;
	}

	public const int DashRight = 2;
	public const int DashLeft = 3;

	public const int DashCooldown = 50;
	public const int DashDuration = 35;

	public const float DashVelocity = 10.5f;

	public int DashDir = -1;

	public int DashDelay = 0;
	public int DashTimer = 0;

}
