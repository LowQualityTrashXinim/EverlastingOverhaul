using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_CoinGun : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.CoinGun;
	public override void SetDefaults(Item entity) {
		entity.useTime = entity.useAnimation = 4;
	}
}
public class EverlastingOverhaul_CoinGun_ModPlayer : ModPlayer {
	public int ShootCustom = 0;
	public int Count = 0;
	public Vector2 randomPos = Vector2.Zero;
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if(item.type != ItemID.CoinGun) {
			return base.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		if (ShootCustom == 0) {
			if (++Count > 100) {
				ShootCustom = Main.rand.Next(1, 4);
			}
		}
		if (ShootCustom == 1) {
			Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(3, 15, 0), type, damage, knockback, Player.whoAmI);
			Projectile.NewProjectile(source, position, velocity.Vector2DistributeEvenlyPlus(3, 15, 2), type, damage, knockback, Player.whoAmI);
			Count -= 5;
		}
		if (ShootCustom == 3) {
			if (randomPos == Vector2.Zero) {
				randomPos = position + Main.rand.NextVector2CircularEdge(100, 100);
			}
			Vector2 vel = (Main.MouseWorld - randomPos).SafeNormalize(Vector2.Zero);
			ModUtils.DustStar(randomPos, DustID.GemDiamond, Color.White, 8, 4, 0, 2.5f);
			Projectile.NewProjectile(source, randomPos, vel * velocity.Length(), type, damage, knockback, Player.whoAmI);
			Count -= 4;
		}
		if (Count <= 0) {
			Count = 0;
			ShootCustom = 0;
			randomPos = Vector2.Zero;
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(ItemID.CoinGun)) {
			if (ShootCustom == 2) {
				Vector2 pos = target.Center + Main.rand.NextVector2CircularEdge(target.width + 300, target.height + 300);
				Projectile.NewProjectile(proj.GetSource_FromAI(), pos, (target.Center - pos).SafeNormalize(Vector2.Zero) * proj.velocity.Length(), proj.type, proj.damage, proj.knockBack, Player.whoAmI);
				Count -= 2;
			}
		}
	}
}
