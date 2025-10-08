using EverlastingOverhaul.Common.Global;
using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using Roguelike.Contents.BuffAndDebuff;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
public class Roguelike_TheUndertaker : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.TheUndertaker;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 9;
		entity.useTime = entity.useAnimation = 30;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		position = position.PositionOFFSET(velocity, 30);
		float length = velocity.Length();
		for (int i = 0; i < 6; i++) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(5).Vector2RandomSpread(length * .75f, Main.rand.NextFloat(.35f, .76f)), type, damage, knockback, player.whoAmI);
		}
		return false;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_TheUndertaker_ModPlayer : ModPlayer {
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType == ItemID.TheUndertaker) {
			Player.Heal(1);
			target.AddBuff(ModContent.BuffType<CrimsonAbsorbtion>(), 240);
		}
	}
}
