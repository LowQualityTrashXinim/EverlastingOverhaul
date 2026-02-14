using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
public class Roguelike_PhoenixBlaster : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.PhoenixBlaster;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 42;
		entity.useTime = entity.useAnimation = 30;
		entity.knockBack += 1;
		entity.crit = 6;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_PhoenixBlaster", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int Counter = player.GetModPlayer<Roguelike_PhoenixBlaster_ModPlayer>().PhoenixBlaster_Counter;
		if (++player.GetModPlayer<Roguelike_PhoenixBlaster_ModPlayer>().PhoenixBlaster_ShootCounter >= 5) {
			player.GetModPlayer<Roguelike_PhoenixBlaster_ModPlayer>().PhoenixBlaster_ShootCounter = 0;
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30), ProjectileID.Flamelash, (int)(damage * .34f), knockback, player.whoAmI);
		}
		if (Counter >= 90) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.DD2PhoenixBowShot, damage * 3, knockback * 3, player.whoAmI);
			Counter -= 90;
			Counter = Counter / 15;
			for (int i = 0; i < Counter; i++) {
				int proj = Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30), ProjectileID.Flamelash, (int)(damage * .34f), knockback, player.whoAmI);
				Main.projectile[proj].penetrate = 1;
			}
		}
		player.GetModPlayer<Roguelike_PhoenixBlaster_ModPlayer>().PhoenixBlaster_Counter = -player.itemAnimationMax;
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class Roguelike_PhoenixBlaster_ModPlayer : ModPlayer {
	public int PhoenixBlaster_Counter = 0;
	public int PhoenixBlaster_ShootCounter = 0;
	public bool PeakShot = false;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++PhoenixBlaster_Counter >= 150) {
			PhoenixBlaster_Counter = 150;
		}
		else {
			PeakShot = false;
		}
		if (Player.HeldItem.type != ItemID.PhoenixBlaster) {
			return;
		}
		if (PhoenixBlaster_Counter == 90) {
			for (int i = 0; i < 60; i++) {
				var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GoldFlame);
				dust.noGravity = true;
				dust.velocity = Main.rand.NextVector2CircularEdge(10, 10);
				dust.scale += 1 + Main.rand.NextFloat(.2f);
			}
		}
		if (!PeakShot && PhoenixBlaster_Counter == 150) {
			PeakShot = true;
			for (int i = 0; i < 120; i++) {
				var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GoldFlame);
				dust.noGravity = true;
				dust.velocity = Vector2.UnitX.Vector2DistributeEvenlyPlus(120, 360, i) * 10;
				dust.scale += 2 + Main.rand.NextFloat(.2f);
			}
			for (int i = 0; i < 10; i++) {
				for (int l = 0; l < 20; l++) {
					float scaler = 1 - l / 20f;
					var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.GoldFlame);
					dust.noGravity = true;
					dust.velocity = Vector2.UnitX.Vector2DistributeEvenlyPlus(10, 360, i) * (10 + 5 * (1 - scaler));
					dust.scale = 1.5f + 1 * scaler;
				}
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (proj.Check_ItemTypeSource(ItemID.PhoenixBlaster) && target.HasBuff(BuffID.OnFire) || target.HasBuff(BuffID.OnFire3)) {
			modifiers.FinalDamage.Base += 10;
		}
	}
}
