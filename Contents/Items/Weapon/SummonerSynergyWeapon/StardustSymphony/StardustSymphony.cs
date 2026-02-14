using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.SummonerSynergyWeapon.StardustSymphony;
internal class StardustSymphony : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		DataStorer.AddContext("Synergy_StardustSymphony", new(700, Vector2.Zero, false, Color.Cyan));
	}
	public override void SetDefaults() {
		Item.BossRushSetDefault(32, 32, 44, 2f, 22, 22, ItemUseStyleID.Shoot, true);
		Item.DamageType = Roguelike_DamageClass.Summon;
		Item.crit = 4;
		Item.shoot = ModContent.ProjectileType<StardustSymphony_Projectile>();
		Item.shootSpeed = 1f;
		Item.mana = 10;
		Item.noUseGraphic = true;
		Item.noMelee = true;
	}
	public override bool AltFunctionUse(Player player) => true;
	public int Counter = 0;
	public int UseCounter = 0;
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
		if (UseCounter == 88) {
			Dust dust;
			for (int i = 0; i < 23; i++) {
				dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
				dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
				dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
				dust.color = Color.Cyan;
				dust.noGravity = true;
			}
			for (int i = 0; i < 53; i++) {
				dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
				dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
				dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
				dust.color = Color.Cyan;
				dust.noGravity = true;
			}
			dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
			dust.scale = Main.rand.NextFloat(.7f, 1.2f);
			dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		if (player.altFunctionUse == 2) {
			if (UseCounter >= 89) {
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(player.Center + Vector2.One.Vector2DistributeEvenlyPlus(100, 360, i) * 750, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
					dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
					dust.velocity = Vector2.Zero;
					dust.color = Color.Cyan;
					dust.noGravity = true;
				}
				player.AddBuff<StardustSymphony_Buff>(ModUtils.ToSecond(40));
				UseCounter = 0;
			}
			if (!player.HasBuff<StardustSymphony_CoolDown>()) {
				player.AddBuff<StardustSymphony_CoolDown>(ModUtils.ToSecond(15));
				player.Heal(100);
				for (int i = 0; i < 5; i++) {
					Vector2 pos = position + Main.rand.NextVector2Circular(50, 50);
					Vector2 vel = (pos - player.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(.4f, .9f);
					Projectile.NewProjectile(source, pos, vel, ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), damage, knockback, player.whoAmI);
				}
			}
		}
		else {
			SoundEngine.PlaySound(SoundID.Item9 with { PitchRange = (.5f, 1f), Volume = .5f });
			if (Counter >= 15) {
				Counter = 0;
				for (int i = 0; i < 5; i++) {
					Projectile.NewProjectile(source, position + Vector2.One.Vector2DistributeEvenly(5, 360, i) * 900, Vector2.Zero, ModContent.ProjectileType<StardustSymphony_StarProjectile_Special>(), damage, knockback, player.whoAmI);
				}
			}
			int countB = Counter % 3;
			for (int i = 0; i < countB; i++) {
				Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(50, 50), velocity, type, damage, knockback, player.whoAmI);
			}
			Counter++;
			UseCounter++;
		}
		for (int i = 0; i < 3; i++) {
			Dust dust = Dust.NewDustDirect(player.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(1.4f, 1.8f);
			dust.velocity = velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.9f, 1.5f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.AbigailsFlower)
			.AddIngredient(ItemID.FairyQueenMagicItem)
			.Register();
	}
}
public class StardustSymphony_Dust : ModDust {
	public override string Texture => ModTexture.Glow_Small;
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .001f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return base.Update(dust);
	}
	public override bool PreDraw(Dust dust) {
		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		dust.color.A = 200;
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
public class StardustSymphony_DustBig : ModDust {
	public override string Texture => ModTexture.Glow_Big;
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .001f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return base.Update(dust);
	}
	public override bool PreDraw(Dust dust) {
		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		dust.color.A = 200;
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, Color.White, dust.rotation, texture.Size() * .5f, dust.scale * .75f, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
public class StardustSymphony_DustStar : ModDust {
	public override string Texture => ModTexture.Glow_Small;
	public override bool Update(Dust dust) {
		dust.velocity *= .98f;
		dust.scale -= .01f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return base.Update(dust);
	}
	public override bool PreDraw(Dust dust) {
		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
		Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);

		Texture2D texture3 = ModContent.Request<Texture2D>(ModTexture.FOURSTAR).Value;
		Main.spriteBatch.Draw(texture3, dust.position - Main.screenPosition, null, dust.color, dust.rotation, texture3.Size() * .5f, dust.scale, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture3, dust.position - Main.screenPosition, null, Color.White with { A = 0 }, dust.rotation, texture3.Size() * .5f, dust.scale * .75f, SpriteEffects.None, 0);
		return false;
	}
}
public class StardustSymphony_ModPlayer : ModPlayer {
	public Vector2 StardustSymphonyField_Pos = Vector2.Zero;
	public int HealDelay = 0;
	public int Dust_Counter = 0;
	public override void ResetEffects() {
		if (Player.HasBuff<StardustSymphony_Buff>()) {
			if (StardustSymphonyField_Pos == Vector2.Zero) {
				StardustSymphonyField_Pos = Player.Center;
			}
			DataStorer.ActivateContext(StardustSymphonyField_Pos, "Synergy_StardustSymphony");
			if (Player.Center.IsCloseToPosition(StardustSymphonyField_Pos, 700f)) {
				if (++HealDelay >= 60) {
					HealDelay = 0;
					Player.ManaHeal(45 + (int)(Player.statManaMax2 * .05f));
					Player.Heal(34 + (int)(Player.statLifeMax2 * .05f));
				}
			}
			Dust dust;
			Dust_Counter = ModUtils.Safe_SwitchValue(Dust_Counter, 100);
			dust = Dust.NewDustDirect(StardustSymphonyField_Pos + Vector2.UnitY.Vector2DistributeEvenlyPlus(100, 360, Dust_Counter) * 700, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = 1;
			dust.velocity = Vector2.Zero;
			dust.color = Color.Cyan;
			dust.noGravity = true;
			dust = Dust.NewDustDirect(StardustSymphonyField_Pos + Vector2.UnitY.Vector2DistributeEvenlyPlus(100, 360, -Dust_Counter) * 700, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = 1;
			dust.velocity = Vector2.Zero;
			dust.color = Color.Cyan;
			dust.noGravity = true;
			dust = Dust.NewDustDirect(StardustSymphonyField_Pos + Main.rand.NextVector2Circular(650, 650), 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.fadeIn = 1;
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		else {
			StardustSymphonyField_Pos = Vector2.Zero;
			DataStorer.DeActivateContext("Synergy_StardustSymphony");
		}
	}
}
public class StardustSymphony_Buff : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		if (player.Center.IsCloseToPosition(player.GetModPlayer<StardustSymphony_ModPlayer>().StardustSymphonyField_Pos, 700f)) {
			player.ModPlayerStats().UpdateCritDamage += .45f;
			player.GetCritChance(DamageClass.Generic) += 15;

		}
	}
}
public class StardustSymphony_CoolDown : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
//Trying out a possible new optimization method
public class StardustSymphony_ModSystem : ModSystem {
	public static NPC closestNPCinRange = null;
	public override void PreUpdateProjectiles() {
		Vector2 pos = Main.LocalPlayer.Center;
		pos.LookForHostileNPC(out NPC npc, 1500);
		closestNPCinRange = npc;
	}
}
public class StardustSymphony_Projectile : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(20);
		Projectile.extraUpdates = 20;
	}
	Vector2 mousePos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		mousePos = Main.MouseWorld;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		Projectile.velocity = (mousePos - Projectile.Center).SafeNormalize(Vector2.Zero) * 5;
		if (Projectile.Center.IsCloseToPosition(mousePos, 10) || !Projectile.Center.IsCloseToPosition(player.Center, 450)) {
			Projectile.Kill();
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Player player = Main.player[Projectile.owner];
		player.Heal(Main.rand.Next(5, 26) + (int)(Projectile.damage * .005f));

		Vector2 pos = player.Center + Main.rand.NextVector2Circular(50, 50);
		Vector2 vel = (pos - player.Center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(.4f, .6f);
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), pos, vel, ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), hit.Damage, hit.Knockback, player.whoAmI);
	}
	public override void OnKill(int timeLeft) {
		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
	public override bool PreDraw(ref Color lightColor) {
		return false;
	}
}
public abstract class StardustSymphony_StarProjectile_Base : ModProjectile {
	public override string Texture => ModTexture.FOURSTAR;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void AI() {
		Projectile.rotation += MathHelper.ToRadians(1);
		if (Main.rand.NextBool(30)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(.7f, 1.2f);
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(90) * 2;
			dust.color = Color.White;
			dust.noGravity = true;
		}

		if (++Projectile.ai[0] <= 600) {
			Projectile.velocity *= .998f;
			return;
		}
		if (StardustSymphony_ModSystem.closestNPCinRange != null) {
			Projectile.timeLeft = 120;
			Vector2 pos = StardustSymphony_ModSystem.closestNPCinRange.Center;
			Projectile.ai[1] *= 1.01f;
			if (Projectile.ai[1] > 2) {
				Projectile.ai[1] = 2;
			}
			Projectile.velocity = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
			Projectile.ai[1] += .01f;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Player player = Main.player[Projectile.owner];
		player.Heal(Main.rand.Next(5, 26) + (int)(Projectile.damage * .005f));

		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
	public override bool PreDraw(ref Color lightColor) {
		Vector2 originOfThisProj = new Vector2(23, 23);
		Texture2D texture = ModContent.Request<Texture2D>(ModTexture.SMALLWHITEBALL).Value;
		Texture2D textureDot = ModContent.Request<Texture2D>(ModTexture.WHITEDOT).Value;
		Vector2 origin = texture.Size() * .5f;
		Vector2 originDot = textureDot.Size() * .5f;
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + originOfThisProj;
			Main.EntitySpriteDraw(texture, drawPos, null, lightColor with { A = 0 }, Projectile.rotation, origin, (Projectile.scale - k * .01f) * .75f, SpriteEffects.None, 0);

			Main.EntitySpriteDraw(textureDot, drawPos, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(textureDot, drawPos + Vector2.One.RotatedBy(MathHelper.PiOver2) * 5, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale * .5f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(textureDot, drawPos + Vector2.One.RotatedBy(MathHelper.Pi) * 5, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale * .5f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(textureDot, drawPos + Vector2.One.RotatedBy(MathHelper.Pi + MathHelper.PiOver2) * 5, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale * .5f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(textureDot, drawPos + Vector2.One.RotatedBy(MathHelper.TwoPi) * 5, null, Color.Cyan, Projectile.rotation, originDot, Projectile.scale * .5f, SpriteEffects.None, 0);
		}

		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture2 = ModContent.Request<Texture2D>(ModTexture.Glow_Big).Value;
		Main.spriteBatch.Draw(texture2, Projectile.position - Main.screenPosition + originOfThisProj, null, lightColor with { A = 0 }, 0, texture2.Size() * .5f, Projectile.scale * 2, SpriteEffects.None, 0);

		Texture2D texture3 = ModContent.Request<Texture2D>(ModTexture.Glow_Medium).Value;
		Main.spriteBatch.Draw(texture3, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.Cyan with { A = 150 }, 0, texture3.Size() * .5f, Projectile.scale * 2, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);

		Texture2D textureStar = ModContent.Request<Texture2D>(Texture).Value;
		Main.spriteBatch.Draw(textureStar, Projectile.position - Main.screenPosition + originOfThisProj, null, lightColor with { A = 0 }, Projectile.rotation, textureStar.Size() * .5f, Projectile.scale * .5f, SpriteEffects.None, 0);

		Texture2D textureStarLar = ModContent.Request<Texture2D>(Texture).Value;
		Main.spriteBatch.Draw(textureStarLar, Projectile.position - Main.screenPosition + originOfThisProj, null, Color.Cyan.ScaleRGB(.3f) with { A = 0 }, Projectile.rotation, textureStarLar.Size() * .5f, Projectile.scale, SpriteEffects.None, 0);

		return false;
	}
}
public class StardustSymphony_StarProjectile_Normal : StardustSymphony_StarProjectile_Base {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(30);
		Projectile.extraUpdates = 10;
	}
	public override void AI() {
		if (++Projectile.ai[2] >= 360) {
			Projectile.ai[2] = 0;
			SoundEngine.PlaySound(SoundID.Item8 with { PitchRange = (-1f, -.9f) });
		}
		Projectile.rotation += MathHelper.ToRadians(1);
		if (Projectile.timeLeft <= 1000) {
			Projectile.scale *= .9985f;
		}
		if (Main.rand.NextBool(30)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(.7f, 1.2f) * Projectile.scale;
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(90) * 2;
			dust.color = Color.White;
			dust.noGravity = true;
		}
		Projectile.velocity *= .998f;
		if (++Projectile.ai[0] <= 600) {
			return;
		}
		if (StardustSymphony_ModSystem.closestNPCinRange != null) {
			Projectile.timeLeft = 120;
			Vector2 pos = StardustSymphony_ModSystem.closestNPCinRange.Center;
			Projectile.ai[1] *= 1.01f;
			if (Projectile.ai[1] > 2) {
				Projectile.ai[1] = 2;
			}
			Projectile.velocity = (pos - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
			Projectile.ai[1] += .01f;
		}
	}
	public override void OnKill(int timeLeft) {
		SoundEngine.PlaySound(SoundID.Item105 with { PitchRange = (.5f, 1f) });
		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
}
public class StardustSymphony_StarProjectile_Special : StardustSymphony_StarProjectile_Base {
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = ModUtils.ToSecond(100);
		Projectile.extraUpdates = 10;
		Projectile.scale += .5f;
	}
	Vector2 mouseLastPos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		mouseLastPos = Main.MouseWorld;
	}
	public override void AI() {
		if (++Projectile.ai[2] >= 360) {
			Projectile.ai[2] = 0;
			SoundEngine.PlaySound(SoundID.Item8 with { PitchRange = (-1f, -.9f) });
		}
		Projectile.rotation += MathHelper.ToRadians(1);
		if (Main.rand.NextBool(30)) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_Dust>());
			dust.scale = Main.rand.NextFloat(.7f, 1.2f);
			dust.velocity = -Projectile.velocity.Vector2RotateByRandom(90) * 2;
			dust.color = Color.White;
			dust.noGravity = true;
		}
		Projectile.velocity *= .998f;
		Vector2 dis = mouseLastPos - Projectile.Center;
		Projectile.velocity = dis.SafeNormalize(Vector2.Zero) * dis.Length() / 128f;
		Projectile.velocity = Projectile.velocity.LimitedVelocity(2);
		if (Projectile.Center.IsCloseToPosition(mouseLastPos, 5)) {
			Projectile.Kill();
		}
	}
	public override void OnKill(int timeLeft) {
		SoundEngine.PlaySound(SoundID.Item105 with { PitchRange = (.5f, 1f) });
		int amount = Main.rand.Next(1, 4);
		for (int i = 0; i < amount; i++) {
			Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(1, 1) * Main.rand.NextFloat(.9f, 1.3f), ModContent.ProjectileType<StardustSymphony_StarProjectile_Normal>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		Projectile projExplode = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f), ModContent.ProjectileType<StardustSymphony_Projectile_Explosion>(), Projectile.damage * 3, Projectile.knockBack, Projectile.owner);
		projExplode.scale = Main.rand.NextFloat(3.4f, 4.8f);

		Dust dust;
		for (int i = 0; i < 3; i++) {
			dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustBig>());
			dust.scale = Main.rand.NextFloat(.4f, .8f);
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5) * Main.rand.NextFloat(.6f, 1.1f);
			dust.color = Color.Cyan;
			dust.noGravity = true;
		}
		dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<StardustSymphony_DustStar>());
		dust.scale = Main.rand.NextFloat(.7f, 1.2f);
		dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.6f, 1.1f);
		dust.color = Color.Cyan;
		dust.noGravity = true;
	}
}
public class StardustSymphony_Projectile_Explosion : ModProjectile {
	public override string Texture => ModTexture.Glow_Big;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 46;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
	}
	public override void AI() {
		Projectile.velocity *= .98f;
		Projectile.scale -= .1f;
		if (Projectile.scale <= 0) {
			Projectile.Kill();
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		ModUtils.Draw_SetUpToDrawGlow(Main.spriteBatch);

		Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
		Vector2 drawpos = Projectile.position - Main.screenPosition;
		Main.spriteBatch.Draw(texture, drawpos, null, Color.Cyan with { A = 200 }, Projectile.rotation, texture.Size() * .5f, Projectile.scale, SpriteEffects.None, 0);

		Main.spriteBatch.Draw(texture, drawpos, null, Color.White, Projectile.rotation, texture.Size() * .5f, Projectile.scale * .75f, SpriteEffects.None, 0);

		ModUtils.Draw_ResetToNormal(Main.spriteBatch);
		return false;
	}
}
