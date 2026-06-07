using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_ShrimpPoBoy : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.ShrimpPoBoy;
	public override int LifeAmount() => 120;
	public override int CoolDownBetweenUse() => 360;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(4.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_ShrimpPoBoy_Buff>(), ModUtils.ToMinute(21));
	}
}
public class Roguelike_ShrimpPoBoy_Buff : FoodItemTier2 {
	public override int TypeID => ItemID.ShrimpPoBoy;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_ShrimpPoBoy_ModPlayer>().ShrimpPoBoy = true;
	}
}
public class Roguelike_ShrimpPoBoy_ModPlayer : ModPlayer {
	public bool ShrimpPoBoy = false;
	public override void ResetEffects() {
		ShrimpPoBoy = false;
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitEffect();
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		OnHitEffect();
	}
	private void OnHitEffect() {
		if (!ShrimpPoBoy) {
			return;
		}
		int projtype = ModContent.ProjectileType<Roguelike_ShrimpPoBoy_ModProjectile>();
		if (Main.rand.NextFloat() <= .01f && Player.ownedProjectileCounts[projtype] < 1) {
			Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Main.rand.NextVector2RectangleEdge(1, 1), ModContent.ProjectileType<Roguelike_ShrimpPoBoy_ModProjectile>(), 120 + Player.GetWeaponDamage(Player.HeldItem), 1, Player.whoAmI);
		}
	}
}
public class Roguelike_ShrimpPoBoy_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<NPC>(NPCID.Shark);
	public override void SetDefaults() {
		Projectile.width = 48;
		Projectile.height = 48;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = ModUtils.ToSecond(15);
		Projectile.tileCollide = false;
	}
	public float DashDuration { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }
	public float DashCoolDown { get => Projectile.ai[1]; set => Projectile.ai[1] = value; }
	public NPC npc = null;
	public override void AI() {
		if (DashDuration <= 0) {
			if (Main.MouseWorld.LookForHostileNPC(out NPC target, 1000)) {
				npc = target;
			}
			else {
				npc = null;
			}
		}
		if (npc != null && (!npc.active || npc.life <= 0)) {
			npc = null;
			DashDuration = 0;
			DashCoolDown = 10;
			return;
		}
		if (npc != null) {
			if (DashDuration <= 0 && Projectile.Center.IsCloseToPosition(npc.Center, 150)) {
				if (DashCoolDown <= 0) {
					DashDuration = 20;
					DashCoolDown = 20;
				}
			}
			if (DashCoolDown > 0 && DashDuration <= 0) {
				DashCoolDown--;
				Projectile.velocity *= .8f;
				return;
			}
			if (DashDuration <= 0) {
				Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 3;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(10);
			}
			else {
				if (DashDuration == 20) {
					Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 25;
				}
				if (!Projectile.Center.IsCloseToPosition(npc.Center, 150)) {
					DashDuration = 0;
				}
				DashDuration--;
				Projectile.velocity = Projectile.velocity.LimitedVelocity(25);
				return;
			}
		}
		else {
			Player player = Main.player[Projectile.owner];
			Vector2 destination = player.Center;
			if (Projectile.Center.IsCloseToPosition(player.Center, 170)) {
				destination = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.timeLeft * 2)) * 160;
			}
			Projectile.velocity += (destination - Projectile.Center).SafeNormalize(Vector2.Zero) * 3;
			if (DashDuration <= 0) {
				Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
			}
		}
	}
	public override void PostAI() {
		Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
		Projectile.spriteDirection = Projectile.direction;
		Projectile.rotation = Projectile.velocity.ToRotation();
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Rectangle framing = texture.Frame(1, 4, 0, 0, 0, 0);
		SpriteEffects effects;
		float rotation = Projectile.rotation;
		if (Projectile.spriteDirection == 1) {
			effects = SpriteEffects.FlipHorizontally;
		}
		else {
			rotation -= MathHelper.Pi;
			effects = SpriteEffects.None;
		}
		Vector2 origin = texture.Size() * .5f;
		origin.Y /= 4;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Main.spriteBatch.Draw(texture, drawpos, framing, lightColor, rotation, origin, Projectile.scale, effects, 0);
		return false;
	}
}
