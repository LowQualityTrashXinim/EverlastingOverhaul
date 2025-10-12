using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_Minishark : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Minishark;
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 50);
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int counter = ++player.GetModPlayer<Roguelike_Minishark_ModPlayer>().FreezyCounter;
		if (Main.rand.NextBool(6)) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(10) * Main.rand.NextFloat(.9f, 1), type, damage, knockback, player.whoAmI);
		}
		if (player.HasBuff<Freezy>()) {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(15) * Main.rand.NextFloat(.8f, 1), type, damage, knockback, player.whoAmI);
			if (Main.rand.NextBool(3)) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(20) * Main.rand.NextFloat(.7f, 1), type, damage, knockback, player.whoAmI);
			}
			if (Main.rand.NextBool(6)) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.7f, 1), type, damage, knockback, player.whoAmI);
			}
		}
		if (Main.rand.NextBool(100) || counter >= 200) {
			player.GetModPlayer<Roguelike_Minishark_ModPlayer>().FreezyCounter = 0;
			ModUtils.CombatTextRevamp(player.Hitbox, Color.Aquamarine, "Freezy!!");
			player.AddBuff<Freezy>(ModUtils.ToSecond(5));
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_Minishark", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	class Freezy : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			if (player.HeldItem.type == ItemID.Minishark) {
				player.ModPlayerStats().AttackSpeed += .5f;
			}
		}
	}
}
public class Roguelike_Minishark_ModPlayer : ModPlayer {
	public int FreezyCounter = 0;
}
