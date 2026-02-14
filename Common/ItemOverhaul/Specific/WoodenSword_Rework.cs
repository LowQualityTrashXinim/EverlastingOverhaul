using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;

public class Roguelike_WoodenSword : GlobalItem {
	public override bool InstancePerEntity => true;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.WoodenSword;
	}
	int swingCount = 0;
	public override void SetDefaults(Item entity) {
		entity.scale += .45f;
		entity.damage = 33;
		entity.GetGlobalItem<MeleeWeaponOverhaul>().ShaderOffSetLength += 1;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void HoldItem(Item item, Player player) {
		if (player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive) {
			if (++swingCount >= 30) {
				swingCount = 0;
			}
			int directionOfLooking = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, Main.MouseWorld.X);
			if (swingCount % 5 == 0)
				WoodSwordAttack(item, player);
			if (swingCount % 3 == 0) {
				var toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 50;
				for (int i = 0; i < 6; i++) {
					int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
					if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj) {
						woodproj.ItemIDtextureValue = item.type;
						woodproj.rotationSwing = 150 + i * 60;
						woodproj.directionLooking = directionOfLooking * (swingCount % 2 == 0 ? -1 : 1);
						woodproj.Set_TimeLeft = 60;
						woodproj.Set_AnimationTimeEnd = 30;
					}
					Main.projectile[proj].usesLocalNPCImmunity = true;
					Main.projectile[proj].localNPCHitCooldown = 5;
					Main.projectile[proj].ai[2] = 120;
				}
			}
			else {
				var toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 50;
				int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
				if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj) {
					woodproj.directionLooking = directionOfLooking * (swingCount % 2 == 0 ? -1 : 1);
					woodproj.ItemIDtextureValue = item.type;
				}
				Main.projectile[proj].ai[2] = 120;
			}
		}
	}
	private void WoodSwordAttack(Item item, Player player) {
		int damage = player.GetWeaponDamage(item);
		float knockback = player.GetWeaponKnockback(item);
		for (int i = 0; i < 40; i++) {
			var pos = new Vector2(Main.MouseWorld.X + Main.rand.NextFloat(-100, 100), player.Center.Y - 1000 - 100 * i);
			var vel = Vector2.UnitY.Vector2RotateByRandom(5) * 20;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), damage, knockback, player.whoAmI, 1);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 spear) {
				spear.ItemIDtextureValue = item.type;
			}
		}
	}
}
