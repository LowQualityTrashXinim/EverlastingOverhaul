using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class Roguelike_TacticalShotgun : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.TacticalShotgun;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new TooltipLine(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Main.rand.NextBool()) {
			int amount = Main.rand.Next(1, 7);
			for (int i = 0; i < amount; i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(25) * Main.rand.NextFloat(.76f, 1.1f), type, damage, knockback, player.whoAmI);
			}
		}
		if (Main.rand.NextBool(4)) {
			if (player.itemAnimationMax < 15) {
				player.itemTime = (int)Math.Round(player.itemAnimationMax / 15f * 10);
				player.itemAnimation = player.itemTime;
			}
			else {
				player.itemAnimation = 10;
				player.itemTime = 10;
			}
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
