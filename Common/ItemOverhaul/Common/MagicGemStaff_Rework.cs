using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using EverlastingOverhaul.Contents.Projectiles;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
public class Roguelike_MagicGemStaff : GlobalItem {
	public override void SetStaticDefaults() {
		ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.AmethystStaff] = 0;
		ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.TopazStaff] = 0;
		ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.SapphireStaff] = 0;
		ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.EmeraldStaff] = 0;
		ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.RubyStaff] = 0;
		ItemID.Sets.BonusAttackSpeedMultiplier[ItemID.DiamondStaff] = 0;
		ItemID.Sets.ItemsThatAllowRepeatedRightClick[ItemID.AmethystStaff] = true;
		ItemID.Sets.ItemsThatAllowRepeatedRightClick[ItemID.TopazStaff] = true;
		ItemID.Sets.ItemsThatAllowRepeatedRightClick[ItemID.SapphireStaff] = true;
		ItemID.Sets.ItemsThatAllowRepeatedRightClick[ItemID.EmeraldStaff] = true;
		ItemID.Sets.ItemsThatAllowRepeatedRightClick[ItemID.RubyStaff] = true;
		ItemID.Sets.ItemsThatAllowRepeatedRightClick[ItemID.DiamondStaff] = true;
	}
	public override void SetDefaults(Item entity) {
		switch (entity.type) {
			case ItemID.AmethystStaff:
				entity.shoot = ModContent.ProjectileType<AmethystMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 15;
				entity.reuseDelay = 30;
				entity.mana = 25;
				entity.shootSpeed = 1;
				entity.damage += 5;
				break;
			case ItemID.TopazStaff:
				entity.shoot = ModContent.ProjectileType<TopazMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 18;
				entity.reuseDelay = 33;
				entity.mana = 26;
				entity.shootSpeed = 1;
				entity.damage += 5;
				break;
			case ItemID.SapphireStaff:
				entity.shoot = ModContent.ProjectileType<SapphireMagicalBolt>();
				entity.useTime = 3;
				entity.useAnimation = 18;
				entity.mana = 12;
				entity.damage -= 3;
				entity.shootSpeed = 1;
				break;
			case ItemID.EmeraldStaff:
				entity.shoot = ModContent.ProjectileType<EmeraldMagicalBolt>();
				entity.useTime = 10;
				entity.useAnimation = 20;
				entity.mana = 16;
				entity.shootSpeed = 1;
				break;
			case ItemID.RubyStaff:
				entity.damage += 5;
				entity.shoot = ModContent.ProjectileType<RubyMagicalBolt>();
				entity.shootSpeed = 1;
				entity.mana = 40;
				break;
			case ItemID.DiamondStaff:
				entity.damage += 5;
				entity.shoot = ModContent.ProjectileType<DiamondMagicalBolt>();
				entity.shootSpeed = 4;
				entity.mana = 40;
				break;
		}
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		switch (item.type) {
			case ItemID.AmethystStaff:
			case ItemID.TopazStaff:
			case ItemID.SapphireStaff:
			case ItemID.EmeraldStaff:
			case ItemID.RubyStaff:
			case ItemID.DiamondStaff:
				ModUtils.AddTooltip(ref tooltips, new(Mod, "Staff", ModUtils.LocalizationText("RoguelikeRework", "GemStaff")));
				break;
		}
	}
	public override bool AltFunctionUse(Item item, Player player) {
		switch (item.type) {
			case ItemID.AmethystStaff:
			case ItemID.TopazStaff:
			case ItemID.SapphireStaff:
			case ItemID.EmeraldStaff:
			case ItemID.RubyStaff:
			case ItemID.DiamondStaff:
				return true;
		}
		return base.AltFunctionUse(item, player);
	}
	public override float UseTimeMultiplier(Item item, Player player) {
		if (player.altFunctionUse != 2) {
			float speedMulti = 1;
			switch (item.type) {
				case ItemID.AmethystStaff:
				case ItemID.TopazStaff:
				case ItemID.SapphireStaff:
				case ItemID.EmeraldStaff:
				case ItemID.RubyStaff:
				case ItemID.DiamondStaff:
					speedMulti = item.useAnimation / (float)item.useTime;
					item.reuseDelay = 0;
					return speedMulti;
			}
		}
		else {
			switch (item.type) {
				case ItemID.AmethystStaff:
					item.reuseDelay = 30;
					break;
				case ItemID.TopazStaff:
					item.reuseDelay = 33;
					break;
			}
		}
		return base.UseTimeMultiplier(item, player);
	}
	public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult) {
		switch (item.type) {
			case ItemID.AmethystStaff:
			case ItemID.TopazStaff:
			case ItemID.SapphireStaff:
			case ItemID.EmeraldStaff:
			case ItemID.RubyStaff:
			case ItemID.DiamondStaff:
				if (player.altFunctionUse != 2) {
					mult = .2f;
				}
				break;
		}
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (player.altFunctionUse != 2) {
			switch (item.type) {
				case ItemID.AmethystStaff:
					type = ProjectileID.AmethystBolt;
					if (!velocity.IsLimitReached(5)) {
						velocity = velocity.SafeNormalize(Vector2.Zero) * 5;
					}
					break;
				case ItemID.TopazStaff:
					type = ProjectileID.TopazBolt;
					if (!velocity.IsLimitReached(5)) {
						velocity = velocity.SafeNormalize(Vector2.Zero) * 5;
					}
					break;
				case ItemID.SapphireStaff:
					type = ProjectileID.SapphireBolt;
					if (!velocity.IsLimitReached(7)) {
						velocity = velocity.SafeNormalize(Vector2.Zero) * 7;
					}
					break;
				case ItemID.EmeraldStaff:
					type = ProjectileID.EmeraldBolt;
					if (!velocity.IsLimitReached(7)) {
						velocity = velocity.SafeNormalize(Vector2.Zero) * 7;
					}
					break;
				case ItemID.DiamondStaff:
					type = ProjectileID.DiamondBolt;
					if (!velocity.IsLimitReached(9)) {
						velocity = velocity.SafeNormalize(Vector2.Zero) * 9;
					}
					break;
				case ItemID.RubyStaff:
					type = ProjectileID.RubyBolt;
					if (!velocity.IsLimitReached(9)) {
						velocity = velocity.SafeNormalize(Vector2.Zero) * 9;
					}
					break;
			}
		}
		else {
			switch (item.type) {
				case ItemID.AmethystStaff:
					velocity = velocity.Vector2RotateByRandom(10);
					position = position.PositionOFFSET(velocity, 50);
					break;
				case ItemID.TopazStaff:
					velocity = velocity.Vector2RotateByRandom(15) * Main.rand.NextFloat(.75f, 1.25f);
					position = position.PositionOFFSET(velocity, 50);
					break;
				case ItemID.SapphireStaff:
					velocity = velocity.Vector2RotateByRandom(6) * Main.rand.NextFloat(.75f, 1.25f);
					position = position.PositionOFFSET(velocity, 50);
					break;
				case ItemID.EmeraldStaff:
					velocity *= Main.rand.NextFloat(1, 1.5f);
					position = position.PositionOFFSET(velocity, 50);
					break;
				case ItemID.DiamondStaff:
				case ItemID.RubyStaff:
					position = position.PositionOFFSET(velocity, 50);
					damage *= 2;
					break;
			}
		}
	}
}
