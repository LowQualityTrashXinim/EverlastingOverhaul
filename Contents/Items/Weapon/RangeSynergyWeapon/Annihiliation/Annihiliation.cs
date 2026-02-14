using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.AkaiHanbunNoHasami;
using EverlastingOverhaul.Texture;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Annihiliation;
internal class Annihiliation : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(138, 36, 33, 3f, 2, 6, ItemUseStyleID.Shoot, ModContent.ProjectileType<AnnihiliationBullet>(), 20f, true, AmmoID.Bullet);
		Item.scale = .87f;
		Item.UseSound = SoundID.Item38 with {
			Pitch = 1f
		};
		Item.Set_InfoItem();
	}
	public override Vector2? HoldoutOffset() {
		return new(-30, 0);
	}
	public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {
		int counter = ++modplayer.Annihiliation_Counter;
		if (counter >= 600) {
			modplayer.Annihiliation_Counter = 600;
		}
		if (!player.IsHeldingModItem<Annihiliation>()) {
			return;
		}
		if (!player.ItemAnimationActive) {
			if (counter >= 360) {
				player.AddBuff<Epilogue_Ishboshet>(ModUtils.ToSecond(5) + counter - 360);
			}
		}
		else {
			modplayer.Annihiliation_Counter = 0;
		}
	}
	public override float UseSpeedMultiplier(Player player) {
		if (player.altFunctionUse == 2) {
			return .15f;
		}
		return base.UseSpeedMultiplier(player);
	}
	public override bool AltFunctionUse(Player player) => true;
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity * Main.rand.NextFloat(.075f, .1f);
		type = Item.shoot;
		position = position.PositionOFFSET(velocity, 90);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		if (player.altFunctionUse == 2) {
			if (player.itemAnimation == player.itemAnimationMax) {
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(position.PositionOFFSET(velocity, -40) + Main.rand.NextVector2CircularEdge(1, 10).RotatedBy(velocity.ToRotation()) * 10, 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust2>());
					dust.velocity = velocity * .25f;
					dust.scale = 1 + Main.rand.NextFloat(.2f, .35f);
					if (Main.rand.NextBool()) {
						dust.color = Color.Purple with { A = 0 };
					}
					else {
						dust.color = Color.Black with { A = 10 };
					}
					dust.rotation += Main.rand.NextFloat();
					Dust dust1 = Dust.NewDustDirect(position.PositionOFFSET(velocity, 10) + Main.rand.NextVector2CircularEdge(1, 10).RotatedBy(velocity.ToRotation()) * 6, 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust2>());
					dust1.velocity = velocity * .5f;
					dust1.scale = 1 + Main.rand.NextFloat(.2f, .35f);
					if (Main.rand.NextBool()) {
						dust1.color = Color.Purple with { A = 0 };
					}
					else {
						dust1.color = Color.Black with { A = 10 };
					}
					dust1.rotation += Main.rand.NextFloat();
					Dust dust2 = Dust.NewDustDirect(position.PositionOFFSET(velocity, 40) + Main.rand.NextVector2CircularEdge(1, 10).RotatedBy(velocity.ToRotation()) * 3, 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust2>());
					dust2.velocity = velocity;
					dust2.scale = 1 + Main.rand.NextFloat(.2f, .35f);
					if (Main.rand.NextBool()) {
						dust2.color = Color.Purple with { A = 0 };
					}
					else {
						dust2.color = Color.Black with { A = 10 };
					}
					dust2.rotation += Main.rand.NextFloat();
				}
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AnnihiliationShot>(), damage * 15, knockback, player.whoAmI);
			}
		}
		else {
			Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(3), type, damage, knockback, player.whoAmI);
		}
		CanShootItem = false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.ChainGun)
			.AddIngredient(ItemID.TrueNightsEdge)
			.Register();
	}
}
internal class AnnihiliationShot : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 20;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 4;
		Projectile.scale = 4;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		if (Projectile.timeLeft < 15) {
			return false;
		}
		if (!Collision.CanHitLine(Projectile.Center, 1, 1, targetHitbox.Center(), 1, 1)) {
			return false;
		}
		return ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Projectile.Center, Projectile.Center.IgnoreTilePositionOFFSET(ToMouseDirection, 1000));
	}
	Vector2 scaleVec = Vector2.One * 5;
	public override void AI() {
		if (Projectile.timeLeft == 20) {
			scaleVec = Vector2.One * 10;
			scaleVec *= Projectile.scale;
			Vector2 toMouse = Projectile.velocity.SafeNormalize(Vector2.Zero);
			Projectile.velocity = Vector2.Zero;
			Player player = Main.player[Projectile.owner];
			Projectile.ai[0] = toMouse.X;
			Projectile.ai[1] = toMouse.Y;
			Projectile.Center = player.Center;
			Projectile.rotation = toMouse.ToRotation() - MathHelper.PiOver2;
			Projectile.ai[2] = 1515;
			SoundEngine.PlaySound(SoundID.Item88 with { Pitch = -1 });
			SoundEngine.PlaySound(SoundID.Item33 with { Pitch = 1 });
			//When projectile can stop
			for (int i = 0; i <= Projectile.ai[2]; i++) {
				if (!Collision.CanHitLine(Projectile.Center, 0, 0, Projectile.Center + toMouse * i, 0, 0)) {
					Projectile.ai[2] = i;
					Projectile.ai[2] += 15;
					break;
				}
			}
			scaleVec.Y = Projectile.ai[2] * .001f;
			float amountDust = 500 * scaleVec.Y;
			for (int i = 0; i < amountDust; i++) {
				var dust = Dust.NewDustDirect(ModUtils.NextPointOn2Vector2(Projectile.Center, Projectile.Center + toMouse * Projectile.ai[2]), 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust2>());
				dust.velocity = toMouse.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0, 2);
				dust.scale = Main.rand.NextFloat(.4f, .85f);
				if (Main.rand.NextBool()) {
					dust.color = Color.Purple with { A = 0 };
				}
				else {
					dust.color = Color.Black with { A = 150 };
				}
				dust.rotation += Main.rand.NextFloat();
				dust.noGravity = true;
			}
		}

		scaleVec.X *= .9f;
		if (scaleVec.X <= 0) {
			Projectile.Kill();
		}
	}
	Vector2 ToMouseDirection => new(Projectile.ai[0], Projectile.ai[1]);
	public override bool PreDraw(ref Color lightColor) {
		//Ain't the best way
		Vector2 drawpos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) + Vector2.One * .5f;
		Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawpos, null, Color.Purple with { A = 0 }, Projectile.rotation, Vector2.One * .5f, scaleVec, SpriteEffects.None);
		Vector2 scalele2 = scaleVec;
		scalele2.X *= .5f;
		Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawpos, null, Color.Black, Projectile.rotation, Vector2.One * .5f, scalele2, SpriteEffects.None);
		Vector2 scalele3 = scalele2;
		scalele3.X *= .5f;
		Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, drawpos, null, Color.White, Projectile.rotation, Vector2.One * .5f, scalele3, SpriteEffects.None);
		return false;
	}
}
public class Epilogue_Ishboshet : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override bool ReApply(Player player, int time, int buffIndex) {
		player.buffTime[buffIndex] = time;
		return true;
	}
	public override void Update(Player player, ref int buffIndex) {
		for (int i = 0; i < 2; i++) {
			Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, Main.rand.Next(new int[] { DustID.Shadowflame, DustID.Wraith, DustID.DemonTorch }));
			dust.noGravity = true;
			dust.velocity = Vector2.UnitY * -Main.rand.NextFloat(10);
			dust.scale = Main.rand.NextFloat(0.75f, 1.25f);
		}
		if (player.HeldItem.type == ModContent.ItemType<Annihiliation>()) {
			PlayerStatsHandle modplayer = player.GetModPlayer<PlayerStatsHandle>();
			modplayer.AddStatsToPlayer(PlayerStats.CritDamage, 4);
			modplayer.AddStatsToPlayer(PlayerStats.AttackSpeed, 2);
		}
	}
}
public class AnnihiliationBullet : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = ModUtils.ToSecond(30);
		Projectile.extraUpdates = 20;
		Projectile.penetrate = 12;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
	}
	public override bool? CanDamage() {
		return Projectile.penetrate != 1;
	}
	public override Color? GetAlpha(Color lightColor) {
		Color color = Color.White;
		color.A = 0;
		//money symbol
		return color * Projectile.Opacity;
	}
	public override void AI() {
		if (Projectile.penetrate <= 10) {
			int timeleft = 300;
			if (Projectile.timeLeft > timeleft) {
				Projectile.timeLeft = timeleft;
			}
			float progress = Projectile.timeLeft / (float)timeleft;
			//Projectile.alpha = (int)(255 * progress);
			Projectile.Opacity = progress;
			if (Projectile.ai[0] < 0) {
				return;
			}
			Projectile.velocity *= .995f;
		}
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.player[Projectile.owner].HasBuff<Epilogue_Ishboshet>()) {
			modifiers.ScalingArmorPenetration += 1;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.damage = (int)(Projectile.damage * .9f);
		if (Projectile.damage < 0) {
			Projectile.damage = 1;
		}
		float randomrotation = Main.rand.NextFloat(90);
		Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
		for (int i = 0; i < 4; i++) {
			Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * Main.rand.NextFloat(2, 3);
			for (int l = 0; l < 4; l++) {
				float multiplier = Main.rand.NextFloat();
				float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
				int dust = Dust.NewDust(Projectile.Center + randomPosOffset, 0, 0, DustID.GemAmethyst, 0, 0, 0, Main.rand.Next(new Color[] { Color.White, Color.Purple }), scale);
				Main.dust[dust].velocity = Toward * multiplier;
				Main.dust[dust].noGravity = true;
			}
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Vector2.Zero;
		Projectile.ai[0] = -1;
		Projectile.timeLeft = 120;
		return false;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
		for (int k = 0; k < Projectile.oldPos.Length; k++) {
			Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Color.Magenta * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color.A = 0;
			float scaling = Math.Clamp(k * .01f, 0, 10f);
			float realscale = (Projectile.scale - scaling) * Projectile.Opacity;
			Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], origin, realscale, SpriteEffects.None, 0);

			Color color2 = Color.White * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			color2.A = 0;
			Main.EntitySpriteDraw(texture, drawPos, null, color2, Projectile.oldRot[k], origin, realscale * .5f, SpriteEffects.None, 0);
		}
		return false;
	}
}
