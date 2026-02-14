using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.SkullRevolver;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.NoneSynergy;
internal class OldSkullRevolver : ModItem {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<SkullRevolver>();
	int counter = 0;
	public override void SetDefaults() {
		Item.BossRushDefaultRange(26, 52, 25, 3f, 10, 60, ItemUseStyleID.Shoot, ProjectileID.Bullet, 20f, false, AmmoID.Bullet);
		Item.rare = ItemRarityID.Orange;
		Item.UseSound = SoundID.Item11;
		Item.crit = 15;
		Item.reuseDelay = 57;
		Item.value = Item.buyPrice(gold: 50);
		Item.UseSound = SoundID.Item41;
	}
	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 50);
		velocity = ModUtils.RoguelikeSpread(velocity, 10);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		SoundEngine.PlaySound(Item.UseSound);
		counter++;
		if (counter == 2) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BookOfSkullsSkull, damage, knockback, player.whoAmI);
		}
		if (counter == 4) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.ClothiersCurse, damage, knockback, player.whoAmI);
			counter = 0;
		}
		return true;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-3, 0);
	}
}
