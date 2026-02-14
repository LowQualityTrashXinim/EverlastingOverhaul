using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Consumable.Ammo;
internal class FragmentRound : ModItem {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Item.Item_DefaultToAmmo(32, 32, 7, 0, 1.9f, 10, ModContent.ProjectileType<FragmentRound_Projectile>(), AmmoID.Bullet);
		Item.DamageType = DamageClass.Ranged;
		Item.rare = ItemRarityID.Green;
		Item.value = 10;
	}
}
public class FragmentRound_Projectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.Bullet);
	public override void SetDefaults() {
		Projectile.height = Projectile.width = 4;
		Projectile.friendly = true;
		Projectile.timeLeft = 30;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
		Projectile.light = 0.5f;
		Projectile.extraUpdates = 1;
		Projectile.alpha = 255;
	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.timeLeft += Main.rand.Next(-10, 11);
	}
	public override void AI() {
		Projectile.alpha -= 20;
		if (Projectile.timeLeft <= 4) {
			int amount = Main.rand.Next(2, 4);
			for (int i = 0; i < amount; i++) {
				var bullet = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.Vector2RotateByRandom(30).Vector2RandomSpread(2, Main.rand.NextFloat(.8f, 1.1f)), ProjectileID.Bullet, (int)(Projectile.damage * .45f), Projectile.knockBack * .5f, Projectile.owner);
				bullet.scale -= .5f;
				bullet.Resize(2, 2);
				var smoke = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Scale: Main.rand.NextFloat(.95f, 1.25f));
				smoke.velocity = Main.rand.NextVector2Circular(2, 2);
				smoke.noGravity = true;
				var spark = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Scale: Main.rand.NextFloat(.75f, 1.25f));
				spark.velocity = Main.rand.NextVector2Circular(2, 2);
				spark.noGravity = true;
			}
			Projectile.Kill();
		}
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
	}
}
