using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class Roguelike_Revolver : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Revolver;
	}
	public override void SetDefaults(Item entity) {
		entity.damage += 20;
		entity.crit += 10;
		entity.useTime = entity.useAnimation = 16;
	}
	public override bool CanUseItem(Item item, Player player) {
		var modplayer = player.GetModPlayer<Roguelike_Revolver_ModPlayer>();
		if (modplayer.Counter >= 5) {
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
			var modplayer = player.GetModPlayer<Roguelike_Revolver_ModPlayer>();
			if (modplayer.Counter >= 5) {
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
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (type == ProjectileID.Bullet)
			type = ProjectileID.BulletHighVelocity;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		var modplayer = player.GetModPlayer<Roguelike_Revolver_ModPlayer>();
		if (++modplayer.Counter >= 6) {
			modplayer.Counter = 0;
			var proj = Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero) * 20, ModContent.ProjectileType<Roguelike_Revolver_ModProjectile>(), damage, knockback, player.whoAmI);
			proj.rotation = MathHelper.ToRadians(Main.rand.NextFloat(360));
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class Roguelike_Revolver_ModPlayer : ModPlayer {
	public int Counter = 0;
}
public class Roguelike_Revolver_ModProjectile : ModProjectile {
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
		Projectile.rotation += Projectile.velocity.ToRotation() * .1f;
		if (++Projectile.ai[1] >= 30) {
			if (Projectile.velocity.Y <= 20) {
				Projectile.velocity.Y += .25f;
			}
		}
		if (OnHitEnemy) {
			if (++Projectile.ai[0] >= 16) {
				Projectile.ai[0] = 0;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.rotation.ToRotationVector2().SafeNormalize(Vector2.Zero) * 10, ProjectileID.BulletHighVelocity, Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.ai[2] = 1;
		Projectile.ai[1] = 60;
		Projectile.velocity.Y = -oldVelocity.Y;
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
