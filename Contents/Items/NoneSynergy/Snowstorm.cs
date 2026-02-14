using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.NoneSynergy;
internal class Snowstorm : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(78, 24, 22, 1f, 40, 40, ItemUseStyleID.Shoot, 1, 8f, true, AmmoID.Gel);
		Item.UseSound = SoundID.Item42;
	}
	public override Vector2? HoldoutOffset() {
		return new(-17f, 0);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		var proj = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<FlameProjectile>(), damage, knockback, player.whoAmI, 0, 0);
		if (proj.ModProjectile is FlameProjectile flame) {
			flame.FlameColor = new Color(100, 255, 255, 0);
			flame.DebuffType = BuffID.Frostburn;
		}
		return false;
	}
}
