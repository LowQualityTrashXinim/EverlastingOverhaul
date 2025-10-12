using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_RedRyder : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.RedRyder;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 20;
		entity.useTime = entity.useAnimation = 15;
		entity.shootSpeed = 12;
		entity.UseSound = SoundID.Item38 with { Pitch = 1 };
	}
	public override bool CanUseItem(Item item, Player player) {
		if (player.altFunctionUse == 2) {
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
		}
		else {
			item.noUseGraphic = false;
			item.UseSound = SoundID.Item38 with { Pitch = 1 };
		}
		return player.ownedProjectileCounts[ModContent.ProjectileType<Roguelike_RedRyder_ModProjectile>()] < 1;
	}
	public override bool AltFunctionUse(Item item, Player player) => player.GetModPlayer<Roguelike_RedRyder_ModPlayer>().CanAltClick;
	public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (player.altFunctionUse == 2) {
			type = ModContent.ProjectileType<Roguelike_RedRyder_ModProjectile>();
		}
		else {
			if (type == ProjectileID.Bullet) {
				type = ProjectileID.BulletHighVelocity;
			}
		}
		if (!player.GetModPlayer<Roguelike_RedRyder_ModPlayer>().CanAltClick) {
			damage *= 3;
		}
	}
	public override void UseStyle(Item item, Player player, Rectangle heldItemFrame) {
		if (player.altFunctionUse == 2) {
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
		}
		else {
			item.noUseGraphic = false;
			item.UseSound = SoundID.Item38 with { Pitch = 1 };
		}
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_RedRyder_Buff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_RedRyder_ModPlayer>().CanAltClick = false;
	}
}
public class Roguelike_RedRyder_ModPlayer : ModPlayer {
	public int Timer = 0;
	public bool CanAltClick = true;
	public override void ResetEffects() {
		if (++Timer >= 180) {
			Timer = 180;
		}
		CanAltClick = true;
	}
	int projectileWhoAmI = -1;
	public override void PreUpdateMovement() {
		if (Player.HeldItem.type != ItemID.RedRyder) {
			return;
		}
		int projType = ModContent.ProjectileType<Roguelike_RedRyder_ModProjectile>();
		if (Player.ownedProjectileCounts[projType] < 1 || !Main.mouseRight || Player.ItemAnimationActive) {
			projectileWhoAmI = -1;
			return;
		}
		if (projectileWhoAmI == -1) {
			foreach (var proj in Main.ActiveProjectiles) {
				if (proj.type == projType) {
					projectileWhoAmI = proj.whoAmI;
				}
			}
		}
		else {
			Projectile projectile = Main.projectile[projectileWhoAmI];
			if (projectile.active && projectile.timeLeft > 0) {
				Player.velocity = (projectile.Center - Player.Center).SafeNormalize(Vector2.Zero) * 20;
				Player.stairFall = true;
			}
		}
	}
}
public class Roguelike_RedRyder_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.RedRyder);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 14;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = ModUtils.ToMinute(5);
		Projectile.penetrate = -1;
	}
	public override bool? CanDamage() {
		return Projectile.ai[0] == 0;
	}
	public override bool PreAI() {
		Player player = Main.player[Projectile.owner];
		if (player.ownedProjectileCounts[ModContent.ProjectileType<Roguelike_RedRyder_ModProjectile>()] > 1) {
			Projectile.Kill();
			return false;
		}
		return base.PreAI();
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		Projectile.rotation += Projectile.velocity.ToRotation() * .45f;
		if (!player.ItemAnimationActive &&
			Projectile.Center.IsCloseToPosition(player.Center, 36) || !Projectile.Center.IsCloseToPosition(player.Center, 2000)) {
			player.AddBuff<Roguelike_RedRyder_Buff>(ModUtils.ToSecond(5));
			Projectile.Kill();
		}
		if (Main.mouseLeftRelease) {
			Projectile.ai[0] = 0;
		}
		if (!player.ItemAnimationActive && Main.mouseRight || Projectile.ai[0] == 1) {
			Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * .5f;
			Projectile.ai[0] = 1;
		}
		else {
			if (++Projectile.ai[1] < 30) {
				return;
			}
			if (Projectile.velocity.Y < 16) {
				Projectile.velocity.Y += .5f;
			}
			Projectile.velocity *= .99f;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.velocity = (Projectile.Center - target.Center).SafeNormalize(Vector2.Zero) * Projectile.velocity.Length() * .9f;
	}
	public override bool PreDraw(ref Color lightColor) {
		Player player = Main.player[Projectile.owner];
		if (!player.ItemAnimationActive && Main.mouseRight) {
			Texture2D tex = ModContent.Request<Texture2D>(ModTexture.WHITEDOT).Value;
			Vector2 distance = player.Center - Projectile.Center;
			float length = distance.Length();
			for (int i = 0; i < length; i++) {
				Vector2 currentPos = Vector2.Lerp(Projectile.Center, player.Center, i / length) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
				Main.EntitySpriteDraw(tex, currentPos, null, Color.Red.ScaleRGB(.2f) with { A = 0 }, distance.ToRotation() + MathHelper.PiOver2, tex.Size() * .5f, 1, SpriteEffects.None);
			}
		}
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 drawpos = Projectile.position - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, texture.Size() * .5f, 1, SpriteEffects.None);
		return false;
	}
}
