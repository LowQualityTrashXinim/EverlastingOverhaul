using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.NoneSynergy;
internal class MasterWoodBow : ModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(34, 42, 16, 3f, 29, 29, ItemUseStyleID.Shoot, ProjectileID.WoodenArrowFriendly, 12f, true, AmmoID.Arrow);
		Item.UseSound = SoundID.Item5;
	}
	public override Vector2? HoldoutOffset() {
		return new(-5f, 0);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		for (int i = 0; i < 4; i++) {
			Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(4, 60, i), ProjectileID.WoodenArrowFriendly, damage, knockback, player.whoAmI);
		}
		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
}
