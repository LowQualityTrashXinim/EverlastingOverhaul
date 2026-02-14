using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class Roguelike_AshWoodSword : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.AshWoodSword;
	}
	public override bool InstancePerEntity => true;
	public override void SetDefaults(Item entity) {
		entity.scale += 1.45f;
		entity.damage += 60;
		entity.useTime = entity.useAnimation = 33;
		entity.GetGlobalItem<MeleeWeaponOverhaul>().ShaderOffSetLength += 1;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	int swingCount = 0;
	public override void HoldItem(Item item, Player player) {
		if (player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive) {
			if (++swingCount >= 5) {
				swingCount = 0;
				AshSwordAttack(item, player);

			}
			var toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 100;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = item.type;
			Main.projectile[proj].ai[2] = 120;
		}
	}
	private void AshSwordAttack(Item item, Player player) {
		int damage = player.GetWeaponDamage(item);
		float knockback = player.GetWeaponKnockback(item);
		for (int i = 0; i < 20; i++) {
			var pos = new Vector2(Main.MouseWorld.X + Main.rand.Next(-150, 150), player.Center.Y - 1000 - 200 * i);
			var vel = Vector2.UnitY.Vector2RotateByRandom(5) * 20;
			float scale = 2;
			int projec;
			if (i == 5) {
				var newpos = new Vector2(Main.MouseWorld.X, player.Center.Y - 1100) + Main.rand.NextVector2Circular(100, 100);
				projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), newpos, (Main.MouseWorld - newpos).SafeNormalize(Vector2.Zero) * 10, ModContent.ProjectileType<AshwoodSwordProjectile>(), damage * 10, knockback, player.whoAmI, 1, 0, 1);
				Main.projectile[projec].penetrate = 10;
				Main.projectile[projec].maxPenetrate = 10;
				scale = 7;
			}
			else {
				projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<AshwoodSwordProjectile>(), damage, knockback, player.whoAmI, 1);
			}
			Main.projectile[projec].scale += scale;
		}
	}
}
internal class AshwoodSwordProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.AshWoodSword);
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.TrailCacheLength[Type] = 10;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = 2;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.extraUpdates = 5;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 15;
	}
	public int ItemIDtextureValue = ItemID.WoodenSword;
	Vector2 vel = Vector2.Zero;
	Vector2 mousePos = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		mousePos = Main.MouseWorld;
	}
	public float Counter { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public float State { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
	public override bool? CanDamage() {
		return State != 1 && Projectile.penetrate != 1;
	}
	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		if (Projectile.Center.Y >= mousePos.Y - 50) {
			return true;
		}
		else {
			return false;
		}
	}
	public override void AI() {
		if (State == 1) {
			if (Projectile.timeLeft > 30) {
				Projectile.timeLeft = 30;
			}
			Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.timeLeft / 30f);
			Projectile.velocity = Vector2.Zero;
			return;
		}
		if (Projectile.timeLeft > 900) {
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			vel = Projectile.velocity;
			Projectile.timeLeft = 900;
			if (Counter == 0) {
				Counter = 120;
			}
			Projectile.velocity = Vector2.Zero;
		}
		if (--Counter < 0) {
			Projectile.timeLeft = 900;
			Projectile.velocity += vel * .005f;
			Projectile.velocity = Projectile.velocity.LimitedVelocity(9);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		float length = Projectile.Size.Length() * Projectile.scale;
		var rotationVector = Projectile.rotation.ToRotationVector2();
		var Top = Projectile.Center.IgnoreTilePositionOFFSET(rotationVector, length / 2f);
		var Bottom = Projectile.Center.IgnoreTilePositionOFFSET(rotationVector, -length / 2);
		if (ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Top, Bottom)) {
			return true;
		}
		var rotate90degreeVel = Projectile.velocity.RotatedBy(MathHelper.PiOver2);
		if (ModUtils.Collision_PointAB_EntityCollide(targetHitbox, Top.PositionOFFSET(rotate90degreeVel, 30), Top.PositionOFFSET(rotate90degreeVel, -30))) {
			return true;
		}
		return false;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		var player = Main.player[Projectile.owner];
		int directionTo = (player.Center.X < target.Center.X).ToDirectionInt();
		modifiers.HitDirectionOverride = directionTo;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.ai[2] == 0) {
			State = 1;
		}
		SoundEngine.PlaySound(SoundID.Item88 with { MaxInstances = 0 });
		int amount = (int)Math.Ceiling(50 * Projectile.scale);
		float scale = Projectile.scale;
		for (int i = 0; i < amount; i++) {
			var lava = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Lava);
			lava.position += Main.rand.NextVector2Circular(32, 32);
			lava.velocity = -Vector2.UnitY.Vector2RotateByRandom(30) * Main.rand.NextFloat(1, 15) * scale;
			lava.noGravity = true;
			lava.scale = Main.rand.NextFloat(.5f, 2) + scale * .2f;
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (State != 1) {
			SoundEngine.PlaySound(SoundID.Item88 with { MaxInstances = 0 });
			State = 1;
			Projectile.position += Projectile.velocity * 2;
			Projectile.velocity = Vector2.Zero;
			int amount = (int)Math.Ceiling(50 * Projectile.scale);
			float scale = Projectile.scale;
			for (int i = 0; i < amount; i++) {
				var lava = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Lava);
				lava.position += Main.rand.NextVector2Circular(32, 32);
				lava.velocity = -Vector2.UnitY.Vector2RotateByRandom(30) * Main.rand.NextFloat(1, 15) * scale;
				lava.noGravity = true;
				lava.scale = Main.rand.NextFloat(.5f, 2) + scale * .2f;
			}
		}
		return false;
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadItem(ItemID.AshWoodSword);
		var texture = TextureAssets.Item[ItemID.AshWoodSword].Value;
		var origin = texture.Size() * .5f;
		if (State != 1) {
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				var drawPos2 = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
				var color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * .5f;
				Main.EntitySpriteDraw(texture, drawPos2, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			}
		}
		var drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
