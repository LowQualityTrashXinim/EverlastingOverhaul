using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_Revolver : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Revolver;
	}
	public override bool InstancePerEntity => true;
	public override void SetDefaults(Item entity) {
		entity.damage += 20;
		entity.crit += 10;
		entity.useTime = entity.useAnimation = 16;
	}
	public int Counter = 6;
	public bool Alt_Shoot = false;
	public int PowerUp = 0;
	public override bool CanUseItem(Item item, Player player) {
		if (--Counter <= 0) {
			Counter = 7;
		}
		if (player.altFunctionUse == 2) {
			if (!Main.rand.NextBool(Counter) && Counter > 1) {
				item.UseSound = SoundID.Item98 with { Pitch = 1f };
				PowerUp++;
				Alt_Shoot = false;
			}
			else {
				PowerUp = 0;
				Counter = 7;
				item.UseSound = SoundID.Item41;
				Alt_Shoot = true;
			}
			item.noUseGraphic = true;
			return base.CanUseItem(item, player);
		}
		if (Counter <= 1) {
			item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
		}
		else {
			item.UseSound = SoundID.Item41;
			item.noUseGraphic = false;
		}
		return base.CanUseItem(item, player);
	}
	public override bool? UseItem(Item item, Player player) {
		if (player.ItemAnimationJustStarted) {
			if (player.altFunctionUse == 2) {
				return base.UseItem(item, player);
			}
			if (Counter <= 1) {
				item.UseSound = SoundID.Item1;
				item.noUseGraphic = true;
			}
			else {
				item.UseSound = SoundID.Item41;
				item.noUseGraphic = false;
			}
		}
		return base.UseItem(item, player);
	}
	public override bool AltFunctionUse(Item item, Player player) {
		return true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (PowerUp > 0) {
			damage = damage + (int)(damage * PowerUp * 1.5f);
		}
		if (type == ProjectileID.Bullet)
			type = ProjectileID.BulletHighVelocity;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (player.altFunctionUse == 2) {
			if (Alt_Shoot) {
				Alt_Shoot = false;
				Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<EverlastingOverhaul_Revovler_Fail>(), 1, 1, player.whoAmI);
			}
			return false;
		}
		else {
			PowerUp = 0;
		}
		if (Counter <= 1) {
			Counter = 0;
			var proj = Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero) * 20, ModContent.ProjectileType<EverlastingOverhaul_Revolver_ModProjectile>(), damage, knockback, player.whoAmI);
			proj.rotation = MathHelper.ToRadians(Main.rand.NextFloat(360));
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class EverlastingOverhaul_Revovler_Fail : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.hide = true;
		Projectile.timeLeft = 2;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.penetrate = -1;
	}
	public override void AI() {
		Projectile.Center = Main.player[Projectile.owner].Center;
		Projectile.velocity = Vector2.Zero;
	}
	public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) {
		modifiers.SetMaxDamage(1);
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info) {
		if (target.statLife - 200 <= 1) {
			target.statLife = 2;
		}
		else {
			target.statLife -= 200;
		}
	}
}
public class EverlastingOverhaul_Revolver_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Revolver);
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 10;
		ProjectileID.Sets.TrailingMode[Type] = 2;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 30;
		Projectile.tileCollide = true;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.timeLeft = 900;
		Projectile.extraUpdates = 1;
	}
	bool OnHitEnemy => Projectile.ai[2] == 1;
	public override void AI() {
		Projectile.rotation += MathHelper.ToRadians(1 * Projectile.velocity.Length());
		if (++Projectile.ai[1] >= 30) {
			if (Projectile.velocity.Y <= 20) {
				Projectile.velocity.Y += .25f;
			}
		}
		Projectile.velocity.X *= .99f;
		if (OnHitEnemy && Projectile.velocity.IsLimitReached(1)) {
			if (++Projectile.ai[0] >= 16) {
				Projectile.ai[0] = 0;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().SafeNormalize(Vector2.Zero) * 10, ProjectileID.BulletHighVelocity, Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.ai[2] = 1;
		Projectile.ai[1] = 60;
		if (!Projectile.velocity.IsLimitReached(.1f)) {
			Projectile.velocity = Vector2.Zero;
			return false;
		}
		Projectile.velocity.Y = -oldVelocity.Y;
		Projectile.velocity.Y *= .7f;
		if (Projectile.velocity.X == 0) {
			Projectile.velocity.X = -oldVelocity.X;
		}
		return false;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.ai[2] = 1;
		Projectile.ai[1] = 60;
		if (Projectile.damage <= 5) {
			Projectile.damage = 5;
		}
		else {
			Projectile.damage = (int)(Projectile.damage * .95f);
		}
		Projectile.velocity = (Projectile.Center - target.Center).SafeNormalize(Vector2.Zero).Vector2RotateByRandom(60) * Main.rand.NextFloat(7, 10);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadItem(ItemID.Revolver);
		Texture2D texture = TextureAssets.Item[ItemID.Revolver].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.position - Main.screenPosition + origin;
		Projectile.DrawTrail(lightColor.ScaleRGB(.5f) with { A = 0 }, 0);
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
