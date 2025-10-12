using EverlastingOverhaul.Common.Projectiles;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
public class Roguelike_WoodenBow : GlobalItem {
	public override void SetDefaults(Item entity) {
		if (entity.type == ItemID.WoodenBow) {
			entity.shootSpeed += 3;
			entity.crit += 6;
			entity.damage += 5;
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (item.type == ItemID.WoodenBow) {
			int Counter = player.GetModPlayer<Roguelike_WoodenBow_ModPlayer>().Counter;
			player.GetModPlayer<Roguelike_WoodenBow_ModPlayer>().Counter = 0;
			if (Counter >= 90) {
				Counter -= 90;
				int amount = Counter / 10 + 3;
				for (int i = 0; i < amount; i++) {
					var pos = Main.MouseWorld + Main.rand.NextVector2CircularEdge(2000, 700);
					var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 5;
					Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<WindShot>(), (int)(damage * .55f), 5f, player.whoAmI);
				}
			}
			if (Main.rand.NextFloat() <= .3f) {
				var pos = Main.MouseWorld + Main.rand.NextVector2CircularEdge(2000, 700);
				var vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 5;
				Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<WindShot>(), (int)(damage * .55f), 5f, player.whoAmI);
			}
			var newPos1 = position.IgnoreTilePositionOFFSET(velocity.RotatedBy(MathHelper.PiOver2), 5);
			var newVelocity1 = (Main.MouseWorld - newPos1).SafeNormalize(Vector2.Zero) * velocity.Length();
			var newPos2 = position.IgnoreTilePositionOFFSET(velocity.RotatedBy(-MathHelper.PiOver2), 5);
			var newVelocity2 = (Main.MouseWorld - newPos2).SafeNormalize(Vector2.Zero) * velocity.Length();
			var arrow1 = Projectile.NewProjectileDirect(source, newPos1, newVelocity1, type, damage, knockback, player.whoAmI);
			var arrow2 = Projectile.NewProjectileDirect(source, newPos2, newVelocity2, type, damage, knockback, player.whoAmI);
			if (ContentSamples.ProjectilesByType[type].arrow) {
				arrow1.extraUpdates += 1;
				arrow2.extraUpdates += 1;
			}
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (item.type == ItemID.WoodenBow) {
			ModUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_WoodenBow", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
		}
	}
}
public class Roguelike_WoodenBow_ModPlayer : ModPlayer {
	public int Counter = 0;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++Counter >= 150) {
			Counter = 150;
		}
	}
}
