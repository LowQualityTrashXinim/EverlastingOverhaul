using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_BunnyStew : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BunnyStew;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(6.5f);
	public override int EnergyAmount() => 75;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = 180;
		SetBuff(item, ModContent.BuffType<Roguelike_BunnyStew_ModBuff>(), ModUtils.ToMinute(7));
	}
}
public class Roguelike_BunnyStew_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.BunnyStew;
	public override void Update(Player player, ref int buffIndex) {
		player.pickSpeed += .23f;
		player.tileSpeed += .23f;
		player.GetJumpState<Roguelike_BunnyStew_ExtraJump>().Enable();
		player.ModPlayerStats().UpdateMovement += .23f;
		player.ModPlayerStats().CurrentDashType = "BunnyStew";
	}
}
public class Roguelike_BunnyStew_ModPlayer : ModPlayer {
	public bool BunnyStew = false;
	public override void ResetEffects() {
		BunnyStew = false;
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
		return Player.CheckDashType("BunnyStew")
			&& !Player.setSolar
			&& !Player.mount.Active;
	}

	public const int DashRight = 2;
	public const int DashLeft = 3;

	public const int DashCooldown = 50;
	public const int DashDuration = 35;

	public const float DashVelocity = 7.5f;

	public int DashDir = -1;

	public int DashDelay = 0;
	public int DashTimer = 0;
}
public class Roguelike_BunnyStew_ExtraJump : ExtraJump {
	public override Position GetDefaultPosition() => new After(BlizzardInABottle);

	public override float GetDurationMultiplier(Player player) {
		return 1f;
	}

	public override void UpdateHorizontalSpeeds(Player player) {
		player.runAcceleration *= 2.2f;
		player.maxRunSpeed *= 3f;
	}

	public override void OnStarted(Player player, ref bool playSound) {
		// Use this hook to trigger effects that should appear at the start of the extra jump
		// This example mimics the logic for spawning the puff of smoke from the Cloud in a Bottle
		int offsetY = player.height;
		if (player.gravDir == -1f)
			offsetY = 0;

		offsetY -= 16;

		SpawnCloudPoof(player, player.Top + new Vector2(-16f, offsetY));
		SpawnCloudPoof(player, player.position + new Vector2(-36f, offsetY));
		SpawnCloudPoof(player, player.TopRight + new Vector2(4f, offsetY));
	}

	private static void SpawnCloudPoof(Player player, Vector2 position) {
		var gore = Gore.NewGoreDirect(player.GetSource_FromThis(), position, -player.velocity, Main.rand.Next(11, 14));
		gore.velocity.X = gore.velocity.X * 0.1f - player.velocity.X * 0.1f;
		gore.velocity.Y = gore.velocity.Y * 0.1f - player.velocity.Y * 0.05f;
	}

	public override void ShowVisuals(Player player) {
		// Use this hook to trigger effects that should appear throughout the duration of the extra jump
		// This example mimics the logic for spawning the dust from the Blizzard in a Bottle
		int offsetY = player.height - 6;
		if (player.gravDir == -1f)
			offsetY = 6;

		var spawnPos = new Vector2(player.position.X, player.position.Y + offsetY);

		for (int i = 0; i < 2; i++) {
			SpawnBlizzardDust(player, spawnPos, 0.1f, i == 0 ? -0.07f : -0.13f);
		}
	}

	private static void SpawnBlizzardDust(Player player, Vector2 spawnPos, float dustVelocityMultiplier, float playerVelocityMultiplier) {
		var dust = Dust.NewDustDirect(spawnPos, player.width, 12, DustID.Snow, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, newColor: Color.White);
		dust.fadeIn = 1.5f;
		dust.velocity *= dustVelocityMultiplier;
		dust.velocity += player.velocity * playerVelocityMultiplier;
		dust.noGravity = true;
		dust.noLight = true;
	}
}
