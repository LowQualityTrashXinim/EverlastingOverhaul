using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Consumable.Scroll;
internal class ScrollOfMagicMissile : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.Item_DefaultToConsume(32, 32, 30, 30);
		Item.DamageType = DamageClass.Magic;
		Item.damage = 90;
		Item.knockBack = 6;
		Item.maxStack = 99;
		Item.noUseGraphic = true;
		Item.noMelee = true;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			Vector2 limitedSpawningPosition = Main.MouseWorld;
			Vector2 distance = (limitedSpawningPosition - player.Center).SafeNormalize(Vector2.Zero);
			for (int i = 0; i < 4; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, distance.Vector2DistributeEvenlyPlus(4, 20, i) * 3, ProjectileID.MagicMissile, Item.damage, Item.knockBack, player.whoAmI);
				projectile.timeLeft = 180;
				projectile.tileCollide = false;
			}
		}
		return true;
	}
}
