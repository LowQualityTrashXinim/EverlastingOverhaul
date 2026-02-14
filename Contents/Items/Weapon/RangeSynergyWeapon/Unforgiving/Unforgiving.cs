using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Unforgiving;

public class Unforgiving : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(114, 46, 44, 10f, 10, 10, ItemUseStyleID.Shoot, ProjectileID.ShadowFlameArrow, 12, true, AmmoID.Arrow);
		Item.Set_InfoItem();
		Item.scale = .67f;
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-10, 0);
	}
	public override bool CanShoot(Player player) {
		if (GunMode) {
			Item.useAmmo = AmmoID.Bullet;
		}
		else {
			Item.useAmmo = AmmoID.Arrow;
		}
		var ammo = player.ChooseAmmo(Item);
		if (ammo == null) {
			GunMode = !GunMode;
			if (GunMode) {
				Item.useAmmo = AmmoID.Bullet;
			}
			else {
				Item.useAmmo = AmmoID.Arrow;
			}
			ammo = player.ChooseAmmo(Item);
			if (ammo == null) {
				return false;
			}
		}
		Pre_ShootType = ammo.shoot;
		return player.GetModPlayer<Unforgiving_ModPlayer>().AttackCD <= 0;
	}
	public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {
		player.GetModPlayer<Unforgiving_ModPlayer>().Unforgiving_GunMode = GunMode;
	}
	public int GlobalCounter = 0;
	int Pre_ShootType = 0;
	/// <summary>
	/// If gun mode is false then currently is bow mode<br/>
	/// Otherwise it is gun mode obviously
	/// </summary>
	public bool GunMode = false;
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 50);
		if (GunMode) {
			SoundEngine.PlaySound(SoundID.Item11);
			type = Pre_ShootType;
		}
		else {
			SoundEngine.PlaySound(SoundID.Item5);
			if (Pre_ShootType == ProjectileID.WoodenArrowFriendly) {
				type = ProjectileID.ShadowFlameArrow;
			}
			else {
				type = Pre_ShootType;
			}
		}
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		for (int i = 0; i < 30; i++) {
			Vector2 rotate = Main.rand.NextVector2CircularEdge(10, 2.5f).RotatedBy(velocity.ToRotation() + MathHelper.PiOver2);
			int dust3 = Dust.NewDust(position.PositionOFFSET(velocity, 30), 0, 0, DustID.Shadowflame);
			Main.dust[dust3].noGravity = true;
			Main.dust[dust3].velocity = rotate;
			Main.dust[dust3].fadeIn = 1f;
		}
		GlobalCounter = ModUtils.Safe_SwitchValue(GlobalCounter, 24);
		var unforgive = player.GetModPlayer<Unforgiving_ModPlayer>();
		int counter = unforgive.Counter;
		unforgive.Counter = -player.itemAnimationMax;
		int CD = unforgive.CD;
		var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
		if (counter >= 120) {
			if (GunMode) {
				proj.damage = (int)(proj.damage * 1.5f);
				for (int i = 0; i < 4; i++) {
					var blackbolt = Projectile.NewProjectileDirect(source, position, velocity.Vector2DistributeEvenlyPlus(4, 45, i), ProjectileID.BlackBolt, (int)(damage * 1.5f), knockback, player.whoAmI);
					blackbolt.scale = .56f;
					blackbolt.extraUpdates += 3;
				}
			}
			else {
				proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().CustomDataValue = 1;
				proj.extraUpdates += 2;
			}
		}
		if (counter >= 180) {
			SoundEngine.PlaySound(SoundID.Item60, position);
			Projectile.NewProjectile(source, position, velocity.SafeNormalize(Vector2.Zero) * 10, ModContent.ProjectileType<Unforgiving_BlastWave>(), damage * 2, 0, player.whoAmI);
		}
		if (GlobalCounter % 5 == 0) {
			GunMode = !GunMode;
			unforgive.AttackCD = 30;
		}
		if (GlobalCounter >= 24) {
			player.AddBuff<Unforgiving_BoltFury>(ModUtils.ToSecond(1));
		}
		if (CD <= 0) {
			unforgive.CD = 60;
			if (GunMode) {
				Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback, player.whoAmI);
			}
			else {
				for (int i = 0; i < 3; i++) {
					proj = Projectile.NewProjectileDirect(source, position, velocity.Vector2DistributeEvenlyPlus(3, 40, i), ProjectileID.ShadowFlameArrow, damage, knockback, player.whoAmI);
					if (counter >= 120) {
						proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().CustomDataValue = 1;
						proj.extraUpdates += 2;
					}
				}
			}
		}
	}
	public override void OutroAttack(Player player) {
		if (player.GetModPlayer<Unforgiving_ModPlayer>().CanActivateOutroAttack()) {
			player.AddBuff<Unforgiving_OutroAttack>(ModUtils.ToSecond(6));
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.OnyxBlaster)
			.AddIngredient(ItemID.ShadowFlameBow)
			.AddIngredient(ItemID.SpiritFlame)
			.Register();
	}
}
public class Unforgiving_OutroAttack : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class Roguelike_SpiritFlame : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.SpiritFlame);
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 4;
	}
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.SpiritFlame);
		Projectile.aiStyle = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.penetrate = 1;
		Projectile.width = 18;
		Projectile.height = 32;
		Projectile.timeLeft = 60;
	}
	NPC npc = null;
	public override void AI() {
		if (++Projectile.frameCounter > 4) {
			Projectile.frameCounter = 0;
			if (++Projectile.frame >= 4)
				Projectile.frame = 0;
		}
		if (npc != null) {
			if (!npc.active) {
				npc = null;
			}
		}
		float num1052 = 3f;
		float num1053 = 12.5f;
		float num1054 = 1f;
		int num1055 = 420;
		if (Projectile.localAI[0] > 0f)
			Projectile.localAI[0]--;

		if (Projectile.localAI[0] == 0f && npc == null && Projectile.owner == Main.myPlayer) {
			Projectile.localAI[0] = 5f;
			if (Projectile.Center.LookForHostileNPC(out NPC target, 900)) {
				npc = target;
			}

			if (npc != null) {
				Projectile.timeLeft = num1055;
				Projectile.netUpdate = true;
			}
		}

		if (Projectile.timeLeft > 30 && Projectile.alpha > 0)
			Projectile.alpha -= 12;

		if (Projectile.timeLeft > 30 && Projectile.alpha < 128 && Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
			Projectile.alpha = 128;

		if (Projectile.alpha < 0)
			Projectile.alpha = 0;

		float num1058 = 0.5f;
		if (Projectile.timeLeft < 120)
			num1058 = 1.1f;

		if (Projectile.timeLeft < 60)
			num1058 = 1.6f;

		Projectile.ai[1]++;
		for (float num1060 = 0f; num1060 < 3f; num1060++) {
			if (Main.rand.Next(3) == 0) {
				var dust61 = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, 0f, -2f)];
				dust61.position = Projectile.Center + Vector2.UnitY.RotatedBy(num1060 * ((float)Math.PI * 2f) / 3f + Projectile.ai[1]) * 10f;
				dust61.noGravity = true;
				dust61.velocity = Projectile.DirectionFrom(dust61.position);
				dust61.scale = num1058;
				dust61.fadeIn = 0.5f;
				dust61.alpha = 200;
			}
		}

		if (npc != null) {
			if (npc.active) {
				if (Projectile.Distance(npc.Center) > num1054) {
					var vector174 = Projectile.DirectionTo(npc.Center);
					if (vector174.HasNaNs())
						vector174 = Vector2.UnitY;

					Projectile.velocity = (Projectile.velocity * (num1052 - 1f) + vector174 * num1053) / num1052;
				}

			}
			return;
		}
	}
}
public class Unforgiving_ModPlayer : ModPlayer {
	public int Counter = 0;
	public int CD = 0;
	public bool Unforgiving_GunMode = false;
	public int AttackCD = 0;
	const int AttackBucketCap = 34000;
	int AttackBucket = 0;
	bool OutroAttack = false;
	bool ReachedMaxPotential = false;
	public bool CanActivateOutroAttack() {
		if (OutroAttack) {
			OutroAttack = false;
			AttackBucket = 0;
			return true;
		}
		return false;
	}
	public override void ResetEffects() {
		AttackCD = ModUtils.CountDown(AttackCD);
		CD = ModUtils.CountDown(CD);
		if (++Counter >= 180) {
			Counter = 180;
			if (Player.IsHeldingModItem<Unforgiving>()) {
				if (!ReachedMaxPotential) {
					SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 1 });
					for (int i = 0; i < 150; i++) {
						var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.Shadowflame, Scale: Main.rand.NextFloat(1f, 1.2f));
						dust.velocity = Main.rand.NextVector2CircularEdge(5, 5);
						var dust2 = Dust.NewDustDirect(Player.Center, 0, 0, DustID.Granite, Scale: Main.rand.NextFloat(1, 1.1f));
						dust2.velocity = Main.rand.NextVector2CircularEdge(8, 8);
						dust2.noGravity = true;
					}
					ModUtils.DustStar(Player.Center, DustID.Granite, Color.Black);
					ReachedMaxPotential = true;
				}
			}
		}
		else {
			if (Player.IsHeldingModItem<Unforgiving>()) {
				if (Counter == 120) {
					SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = -1 });
					for (int i = 0; i < 150; i++) {
						var dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.Granite, Scale: Main.rand.NextFloat(0.55f, 1f));
						dust.velocity = Main.rand.NextVector2CircularEdge(5, 5);
						dust.noGravity = true;
					}
				}
			}
			ReachedMaxPotential = false;
		}
		if (AttackCD > 0) {
			return;
		}
	}
	public override void UpdateEquips() {
		if (Player.HasBuff<Unforgiving_BoltFury>()) {
			int bufftime = Player.buffTime[Player.FindBuffIndex(ModContent.BuffType<Unforgiving_BoltFury>())];
			if (bufftime % 5 == 0) {
				var newPos = Player.Center + (Player.Center - Main.MouseWorld).SafeNormalize(Vector2.Zero) * 50 + Main.rand.NextVector2CircularEdge(10, 10) * Main.rand.NextFloat(.9f, 1.4f);
				var vel = (newPos - Player.Center).SafeNormalize(Vector2.Zero) * 2;
				int damage = 5;
				if (Player.IsHeldingModItem<Unforgiving>()) {
					damage += Player.GetWeaponDamage(Player.HeldItem) / 2;
				}
				Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem),
					newPos, Main.rand.NextVector2CircularEdge(1, 1) + vel, ModContent.ProjectileType<Unforgiving_Bolt>(), damage, 3f, Player.whoAmI);
			}
		}
	}
	public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (Player.HasBuff<Unforgiving_OutroAttack>()) {
			Projectile.NewProjectile(source, position, velocity, ProjectileID.BlackBolt, damage * 3, knockback, Player.whoAmI);
			var vel = velocity.SafeNormalize(Vector2.Zero);
			Projectile.NewProjectile(source, position + Main.rand.NextVector2CircularEdge(100, 100) * Main.rand.NextFloat(.8f, 2), vel, ModContent.ProjectileType<Roguelike_SpiritFlame>(), damage, knockback, Player.whoAmI);
		}
		return base.Shoot(item, source, position, velocity, type, damage, knockback);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		AttackBucket = Math.Clamp(AttackBucket + hit.Damage, 0, int.MaxValue);
		if (AttackBucket >= AttackBucketCap) {
			OutroAttack = true;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource<Unforgiving>()) {
			if (Main.rand.NextBool(4) || Counter >= 120) {
				int time = ModUtils.ToSecond(Main.rand.Next(1, 3));
				target.AddBuff(BuffID.ShadowFlame, time);
			}
			if ((proj.type == ModContent.ProjectileType<Unforgiving_Bolt>() || target.HasBuff<Unforgiving_Mark>()) && target.HasBuff(BuffID.ShadowFlame)) {
				Projectile.NewProjectile(proj.GetSource_FromThis(), target.Center + Main.rand.NextVector2CircularEdge(100, 100), Vector2.Zero, ModContent.ProjectileType<Roguelike_SpiritFlame>(), 1 + hit.Damage / 3, 5, Player.whoAmI);
			}
		}
	}
}
public class Unforgiving_BoltFury : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
}
public class Unforgiving_Bolt : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 100;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 15;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 6000;
		Projectile.extraUpdates = 10;
	}
	Vector2 InitialMousePos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		Projectile.ai[0] = -1;
	}
	public NPC getNPC() {
		int whoAmI = (int)Projectile.ai[0];
		if (whoAmI == -1 || whoAmI >= Main.maxNPCs) {
			return null;
		}
		var npc = Main.npc[whoAmI];
		if (!npc.active || npc.life <= 0) {
			return null;
		}
		return npc;
	}
	public override void AI() {
		if (++Projectile.ai[2] < 300) {
			Projectile.velocity *= .99f;
			return;
		}
		if (InitialMousePos == Vector2.Zero) {
			InitialMousePos = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero);
			return;
		}
		var npc = getNPC();
		Projectile.ai[1] += .005f;
		if (Projectile.ai[1] > 2) {
			Projectile.ai[1] = 2;
		}
		if (npc == null) {
			Projectile.velocity = InitialMousePos * Projectile.ai[1];
			if (Projectile.Center.LookForHostileNPC(out NPC target, 600)) {
				Projectile.ai[0] = target.whoAmI;
			}
		}
		else {
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[1];
		}
		Projectile.velocity = Projectile.velocity.LimitedVelocity(2);
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff<Unforgiving_Mark>(ModUtils.ToSecond(Main.rand.Next(5, 10)));
	}
	public override bool PreDraw(ref Color lightColor) {
		lightColor.R = 255;
		lightColor.B = 255;
		lightColor.G = 0;
		Projectile.DrawTrail(lightColor, .01f);
		return false;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 30; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.GemAmethyst);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2CircularEdge(5, 5);
		}
	}
}
public class Unforgiving_Mark : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class Unforgiving_BlastWave : ModProjectile {
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
	}
	public override void SetDefaults() {
		Projectile.width = 68;
		Projectile.height = 112;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1250;
		Projectile.light = 0.5f;
		Projectile.extraUpdates = 10;
		Projectile.alpha = 255;
	}
	public override void AI() {
		if (Projectile.timeLeft <= 75) {
			Projectile.velocity *= .96f;
			Projectile.alpha = Math.Clamp(Projectile.alpha - 3, 0, 255);
		}
		Projectile.rotation = Projectile.velocity.ToRotation();

		var BetterTop = new Vector2(Projectile.Center.X, Projectile.Center.Y);
		var dust = Dust.NewDustDirect(BetterTop, 0, 0, DustID.Shadowflame, Projectile.velocity.X, 0, 0, Color.White, Main.rand.NextFloat(0.55f, 1f));
		dust.position += Main.rand.NextVector2Circular(Projectile.width, Projectile.width);
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ScalingArmorPenetration += 1f;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.damage > 10) {
			Projectile.damage = (int)(Projectile.damage * .95f);
		}
		else {
			Projectile.damage = 10;
		}
		target.immune[Projectile.owner] = 2;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		var texture = TextureAssets.Projectile[Projectile.type].Value;
		float percentageAlpha = Math.Clamp(Projectile.alpha / 255f, 0, 1f);
		var origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
		for (int k = 1; k < Projectile.oldPos.Length + 1; k++) {
			var drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
			var color = new Color(0, 0, 0, 1 - k / 100f);
			Main.EntitySpriteDraw(texture, drawPos, null, color * percentageAlpha, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		}
		return false;
	}
}
