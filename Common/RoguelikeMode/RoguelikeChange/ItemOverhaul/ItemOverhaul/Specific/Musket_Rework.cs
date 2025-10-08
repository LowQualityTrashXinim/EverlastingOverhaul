using EverlastingOverhaul.Common.Global;
using Microsoft.Xna.Framework;
using Roguelike.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
public class Roguelike_Musket : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Musket;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 120;
		entity.useTime = entity.useAnimation = 60;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int timer = player.GetModPlayer<Roguelike_Musket_ModPlayer>().Timer;
		Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
		player.GetModPlayer<Roguelike_Musket_ModPlayer>().Timer = -player.itemAnimationMax;
		if (timer >= 60) {
			proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().SetCrit++;
			if (timer >= 180) {
				proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().CritDamage += 1.5f;
			}
		}
		return false;
	}
}
public class Roguelike_Musket_ModPlayer : ModPlayer {
	public int Timer = 0;
	public override void ResetEffects() {
		if (++Timer >= 180) {
			Timer = 180;
		}
		if (Player.active && !Player.dead && Player.HeldItem.type == ItemID.Musket) {
			if (Timer == 60) {
				SoundEngine.PlaySound(SoundID.Item102 with { Pitch = 1 });
			}
		}
	}
}
