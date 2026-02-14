using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.ItemOverhaul;

namespace EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.AkaiHanbunNoHasami;
internal class AkaiHanbunNoHasami : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(86, 104, 56, 4f, 24, 24, ItemUseStyleID.Swing, ModContent.ProjectileType<AkaiHanbunNoHasami_Slash_Projectile>(), 1f, true);
		Item.UseSound = SoundID.Item1 with { Pitch = 1f };
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
			global.SwingType = BossRushUseStyle.SwipeDown;
			global.SwingDegree = 165;
			global.SwingStrength = 11f;
			global.Ignore_AttackSpeed = true;
			global.DistanceThrust = 120;
			global.OffsetThrust = 20;
		}
		Item.Set_InfoItem();
	}
	int ComboChain_ResetTimer = 0;
	int ComboChain_Count = 0;
	public override bool CanUseItem(Player player) {
		if (!player.ItemAnimationActive) {
			var overhaul = Item.GetGlobalItem<MeleeWeaponOverhaul>();
			if (player.altFunctionUse == 2 && player.GetModPlayer<AkaiHanbunNoHasami_ModPlayer>().ThreadCutter_Counter > 10) {
				overhaul.HideSwingVisual = true;
				Item.noUseGraphic = true;
				Item.noMelee = true;
				ComboChain_Count = 0;
				return player.ownedProjectileCounts[ModContent.ProjectileType<AkaiHanbunNoHasami_SawMode_Projectile>()] < 1;
			}
			else {
				Item.noUseGraphic = false;
				Item.noMelee = false;
			}
			ComboChain_ResetTimer = 60 + player.itemAnimationMax;
			switch (++ComboChain_Count) {
				case 1:
					overhaul.HideSwingVisual = false;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					overhaul.SwingStrength = 11;
					break;
				case 2:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					overhaul.SwingStrength = 11;
					break;
				case 3:
					overhaul.HideSwingVisual = true;
					overhaul.SwingType = BossRushUseStyle.Thrust_Style1;
					overhaul.SwingStrength = 15;
					overhaul.IframeDivision = 1;
					overhaul.DistanceThrust = 10;
					break;
			}
			if (ComboChain_Count >= 3) {
				ComboChain_Count = 0;
			}
		}
		return player.ownedProjectileCounts[ModContent.ProjectileType<AkaiHanbunNoHasami_SawMode_Projectile>()] < 1;
	}
	public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {
		ComboChain_ResetTimer = ModUtils.CountDown(ComboChain_ResetTimer);
		if (ComboChain_ResetTimer <= 0) {
			ComboChain_Count = 0;
		}
	}
	public int ComboChain_ExtraEnhanced = 0;
	public override bool AltFunctionUse(Player player) {
		return true;
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
		var modplayerExtra = player.GetModPlayer<AkaiHanbunNoHasami_ModPlayer>();
		if (player.altFunctionUse == 2) {
			if (modplayerExtra.ThreadCutter_Counter >= 10) {
				switch (ComboChain_ExtraEnhanced) {
					case 1:
						damage = (int)(damage * .34f);
						break;
					case 2:
						damage = (int)(damage * .44f);
						break;
					case 3:
						damage = (int)(damage * 1.15f);
						break;
				}
				position.LookForHostileNPC(out var npclist, 750);
				if (npclist != null) {
					foreach (var target in npclist) {
						target.AddBuff<AkaiHanbunNoHasami_ThreadOfFate>(ModUtils.ToSecond(15));
					}
				}
				if (ComboChain_ExtraEnhanced <= 0) {
					ComboChain_ExtraEnhanced = 1;
				}
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<AkaiHanbunNoHasami_SawMode_Projectile>(), damage, knockback, player.whoAmI, ComboChain_ExtraEnhanced);
				if (ComboChain_ExtraEnhanced >= 3) {
					ComboChain_ExtraEnhanced = 0;
					modplayerExtra.ThreadCutter_Counter = 0;
				}
				else {
					ComboChain_ExtraEnhanced++;
				}
				return;
			}
			if (!player.HasBuff<AkaiHanbunNoHasami_ThreadOfFate_CoolDown>()) {
				player.AddBuff<AkaiHanbunNoHasami_ThreadOfFate_CoolDown>(ModUtils.ToSecond(12));
				position.LookForHostileNPC(out var npclist, 750);
				if (npclist != null) {
					foreach (var target in npclist) {
						player.StrikeNPCDirect(target, target.CalculateHitInfo(damage, 1));
						target.AddBuff<AkaiHanbunNoHasami_ThreadOfFate>(ModUtils.ToSecond(15));
					}
				}
				return;
			}
		}
		var pos = Main.MouseWorld;
		if ((pos - player.Center).LengthSquared() > 5625) {
			pos = position.PositionOFFSET(velocity, 75f);
		}
		int count = ComboChain_Count;
		Projectile projectile;
		if (count == 0) {
			damage = (int)(damage * 2.5f);
			projectile = Projectile.NewProjectileDirect(source, pos.PositionOFFSET(velocity, 150), velocity, type, damage, knockback, player.whoAmI, 5, 15);
			if (projectile.ModProjectile is AkaiHanbunNoHasami_Slash_Projectile proj) {
				proj.ScaleX = 5f;
				proj.ScaleY = 1f;
				proj.ProjectileColor = Color.Red;
				projectile.scale = 2;
			}
		}
		else {
			int amount = Main.rand.Next(1, 4);
			for (int i = 0; i < amount; i++) {
				projectile = Projectile.NewProjectileDirect(source, pos + Main.rand.NextVector2Circular(50, 50), velocity.RotatedBy(MathHelper.PiOver2).Vector2RotateByRandom(30), type, damage, knockback, player.whoAmI, 5, 5);
				if (projectile.ModProjectile is AkaiHanbunNoHasami_Slash_Projectile proj) {
					proj.ScaleX = 3f;
					proj.ScaleY = .5f;
					proj.ProjectileColor = Color.Red;
					projectile.scale = 2;
				}
			}
		}
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), target.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<AkaiHanbunNoHasami_Slash_Projectile>(), player.GetWeaponDamage(Item), 1, player.whoAmI, 2, 10);
		if (projectile.ModProjectile is AkaiHanbunNoHasami_Slash_Projectile proj) {
			proj.ScaleX = 3f;
			proj.ScaleY = 1f;
			proj.ProjectileColor = Color.Red;
			projectile.scale = .5f;
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.StylistKilLaKillScissorsIWish)
			.AddIngredient(ItemID.BreakerBlade)
			.Register();
	}
}
public class AkaiHanbunNoHasami_ThreadOfFate_CoolDown : ModBuff {
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class AkaiHanbunNoHasami_ThreadOfFate : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class AkaiHanbunNoHasami_ModPlayer : ModPlayer {
	public int ThreadCutter_Counter = 0;
	public int ThreadCutter_HitCooldown = 0;
	public override void ResetEffects() {
		ThreadCutter_HitCooldown = ModUtils.CountDown(ThreadCutter_HitCooldown);
	}
	public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot) {
		if (Player.HeldItem.type == ModContent.ItemType<AkaiHanbunNoHasami>()
			&& Player.ownedProjectileCounts[ModContent.ProjectileType<AkaiHanbunNoHasami_SawMode_Projectile>()] > 0) {
			return false;
		}
		return base.CanBeHitByNPC(npc, ref cooldownSlot);
	}
	public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
		if (Player.HeldItem.type != ModContent.ItemType<AkaiHanbunNoHasami>()) {
			return;
		}
		var tex = ModContent.Request<Texture2D>(ModTexture.WHITEDOT).Value;
		List<int> list_WhoAmI = new();
		foreach (var target in Main.ActiveNPCs) {
			if (!target.HasBuff<AkaiHanbunNoHasami_ThreadOfFate>()) {
				continue;
			}
			if (list_WhoAmI.Contains(target.whoAmI)) {
				continue;
			}
			foreach (var npc in Main.ActiveNPCs) {
				if (npc.whoAmI == target.whoAmI) {
					continue;
				}
				if (!npc.HasBuff<AkaiHanbunNoHasami_ThreadOfFate>()) {
					continue;
				}
				if (list_WhoAmI.Contains(npc.whoAmI)) {
					continue;
				}
				var distance = npc.Center - target.Center;
				float length = distance.Length();
				float rotation = distance.ToRotation();
				var currentPos = npc.Center.PositionOFFSET(distance, length * -.5f) - Main.screenPosition + new Vector2(0, npc.gfxOffY);
				Main.EntitySpriteDraw(tex, currentPos, null, Color.Red with { A = 0 }, rotation, tex.Size() * .5f, new Vector2(length * .5f, 1f), SpriteEffects.None);
				list_WhoAmI.Add(npc.whoAmI);
				list_WhoAmI.Add(target.whoAmI);
			}
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type != ModContent.ItemType<AkaiHanbunNoHasami>() || Player.ownedProjectileCounts[ModContent.ProjectileType<AkaiHanbunNoHasami_SawMode_Projectile>()] > 0) {
			return;
		}
		if (ThreadCutter_HitCooldown <= 0) {
			ThreadCutter_HitCooldown = 60;
			ThreadCutter_Counter++;
		}
		Effect();
		if (target.HasBuff<AkaiHanbunNoHasami_ThreadOfFate>()) {
			foreach (var npc in Main.ActiveNPCs) {
				if (npc.HasBuff<AkaiHanbunNoHasami_ThreadOfFate>() && npc != target && npc.immune[Player.whoAmI] <= 0) {
					Player.StrikeNPCDirect(npc, hit);
				}
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!proj.Check_ItemTypeSource<AkaiHanbunNoHasami>() || proj.type == ModContent.ProjectileType<AkaiHanbunNoHasami_SawMode_Projectile>()
			|| Player.ownedProjectileCounts[ModContent.ProjectileType<AkaiHanbunNoHasami_SawMode_Projectile>()] > 0) {
			return;
		}
		if (ThreadCutter_HitCooldown <= 0) {
			ThreadCutter_HitCooldown = 60;
			ThreadCutter_Counter++;
		}
		Effect();
		if (target.HasBuff<AkaiHanbunNoHasami_ThreadOfFate>()) {
			foreach (var npc in Main.ActiveNPCs) {
				if (npc.HasBuff<AkaiHanbunNoHasami_ThreadOfFate>() && npc != target && npc.immune[Player.whoAmI] <= 0) {
					Player.StrikeNPCDirect(npc, hit);
				}
			}
		}
	}
	private void Effect() {
		if (ThreadCutter_Counter == 10) {
			ThreadCutter_Counter++;
			for (int i = 0; i < 100; i++) {
				var dust = Dust.NewDustDirect(Player.Center, 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust>());
				dust.color = Color.Red with { A = 0 };
				dust.rotation += Main.rand.NextFloat();
				dust.velocity = Vector2.UnitX.RotatedBy(MathHelper.PiOver2 * (i % 5f)) * (5 + Main.rand.NextFloat(4, 17));
				dust.scale += Main.rand.NextFloat(.5f, .7f) + .5f;
				if (Main.rand.NextBool()) {
					var dust2 = Dust.NewDustDirect(Player.Center, 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust>());
					dust2.color = Color.Black;
					dust2.rotation += Main.rand.NextFloat();
					dust2.velocity = Main.rand.NextVector2CircularEdge(15, 15);
					dust2.scale += Main.rand.NextFloat(.5f, .7f);
				}
			}
		}
	}
}
public class AkaiHanbunNoHasami_Dust : Roguelike_Dust_ModDust5x5T1 {
	public override void SetStaticDefaults() {
		RoguelikeGlobalDust.TrailLength[Type] = 30;
	}
	public override bool Update(Dust dust) {
		dust.velocity *= .9f;
		dust.velocity = dust.velocity.RotatedBy(MathHelper.ToRadians(2));
		dust.scale -= .05f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return false;
	}
	public override bool PreDraw(Dust dust) {
		var moddust = dust.Dust_GetDust();
		var texture = Texture2D.Value;
		var origin = texture.Size() * .5f;
		for (int i = 0; i < moddust.oldPos.Length; i++) {
			var drawpos = moddust.oldPos[i] - Main.screenPosition;
			Main.EntitySpriteDraw(texture, drawpos, null, dust.color, moddust.oldRot[i], origin, dust.scale * (1 - i / (float)moddust.oldPos.Length), SpriteEffects.None);
		}
		return false;
	}
}
public class AkaiHanbunNoHasami_Dust2 : Roguelike_Dust_ModDust5x5T1 {
	public override bool Update(Dust dust) {
		dust.velocity *= .9f;
		dust.scale -= .01f;
		if (dust.scale <= 0) {
			dust.active = false;
		}
		else {
			dust.position += dust.velocity;
		}
		return false;
	}
	public override bool PreDraw(Dust dust) {
		var moddust = dust.Dust_GetDust();
		var texture = Texture2D.Value;
		var origin = texture.Size() * .5f;
		for (int i = 0; i < moddust.oldPos.Length; i++) {
			var drawpos = moddust.oldPos[i] - Main.screenPosition;
			Main.EntitySpriteDraw(texture, drawpos, null, dust.color, moddust.oldRot[i], origin, dust.scale * (1 - i / (float)moddust.oldPos.Length), SpriteEffects.None);
		}
		return false;
	}
}
public class AkaiHanbunNoHasami_SawMode_Projectile : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<AkaiHanbunNoHasami>();
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 90;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 999;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
	}
	public float Mode { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
	Vector2 initialVelocity = Vector2.Zero;
	public override void OnSpawn(IEntitySource source) {
		if (Mode == 1) {
			Projectile.timeLeft = 120;
		}
		else if (Mode == 2 || Mode == 3) {
			if (Mode == 3) {
				Projectile.timeLeft = 180;
			}
			else {
				Projectile.timeLeft = 150;
			}
			Projectile.velocity = -Projectile.velocity;
			Projectile.spriteDirection = ModUtils.DirectionFromEntityAToEntityB(Projectile.Center.X, Main.MouseWorld.X);
		}
		initialVelocity = Projectile.velocity;
	}
	public int counterExtra = 0;
	public override void AI() {
		SoundEngine.PlaySound(SoundID.Item22 with { Pitch = 1f }, Projectile.Center);
		Projectile.Center.LookForHostileNPC(out var listNPC, 400);
		if (listNPC != null) {
			foreach (var npc in listNPC) {
				if (npc.boss) {
					continue;
				}
				var distance = Projectile.Center - npc.Center;
				npc.velocity += (Projectile.Center - npc.Center).SafeNormalize(Vector2.Zero) * distance.Length() / 64f;
			}
		}
		var player = Main.player[Projectile.owner];
		if (Mode == 1) {
			Projectile.velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
			Saw_rotationAdd += MathHelper.ToRadians(15);
			Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
		}
		if (Mode == 2 || Mode == 3) {
			if (Mode == 3) {
				float ro = MathHelper.Lerp(0, 180 + 360, ModUtils.OutExpo(Math.Clamp(++Projectile.ai[1] / 50f, 0, 1f), 16f)) * Projectile.spriteDirection;
				bool decider = Main.rand.NextBool(5);
				int damage = Projectile.damage;
				if (decider) {
					damage = (int)(damage * 2.5f);
				}
				var projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center.PositionOFFSET(Projectile.velocity, 60) + Main.rand.NextVector2Circular(100, 100), Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<AkaiHanbunNoHasami_Slash_Projectile>(), damage, 2, player.whoAmI, 5, 5);
				if (projectile.ModProjectile is AkaiHanbunNoHasami_Slash_Projectile proj) {
					if (decider) {
						proj.ScaleX = 5f;
						proj.ScaleY = 1f;
						proj.ProjectileColor = Color.Red;
						projectile.scale = 2;
					}
					else {
						proj.ScaleX = 3f;
						proj.ScaleY = .5f;
						proj.ProjectileColor = Color.Red;
						projectile.scale = 2;
					}
				}
				Projectile.velocity = initialVelocity.RotatedBy(MathHelper.ToRadians(ro));
				if (Projectile.ai[1] >= 45) {
					Projectile.velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				}
			}
			else {
				var projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center.PositionOFFSET(Projectile.velocity, 60) + Main.rand.NextVector2Circular(100, 100), Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<AkaiHanbunNoHasami_Slash_Projectile>(), Projectile.damage, 2, player.whoAmI, 5, 5);
				if (projectile.ModProjectile is AkaiHanbunNoHasami_Slash_Projectile proj) {
					proj.ScaleX = 3f;
					proj.ScaleY = .5f;
					proj.ProjectileColor = Color.Red;
					projectile.scale = 2;
				}
				if (Projectile.ai[2] >= 45) {
					Projectile.velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				}
				else {
					if (++Projectile.ai[2] >= 10) {
						float ro = MathHelper.Lerp(0, 180, ModUtils.OutExpo(Math.Clamp(++Projectile.ai[1] / 30f, 0, 1f), 16f)) * Projectile.spriteDirection;
						Projectile.velocity = initialVelocity.RotatedBy(MathHelper.ToRadians(ro));
					}
				}
			}
			Saw_rotationAdd += MathHelper.ToRadians(10);
		}
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		if (Projectile.spriteDirection == -1) {
			Projectile.rotation -= MathHelper.PiOver2;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver4);
		}
		else {
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2 - MathHelper.PiOver4);
		}
		Projectile.Center = player.Center.PositionOFFSET(Projectile.velocity, 60);
		var dust = Dust.NewDustDirect(Projectile.Center.PositionOFFSET(Projectile.velocity, 55), 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust>());
		dust.color = Color.Red with { A = 0 };
		dust.rotation += Main.rand.NextFloat();
		if (Mode == 3) {
			dust.velocity = Main.rand.NextVector2CircularEdge(35, 35);
			dust.scale += Main.rand.NextFloat(1.2f, 1.3f) + .5f;
		}
		else {
			dust.velocity = Main.rand.NextVector2CircularEdge(15, 15);
			dust.scale += Main.rand.NextFloat(.5f, .7f) + .5f;
		}
		if (Main.rand.NextBool()) {
			var dust2 = Dust.NewDustDirect(Projectile.Center.PositionOFFSET(Projectile.velocity, 55), 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust>());
			dust2.color = Color.Black;
			dust2.rotation += Main.rand.NextFloat();
			if (Mode == 3) {
				dust2.velocity = Main.rand.NextVector2CircularEdge(35, 35);
				dust2.scale += Main.rand.NextFloat(1.2f, 1.3f);
			}
			else {
				dust2.velocity = Main.rand.NextVector2CircularEdge(15, 15);
				dust2.scale += Main.rand.NextFloat(.5f, .7f);
			}
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.immune[Projectile.owner] = 5;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		return ModUtils.CompareSquareFloatValueWithHitbox(Projectile.Center.PositionOFFSET(Projectile.velocity, 70), targetHitbox.Center(), targetHitbox, 70);
	}
	public float Saw_rotationAdd = 0;
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		var texture = TextureAssets.Projectile[Type].Value;
		var origin = texture.Size() * .5f;
		var drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
		var effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, effect, 0);
		drawPos = drawPos.PositionOFFSET(Projectile.velocity, 60);
		float scale = Projectile.scale * .5f;
		if (Mode == 3) {
			scale = 1f;
		}
		for (int i = 1; i < 24; i++) {
			if (Mode == 1 && Projectile.timeLeft >= 70 + i * 2) {
				continue;
			}
			Main.EntitySpriteDraw(texture, drawPos, null, Color.Red with { A = 200 }, Projectile.rotation + MathHelper.Lerp(0, MathHelper.TwoPi, i / 23f) + Saw_rotationAdd, origin * 2, scale, SpriteEffects.FlipHorizontally, 0);
		}
		var tex = ModContent.Request<Texture2D>(ModTexture.WHITEDOT).Value;
		var texOri = tex.Size() * .5f;
		foreach (var target in Main.ActiveNPCs) {
			if (!target.HasBuff<AkaiHanbunNoHasami_ThreadOfFate>()) {
				continue;
			}
			var distance = Projectile.Center - target.Center;
			float length = distance.Length();
			float rotation = distance.ToRotation();
			var currentPos = drawPos.PositionOFFSET(distance, length * -.5f) + new Vector2(0, Projectile.gfxOffY);
			Main.EntitySpriteDraw(tex, currentPos, null, Color.Red with { A = 0 }, rotation, texOri, new Vector2(length * .5f, 1f), SpriteEffects.None);
		}
		return false;
	}
	public override void OnKill(int timeLeft) {
		if (Mode == 3) {
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AkaiHanbunNoHasami_SawMode_Projectile_DeathProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}
	}
}
public class AkaiHanbunNoHasami_SawMode_Projectile_DeathProjectile : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 1;
		Projectile.timeLeft = 150;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hide = true;
	}
	public override bool? CanDamage() => false;
	public override void AI() {
		if (++Projectile.ai[0] < 5) {
			return;
		}
		bool decider = Main.rand.NextBool(5);
		int damage = Projectile.damage;
		if (decider) {
			damage = (int)(damage * 3.5f);
		}
		var projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + Main.rand.NextVector2Circular(250, 250), Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<AkaiHanbunNoHasami_Slash_Projectile>(), damage, 2, Projectile.owner, 5, 5);
		if (projectile.ModProjectile is AkaiHanbunNoHasami_Slash_Projectile proj) {
			if (decider) {
				proj.ScaleX = 10f;
				proj.ScaleY = 1f;
				proj.ProjectileColor = Color.Red;
				projectile.scale = 2;
			}
			else {
				proj.ScaleX = 3f;
				proj.ScaleY = .5f;
				proj.ProjectileColor = Color.Red;
				projectile.scale = 2;
			}
		}
	}
}
/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// </summary>
public class AkaiHanbunNoHasami_Slash_Projectile : ModProjectile {
	public Color ProjectileColor = Color.White;
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
	float InitialScaleXValue = 0f;
	float InitialScaleYValue = 0f;
	public float ScaleX = 3f;
	public float ScaleY = 1f;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 36;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 60;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 10;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
	}
	public override void OnSpawn(IEntitySource source) {
		if (Projectile.ai[0] <= 0) {
			Projectile.ai[0] = 1;
		}
		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.ai[0];
		Projectile.rotation = Projectile.velocity.ToRotation();
		if (Projectile.ai[1] <= 0) {
			Projectile.ai[1] = 15;
		}
		Projectile.timeLeft = (int)Projectile.ai[1];
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		var pointEdgeOfProjectile = Projectile.Center.IgnoreTilePositionOFFSET(Projectile.rotation.ToRotationVector2(), 18 * ScaleX * Projectile.scale);
		var pointEdgeOfProjectile2 = Projectile.Center.IgnoreTilePositionOFFSET((Projectile.rotation + MathHelper.Pi).ToRotationVector2(), 18 * ScaleX * Projectile.scale);
		return ModUtils.Collision_PointAB_EntityCollide(targetHitbox, pointEdgeOfProjectile, pointEdgeOfProjectile2);
	}
	public override Color? GetAlpha(Color lightColor) {
		ProjectileColor.A = 0;
		return ProjectileColor * (Projectile.timeLeft / Projectile.ai[1]);
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ScalingArmorPenetration += 1f;
	}
	public override void AI() {
		float timeleft = Projectile.Get_ProjectileTimeInitial();
		if (Projectile.timeLeft == timeleft) {
			InitialScaleXValue = ScaleX;
			InitialScaleYValue = ScaleY;
			float extraScaleX = ScaleX * .5f;
			for (int i = 0; i < 40; i++) {
				var dust = Dust.NewDustDirect(ModUtils.NextPointOn2Vector2(Projectile.Center.PositionOFFSET(Projectile.velocity, 36 * extraScaleX), Projectile.Center.PositionOFFSET(Projectile.velocity, -36 * extraScaleX)), 0, 0, ModContent.DustType<AkaiHanbunNoHasami_Dust2>());
				dust.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3, 5);
				dust.scale = Main.rand.NextFloat(.2f, .35f);
				dust.color = Color.Black with { A = 150 };
				dust.rotation += Main.rand.NextFloat();
			}
		}
		ScaleX = InitialScaleXValue * (Projectile.timeLeft / timeleft);
		ScaleY = InitialScaleYValue * (Projectile.timeLeft / timeleft);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
		var texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
		var origin = texture.Size() * .5f;
		var drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, Color.Red with { A = 100 }, Projectile.rotation, origin, new Vector2(ScaleX, ScaleY) * Projectile.scale, SpriteEffects.None, 0);
		Main.EntitySpriteDraw(texture, drawPos, null, Color.Black with { A = 200 }, Projectile.rotation, origin, new Vector2(ScaleX, ScaleY) * Projectile.scale * .5f, SpriteEffects.None, 0);
		return false;
	}
}
