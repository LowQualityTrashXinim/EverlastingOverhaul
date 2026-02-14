using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.NoneSynergy;
internal class GoldComplexBow : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(34, 42, 16, 3f, 29, 29, ItemUseStyleID.Shoot, ProjectileID.WoodenArrowFriendly, 12f, true, AmmoID.Arrow);
		Item.UseSound = SoundID.Item5;
	}
	public override Vector2? HoldoutOffset() {
		return new(-1f, 0);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		for (int i = 0; i < 2; i++) {
			Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(20, 20), velocity * Main.rand.NextFloat(.8f,1f), type, damage, player.whoAmI);
		}
		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
}
