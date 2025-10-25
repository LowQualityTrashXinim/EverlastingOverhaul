using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Common;
public class Roguelike_ShortSword : GlobalItem {
	public bool ShortSwordCheck(int type) =>
		type == ItemID.CopperShortsword
			|| type == ItemID.TinShortsword
			|| type == ItemID.IronShortsword
			|| type == ItemID.LeadShortsword
			|| type == ItemID.SilverShortsword
			|| type == ItemID.TungstenShortsword
			|| type == ItemID.GoldShortsword
			|| type == ItemID.PlatinumShortsword;
	public override void SetDefaults(Item entity) {
		if (ShortSwordCheck(entity.type)) {
			entity.damage = 14;
			entity.crit += 21;
			entity.useTime = entity.useAnimation = 6;
			entity.Set_ItemCriticalDamage(3f);
		}
	}
	public override bool AltFunctionUse(Item item, Player player) {
		if (ShortSwordCheck(item.type))
			return true;
		return base.AltFunctionUse(item, player);
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (ShortSwordCheck(item.type)) {
			if (!player.HasBuff<ThrowShortSwordCoolDown>() && player.altFunctionUse == 2) {
				Projectile.NewProjectile(source, position, velocity * 9, ModContent.ProjectileType<Special_ThrowShortSwordProjectile>(), damage, knockback, player.whoAmI, ai2: item.type);
				player.AddBuff(ModContent.BuffType<ThrowShortSwordCoolDown>(), 120);
			}
			else {
				Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(15, 15), velocity.Vector2RotateByRandom(5), ModContent.ProjectileType<RoguelikeOverhaul_ShortSwordProjectile>(), damage, knockback, player.whoAmI, ai2: item.type);
			}
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		if (ShortSwordCheck(item.type))
			ModUtils.AddTooltip(ref tooltips, new TooltipLine(Mod, "RogueLike_ShortSword", ModUtils.LocalizationText("RoguelikeRework", "ShortSword")));
	}
	class RoguelikeOverhaul_ShortSwordProjectile : ModProjectile {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.TinShortsword);
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 36;
			Projectile.friendly = true;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 3;
		}
		int MaxProgression = 50;
		int progression = 0;
		Vector2 spawnPosition = Vector2.Zero;
		float length = 0;
		bool HitEnemey = false;
		public override bool? CanDamage() {
			return progression > 0 && !HitEnemey;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
		public override void AI() {
			if (Projectile.timeLeft == 300) {
				MaxProgression = 10;
				progression = MaxProgression;
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
				spawnPosition = Projectile.Center;
				length = Math.Clamp((Main.MouseWorld - Projectile.Center).Length(), 0, 110);
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (progression <= 0 || HitEnemey)
				if (Projectile.timeLeft > MaxProgression)
					Projectile.timeLeft = MaxProgression;
			float progress = (MaxProgression - progression) / (float)MaxProgression;
			Projectile.Center = spawnPosition + Vector2.SmoothStep(Projectile.velocity, Projectile.velocity * length, progress);
			if (Projectile.spriteDirection == -1) {
				Projectile.rotation += MathHelper.PiOver4;
			}
			else {
				Projectile.rotation += MathHelper.PiOver4;
			}
			if (!HitEnemey) {
				progression--;
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			HitEnemey = true;
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			if (Projectile.timeLeft <= 10) {
				lightColor.A = (byte)MathHelper.Lerp(0, 255, Projectile.timeLeft / 10f);
				lightColor = lightColor.ScaleRGB(Projectile.timeLeft / 10f);
			}
			Texture2D texture = ModContent.Request<Texture2D>(ModUtils.GetVanillaTexture<Item>((int)Projectile.ai[2])).Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
	class Special_ThrowShortSwordProjectile : ModProjectile {
		public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.CopperShortsword);
		public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 150;
			Projectile.aiStyle = 2;
			Projectile.tileCollide = true;
		}

		public override void AI() {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			modifiers.SetCrit();
			modifiers.CritDamage += 2;
			target.AddBuff<Special_Vulnerable>(180);
		}
		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = ModContent.Request<Texture2D>(ModUtils.GetVanillaTexture<Item>((int)Projectile.ai[2])).Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
	class ThrowShortSwordCoolDown : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
		}
	}
	class Special_Vulnerable : ModBuff {
		public override string Texture => ModTexture.EMPTYBUFF;
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultDeBuff();
		}
		public override void Update(NPC npc, ref int buffIndex) {
			npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense -= .35f;
			npc.lifeRegen -= 5;
		}
	}
}
