using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_PlatinumBow : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.PlatinumBow;
	}
	public override void SetDefaults(Item entity) {
		entity.useTime = entity.useAnimation = 42;
		entity.damage += 20;
		entity.shootSpeed += 3;
		entity.crit += 6;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, $"Roguelike_{item.Name}", string.Format(ModUtils.LocalizationText("RoguelikeRework", item.Name), new string[] { $"[i:{ItemID.DiamondStaff}]", $"[i:{ItemID.Starfury}]" })));
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (player.OldItemType() == ItemID.DiamondStaff) {
			for (int i = 0; i < 8; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(8, 40, i), ProjectileID.DiamondBolt, damage, knockback, player.whoAmI);
			}
			damage = (int)(damage * 1.5f);
		}
		if (player.OldItemType() == ItemID.Starfury) {
			float len = velocity.Length();
			for (int i = 0; i < 3; i++) {
				Vector2 positionAbove = position.Add(Main.rand.Next(0, 100) * player.direction, 1000);
				Vector2 vel = (Main.MouseWorld - positionAbove + Main.rand.NextVector2Circular(50, 20)).SafeNormalize(Vector2.Zero) * (20 + Main.rand.Next(10));
				Projectile.NewProjectile(source, positionAbove, vel, ProjectileID.Starfury, damage * 3, knockback, player.whoAmI);
			}
			for (int i = 0; i < 5; i++) {
				Vector2 positionAbove = position.Add(Main.rand.Next(0, 100) * player.direction, 1000);
				Vector2 vel = (Main.MouseWorld - positionAbove + Main.rand.NextVector2Circular(50, 20)).SafeNormalize(Vector2.Zero) * (len + Main.rand.NextFloat(-len * .25f, len * .25f));
				var arrow = Projectile.NewProjectileDirect(source, positionAbove, vel, type, damage, knockback, player.whoAmI);
				if (arrow.arrow) {
					arrow.extraUpdates += 1;
				}
			}
		}
		var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
		if (projectile.arrow) {
			projectile.extraUpdates += 1;
		}
		return false;
	}
}
