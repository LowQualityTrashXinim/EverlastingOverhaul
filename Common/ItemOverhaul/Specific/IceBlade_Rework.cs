using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Graphics;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class Roguelike_IceBlade : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.IceBlade;
	public static readonly WeaponProgress progress = new() {

	};
	public override void SetStaticDefaults() {
		progress.Charge = true;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 39;
		entity.shootsEveryUse = true;
		entity.scale += .5f;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, $"RoguelikeOverhaul_{item.Name}", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void HoldItem(Item item, Player player) {
		if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player)) {
			OutroEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(7));
		}
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.SetWeaponProgress(progress);
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.barProgress = player.GetModPlayer<Roguelike_IceBlade_ModPlayer>().Counter / 120f;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientA = Color.Cyan;
		ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientB = Color.AliceBlue;
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Roguelike_IceBlade_ModPlayer modplayer = player.GetModPlayer<Roguelike_IceBlade_ModPlayer>();
		Vector2 velUnit = velocity.SafeNormalize(Vector2.Zero);
		if (OutroEffect_ModPlayer.Check_IntroEffect(player, item.type)) {

			var velocityToward = velocity.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).Vector2RotateByRandom(55);
			var Swordprojectile = Projectile.NewProjectileDirect(source, position + velUnit * item.Size.Length(), velocityToward, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(damage * .85f), 2f, player.whoAmI, 2f + Main.rand.NextFloat(2));
			if (Swordprojectile.ModProjectile is SimplePiercingProjectile2 modproj) {
				modproj.ProjectileColor = SwordSlashTrail.averageColorByID[ItemID.IceBlade] * 2;
				modproj.ScaleX = 9 + Main.rand.NextFloat();
			}
			Swordprojectile.usesIDStaticNPCImmunity = false;
			Swordprojectile.usesLocalNPCImmunity = true;
			Swordprojectile.localNPCHitCooldown = 60;
		}
		if (modplayer.Counter >= 120) {
			Vector2 rotate = velocity.RotatedBy(MathHelper.PiOver2);
			for (int i = 0; i < 5; i++) {
				Projectile projectile = Projectile.NewProjectileDirect(source, position + velUnit * 35 * (1 + i), rotate.Vector2RotateByRandom(55) * Main.rand.NextBool().ToDirectionInt(), ModContent.ProjectileType<IceBlade_Slash_Projectile>(), damage, knockback, player.whoAmI, .1f, 3, 5 + i);
				if (projectile.ModProjectile is IceBlade_Slash_Projectile slash) {
					slash.ScaleX = 2 + i * .05f;
					slash.ScaleY = .5f;
					slash.ProjectileColor = Color.Cyan;
					slash.ExtraDelay = 30;
				}
			}
			modplayer.Counter = -player.itemAnimationMax;
			return false;
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
}
public class Roguelike_IceBlade_ModPlayer : ModPlayer {
	public int Counter = 0;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++Counter >= 120) {
			Counter = 120;
		}
		if (Player.HeldItem.type == ItemID.IceBlade) {
			if (Player.ItemAnimationActive) {
				Counter = -Player.itemAnimationMax;
			}
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		if (Counter >= 120) {
			if (Player.HeldItem.type == ItemID.IceBlade) {
				damage *= 4;
			}
		}
	}
}
/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// Ai2 : Delay before the slash appear
/// </summary>
public class IceBlade_Slash_Projectile : SimplePiercingProjectile2 {
	public override void OnKill(int timeLeft) {
		int amount = Main.rand.Next(4, 9);
		for (int i = 0; i < amount; i++) {
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(2, 2) * Main.rand.NextFloat(1, 2), ModContent.ProjectileType<ReworkIceShards>(), (int)(Projectile.damage * .25f), Projectile.knockBack, Projectile.owner);
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.penetrate = 2;
			projectile.maxPenetrate = 2;
			projectile.tileCollide = true;
			projectile.scale = Main.rand.NextFloat(.5f, .7f);
		}
		amount = 3;
		for (int i = 0; i < amount; i++) {
			Vector2 vel = Main.rand.NextVector2CircularEdge(2, 2);
			Projectile projectile = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + vel * 10, vel, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(Projectile.damage * .67f), Projectile.knockBack, Projectile.owner, .1f, 15, 0);
			if (projectile.ModProjectile is SimplePiercingProjectile2 slash) {
				slash.ScaleX = 4 + i * .05f;
				slash.ScaleY = .25f;
				slash.ProjectileColor = Color.Cyan;
				slash.ExtraDelay = 10;
			}
		}
		Player player = Main.player[Projectile.owner];
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 150);
		foreach (var npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(Projectile.damage * .5f), 1));
		}
	}
}
public class ReworkIceShards : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 3;
	}
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 1200;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.extraUpdates = 5;
	}
	public override void AI() {
		Projectile.scale -= .005f;
		if (Projectile.scale <= 0) {
			Projectile.Kill();
		}
		Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
		if (++Projectile.frameCounter >= 6) {
			if (++Projectile.frame >= Main.projFrames[Type]) {
				Projectile.frame = 0;
			}
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Texture2D texture = TextureAssets.Extra[ExtrasID.CultistIceshard].Value;
		Rectangle rect = texture.Frame(1, 3, 0, Projectile.frame);
		Vector2 origin = rect.Size() * .5f;
		origin.Y /= 3;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Main.EntitySpriteDraw(texture, drawpos, rect, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
