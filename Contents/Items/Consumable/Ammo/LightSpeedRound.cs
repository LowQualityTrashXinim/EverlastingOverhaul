using Terraria;
using Terraria.ID;
using EverlastingOverhaul.Texture;
using Terraria.ModLoader;
using EverlastingOverhaul.Contents.Projectiles;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Contents.Items.Consumable.Ammo;
internal class LightSpeedRound : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.Item_DefaultToAmmo(8, 8, 9, 16, 1.5f, 1, ModContent.ProjectileType<HitScanBullet>(), AmmoID.Bullet);
		Item.DamageType = DamageClass.Ranged;
		Item.rare = ItemRarityID.Green;
		Item.value = 10;
	}
}
