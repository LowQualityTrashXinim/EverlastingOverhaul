using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;

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
				Vector2 toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 50;
				for (int i = 0; i < 6; i++) {
					int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
					if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj) {
						woodproj.ItemIDtextureValue = item.type;
						woodproj.rotationSwing = 150 + i * 60;
						woodproj.directionLooking = directionOfLooking * (swingCount % 2 == 0 ? -1 : 1);
						woodproj.Set_TimeLeft = 60;
						woodproj.Set_AnimationTimeEnd = 30;
					}
					Main.projectile[proj].ai[2] = 120;
				}
			}
			else {
				Vector2 toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 50;
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
		int direction = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, Main.MouseWorld.X);
		for (int i = 0; i < 40; i++) {
			Vector2 pos = new Vector2(player.Center.X + (50 * i + 100) * direction, player.Center.Y - 1000 - 100 * i);
			Vector2 vel = Vector2.UnitY.Vector2RotateByRandom(5) * 20;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), damage, knockback, player.whoAmI, 1);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 spear) {
				spear.ItemIDtextureValue = item.type;
			}
			if (item.type == ItemID.AshWoodSword) {
				Main.projectile[projec].scale += 2;
				Main.projectile[projec].damage *= 2;
				if (i == 10) {
					int proj2 = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), damage * 5, knockback, player.whoAmI, 1);
					if (Main.projectile[proj2].ModProjectile is SwordProjectile2 spear2) {
						spear2.ItemIDtextureValue = item.type;
					}
					Main.projectile[proj2].scale += 10;
				}
			}
		}
	}
}
