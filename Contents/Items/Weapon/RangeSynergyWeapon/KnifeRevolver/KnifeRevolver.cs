using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.KnifeRevolver;
internal class KnifeRevolver : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultRange(84, 24, 21, 3f, 30, 30, ItemUseStyleID.Shoot, ProjectileID.Bullet, 10, false, AmmoID.Bullet);
		Item.crit = 10;
		Item.scale = 0.85f;
		Item.rare = ItemRarityID.LightRed;
		Item.value = Item.buyPrice(gold: 50);
	}
	public override Vector2? HoldoutOffset() {
		return new Vector2(-3, 4);
	}
	public override bool AltFunctionUse(Player player) => true;
	public override bool CanConsumeAmmo(Item ammo, Player player) {
		return player.altFunctionUse != 2;
	}
	public override bool CanUseItem(Player player) {
		return player.ownedProjectileCounts[ModContent.ProjectileType<KnifeRevolverThrown_Projectile>()] < 1;
	}
	public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		position = position.PositionOFFSET(velocity, 40);
		velocity = ModUtils.RoguelikeSpread(velocity, 1);
		if (Main.mouseRight) {
			velocity *= 2;
			type = ModContent.ProjectileType<KnifeRevolverThrown_Projectile>();
			damage *= 2;
			Item.noUseGraphic = true;
		}
		else {
			type = ProjectileID.Bullet;
			Item.noUseGraphic = false;
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Revolver)
			.AddIngredient(ItemID.Gladius)
			.Register();
	}
}
public class KnifeRevolverThrown_Projectile : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAsEntity<KnifeRevolver>();
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = ModUtils.ToMinute(5);
		Projectile.penetrate = -1;
		Projectile.scale = .67f;
	}
	public override bool? CanDamage() {
		return NPC_whoAmI == -1;
	}
	public override bool PreAI() {
		Player player = Main.player[Projectile.owner];
		if (player.ownedProjectileCounts[Type] > 1) {
			Projectile.Kill();
			return false;
		}
		return base.PreAI();
	}
	int NPC_whoAmI = -1;
	Vector2 offsetFromCenter = Vector2.Zero;
	Vector2 initialVel = Vector2.Zero;
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		NPC npc = null;
		if (Projectile.timeLeft == ModUtils.ToMinute(5)) {
			Projectile.spriteDirection = ModUtils.DirectionFromEntityAToEntityB(Projectile.Center.X, Main.MouseWorld.X);
			initialVel = Projectile.velocity;
		}
		Projectile.rotation = initialVel.ToRotation();
		if (NPC_whoAmI > 0 && NPC_whoAmI < 255) {
			npc = Main.npc[NPC_whoAmI];
			if (!npc.active || npc.life <= 0) {
				NPC_whoAmI = -1;
			}
			else {
				if (offsetFromCenter != Vector2.Zero) {
					Projectile.Center = npc.Center + offsetFromCenter;
					Projectile.rotation = (Projectile.Center - player.Center).ToRotation();
					Projectile.velocity = Vector2.Zero;
				}
			}
		}
		if (Projectile.ai[1] == 1 && Projectile.Center.IsCloseToPosition(player.Center, 50)
			|| !Projectile.Center.IsCloseToPosition(player.Center, 1500)) {
			Projectile.Kill();
		}
		if (player.itemAnimation != player.itemAnimationMax) {
			if (NPC_whoAmI == -1 && Projectile.ai[1] == 1) {
				Projectile.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
				if (Projectile.ai[2] == 1) {
					Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 30;
					Projectile.ai[2]++;
				}
				Projectile.Center += player.velocity;
			}
			if (Main.mouseRightRelease) {
				if (npc != null) {
					if (npc.type != NPCID.TargetDummy)
						npc.velocity += (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 5;
					NPC_whoAmI = -1;
					Projectile.ai[2] = 1;
				}
				Projectile.ai[1] = 1;
			}
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (NPC_whoAmI == -1) {
			NPC_whoAmI = target.whoAmI;
			target.immune[Projectile.owner] = 60;
			offsetFromCenter = Projectile.Center - target.Center;
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Main.rand.NextVector2CircularEdge(1, 1) * .1f, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(Projectile.damage * .45f), 2f, Projectile.owner, 2f + Main.rand.NextFloat(2));
		}
		Player player = Main.player[Projectile.owner];
		if (!player.GetModPlayer<KnifeRevolverPlayer>().SpecialShotReady) {
			player.GetModPlayer<KnifeRevolverPlayer>().SpecialShotReady = true;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Player player = Main.player[Projectile.owner];
		Texture2D tex = ModContent.Request<Texture2D>(ModTexture.WHITEDOT).Value;
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 distance = (player.Center - Projectile.Center);
		float length = distance.Length() - texture.Size().Length() * .5f;
		for (int i = 0; i < length; i++) {
			Vector2 currentPos = Vector2.Lerp(Projectile.Center, player.Center, i / length) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
			Main.EntitySpriteDraw(tex, currentPos, null, Color.Red.ScaleRGB(.2f) with { A = 0 }, distance.ToRotation() + MathHelper.PiOver2, tex.Size() * .5f, 1, SpriteEffects.None);
		}
		Main.instance.LoadProjectile(Type);
		Vector2 drawpos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
		SpriteEffects effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, texture.Size() * .5f, 1, effect);
		return false;
	}
}
public class DeathshotMark : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
}
public class KnifeRevolverPlayer : ModPlayer {
	public bool SpecialShotReady = false;
	public int shootCounter = 0;
	public override void PreUpdate() {
		if (Player.HeldItem.type != ModContent.ItemType<KnifeRevolver>()) {
			return;
		}
		if (Main.mouseRight) {
			Player.HeldItem.UseSound = SoundID.Item1;
		}
		else {
			Player.HeldItem.UseSound = SoundID.Item40;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource<KnifeRevolver>() && shootCounter >= 3) {
			if (target.HasBuff<DeathshotMark>()) {
				SpecialShotReady = true;
				target.DelBuff(target.FindBuffIndex(ModContent.BuffType<DeathshotMark>()));
			}
			else {
				target.AddBuff<DeathshotMark>(120);
			}
		}
	}
	public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		if (SpecialShotReady && Player.HeldItem.type == ModContent.ItemType<KnifeRevolver>() && Player.altFunctionUse != 2) {
			type = ModContent.ProjectileType<KnifeRevolverP>();
			damage *= 5;
			SpecialShotReady = false;
		}
		shootCounter = ModUtils.Safe_SwitchValue(shootCounter, 4);
	}
}
