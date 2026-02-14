using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
namespace EverlastingOverhaul.Contents.Items.NoneSynergy.RectangleShotgun {
	class RectangleShotgun : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(12, 74, 50, 4f, 10, 10, ItemUseStyleID.Shoot, ModContent.ProjectileType<RectangleBullet>(), 100f, true, AmmoID.Bullet);
			Item.value = Item.buyPrice(gold: 50);
			Item.rare = ItemRarityID.LightRed;
			Item.reuseDelay = 30;
			Item.UseSound = SoundID.Item38;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			type = ModContent.ProjectileType<RectangleBullet>();
			position = position.PositionOFFSET(velocity, 40);
			velocity *= .1f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			for (int i = 0; i < Main.rand.Next(3, 5); i++) {
				Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(40), type, damage, knockback, player.whoAmI);
			}
			return false;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-19, 0);
		}
	}
	class RectangleBullet : ModProjectile {
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.width = 70;
			Projectile.height = 18;
			Projectile.light = 0.7f;
			Projectile.timeLeft = 400;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
		}
		public override void AI() {
			if (Projectile.ai[0] == 0) {
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.ai[0]++;
			}
			Projectile.velocity *= .97f;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			var Direction = Projectile.rotation.ToRotationVector2() * 35;
			var Head = Projectile.Center + Direction;
			var End = Projectile.Center - Direction;
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Head, End, 22, ref point);
		}
	}
}
