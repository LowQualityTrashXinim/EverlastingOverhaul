using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class Roguelike_VenusMagnum : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.VenusMagnum;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 52;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new TooltipLine(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		int counter = player.GetModPlayer<Roguelike_VenusMagnum_ModPlayer>().VenusMagnum_Counter;
		if (player.GetModPlayer<Roguelike_CobaltSword_ModPlayer>().PerfectStrike) {
			counter = 300;
		}
		if (counter >= 150) {
			damage += 40;
		}
		if (counter >= 300) {
			damage *= 3;
		}
	}
	public override void HoldItem(Item item, Player player) {
		if (WeaponEffect_ModPlayer.Check_ValidForIntroEffect(player) && player.Check_SwitchedWeapon(item.type)) {
			WeaponEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(9));
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int counter = player.GetModPlayer<Roguelike_VenusMagnum_ModPlayer>().VenusMagnum_Counter;
		if (player.GetModPlayer<Roguelike_VenusMagnum_ModPlayer>().PerfectShot) {
			counter = 300;
		}
		if (counter >= 300) {
			int amount = Main.rand.Next(10, 30);
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.56f, 1f), ProjectileID.ChlorophyteBullet, (int)(damage * .33f) + 10, knockback, player.whoAmI);
			}
		}
		if (counter >= 150) {
			int amount = (counter - 150) / 10 + 1;
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.56f, 1f), ProjectileID.SporeCloud, (int)(damage * .43f) + 10, knockback, player.whoAmI);
			}
		}
		if (WeaponEffect_ModPlayer.Check_IntroEffect(player, item.type)) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.56f, 1f), ProjectileID.ChlorophyteBullet, (int)(damage * .33f) + 10, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.56f, 1f), ProjectileID.SporeCloud, (int)(damage * .43f) + 10, knockback, player.whoAmI);
		}
		player.GetModPlayer<Roguelike_VenusMagnum_ModPlayer>().VenusMagnum_Counter = -player.itemAnimationMax;
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class Roguelike_VenusMagnum_ModPlayer : ModPlayer {
	public int VenusMagnum_Counter = 0;
	public bool PerfectShot = false;
	public bool Effect_ReachCap = false;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (Player.ItemAnimationActive) {
			Effect_ReachCap = false;
		}
		var item = Player.HeldItem;
		VenusMagnum_Counter++;
		if (item.type != ItemID.VenusMagnum) {
			return;
		}
		if (VenusMagnum_Counter > 300) {
			VenusMagnum_Counter = 300;
			if (!Effect_ReachCap) {
				Effect_ReachCap = true;
				SpawnSpecialCapEffect();
			}
		}
		PerfectShot = VenusMagnum_Counter >= 150 && VenusMagnum_Counter <= 175;
		if (PerfectShot && VenusMagnum_Counter == 150) {
			SpawnSpecialCobaltDustEffect();
		}
	}
	public void SpawnSpecialCapEffect() {
		for (int o = 0; o < 100; o++) {
			float scale = Main.rand.NextFloat(.76f, 1.1f);
			int dust = Dust.NewDust(Player.Center, 0, 0, DustID.GemDiamond, 0, 0, 0, Color.Green, scale);
			Main.dust[dust].velocity = Main.rand.NextVector2CircularEdge(5, 5);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].Dust_GetDust().FollowEntity = true;
			Main.dust[dust].Dust_BelongTo(Player);
		}
	}
	public void SpawnSpecialCobaltDustEffect() {
		var GunSoundStyle = new SoundStyle("EverlastingOverhaul/Assets/SFX/ReloadGun");
		SoundEngine.PlaySound(GunSoundStyle with { Pitch = 1f }, Player.Center); 
		for (int o = 0; o < 10; o++) {
			for (int i = 0; i < 4; i++) {
				var Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * (3 + Main.rand.NextFloat()) * 5;
				for (int l = 0; l < 8; l++) {
					float multiplier = Main.rand.NextFloat();
					float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
					int dust = Dust.NewDust(Player.Center.Add(0, -60), 0, 0, DustID.GemDiamond, 0, 0, 0, Color.Green, scale);
					Main.dust[dust].velocity = Toward * multiplier;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].Dust_GetDust().FollowEntity = true;
					Main.dust[dust].Dust_BelongTo(Player);
				}
			}
		}
	}
}
