using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_Megaphone : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.Megaphone;
	}
	public override void UpdateEquip(Item item, Player player) {
		player.GetModPlayer<EverlastingOverhaul_Megaphone_ModPlayer>().Megaphone = true;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
}
public class EverlastingOverhaul_Megaphone_ModPlayer : ModPlayer {
	public bool Megaphone = false;
	int usetime = -1;
	int count = 0;
	public override void ResetEffects() {
		Megaphone = false;
	}
	public override void PostUpdate() {
		if (!Megaphone) {
			return;
		}
		if (usetime <= 0 && Player.ItemAnimationJustStarted) {
			if (++count >= 10) {
				usetime = 20;
				count = 0;
			}
		}
		if (usetime > 0) {
			if (usetime % 4 == 0) {
				SpawnSoundWave2();
			}
			usetime--;
			count = 0;
		}
		if (Player.ModPlayerStats().HasDodgeInThisInstance) {
			SpawnSoundWave();
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (!Megaphone) {
			return;
		}
		SpawnSoundWave();
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		if (!Megaphone) {
			return;
		}
		SpawnSoundWave();
	}
	private void SpawnSoundWave() {
		int damage = 100 + (int)(Player.GetWeaponDamage(Player.HeldItem) * .45f);
		Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero,
			ModContent.ProjectileType<EverlastingOverhaul_Megaphone_Projectile_SoundWave>(), damage, 5f, Player.whoAmI);
	}
	private void SpawnSoundWave2() {
		int damage = 50;
		Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center,
			(Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero) * 4,
			ModContent.ProjectileType<EverlastingOverhaul_Megaphone_Projectile_SoundWave2>(), damage, 5f, Player.whoAmI);
	}
}
public class EverlastingOverhaul_Megaphone_Projectile_SoundWave : ModProjectile {
	public override string Texture => ModTexture.OuterInnerGlow;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 132;
		Projectile.friendly = true;
		Projectile.timeLeft = 120;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.alpha = 255;
	}
	public override void AI() {
		Projectile.alpha -= 10;
		if (Projectile.alpha <= 0) {
			Projectile.alpha = 0;
			Projectile.Kill();
		}
		Projectile.scale += .2f;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.Knockback += 10;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor with { A = (byte)Projectile.alpha }, 0, origin, Projectile.scale, SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
public class EverlastingOverhaul_Megaphone_Projectile_SoundWave2 : ModProjectile {
	public override string Texture => ModTexture.OuterInnerGlow;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 1000;
		Projectile.tileCollide = true;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 40;
		Projectile.extraUpdates = 5;
	}
	public override void AI() {
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + new Vector2(Projectile.velocity.X, 0), 1, 1)) {
			Projectile.velocity.X = -oldVelocity.X;
		}
		if (Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + new Vector2(0, Projectile.velocity.Y), 1, 1)) {
			Projectile.velocity.Y = -oldVelocity.Y;
		}
		return false;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;

		ModUtils.Draw_SetUpToDrawGlowAdditive(Main.spriteBatch);
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, new Vector2(1, .25f) * Projectile.scale, SpriteEffects.None);
		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
