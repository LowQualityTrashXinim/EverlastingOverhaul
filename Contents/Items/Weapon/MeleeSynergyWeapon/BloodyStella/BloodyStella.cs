using Terraria;
using Mono.Cecil;
using Terraria.ID;
using EverlastingOverhaul.Texture;
using Terraria.ModLoader;
using Terraria.GameContent;
using EverlastingOverhaul.Common.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.ItemOverhaul;

namespace EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.BloodyStella;

internal class BloodyStella : SynergyModItem {
	public override void Synergy_SetStaticDefaults() {
		SynergyBonus_System.Add_SynergyBonus(Type, ItemID.EnchantedSword, $"[i:{ItemID.EnchantedSword}] Every 5th swing, you summon a circle of bloody star that inflict [c/ff003d:Infectious Stella]");
	}
	public override void SetDefaults() {
		Item.BossRushSetDefault(52, 52, 37, 5f, 17, 17, ItemUseStyleID.Swing, false);
		Item.DamageType = DamageClass.Melee;
		Item.rare = ItemRarityID.Orange;
		Item.shoot = ModContent.ProjectileType<SmallBloodStar_ModProjectile>();
		Item.shootSpeed = 20;
		Item.value = Item.buyPrice(gold: 50);
		Item.UseSound = SoundID.Item1;
		if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul global)) {
			global.SwingType = BossRushUseStyle.Swipe;
		}
		Item.Set_InfoItem();
	}
	int counter = 0;
	public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
		SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.EnchantedSword);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		if (player.ownedProjectileCounts[type] < 1) {
			Projectile.NewProjectileDirect(source, position, -Vector2.UnitY * 30, type, damage, knockback, player.whoAmI);
		}
		if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.EnchantedSword)) {
			if (++counter >= 5) {
				counter = 0;
				for (int i = 0; i < 20; i++) {
					var vel = velocity.Vector2DistributeEvenlyPlus(20, 360, i);
					Projectile.NewProjectile(source, position.PositionOFFSET(vel, 70), vel, ModContent.ProjectileType<BloodStarProjectile>(), damage, knockback, player.whoAmI);
				}
			}
		}
		var pos = position.Add(Main.rand.NextFloat(-200, 200) + 1000 * player.direction, Main.rand.Next(300, 1000));
		var vel2 = Main.MouseWorld - pos;
		var proj = Projectile.NewProjectileDirect(source, pos, vel2, ModContent.ProjectileType<BloodStarProjectile>(), damage, knockback, player.whoAmI, 20);
		proj.extraUpdates = 2;
		CanShootItem = false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Starfury)
			.AddIngredient(ItemID.BloodRainBow)
			.Register();
	}
}
public class BloodStar_ModPlayer : ModPlayer {
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.HasBuff<InfectiousStella>()) {
			target.Center.LookForHostileNPC(out var npclist, 150);
			foreach (var npc in npclist) {
				Player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(hit.Damage * .5f) + 1, 1));
			}
			for (int i = 0; i < 50; i++) {
				var pos = target.Center + Main.rand.NextVector2CircularEdge(75, 75);
				var blood = Dust.NewDustDirect(pos, 0, 0, DustID.Blood);
				blood.noGravity = true;
				blood.velocity = (pos - target.Center).SafeNormalize(Vector2.Zero) * 3;
				blood.scale = Main.rand.NextFloat(.75f, 2);
			}
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.HasBuff<InfectiousStella>()) {
			target.Center.LookForHostileNPC(out var npclist, 150);
			foreach (var npc in npclist) {
				Player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(hit.Damage * .5f) + 1, 1));
			}
			for (int i = 0; i < 50; i++) {
				var pos = target.Center + Main.rand.NextVector2CircularEdge(75, 75);
				var blood = Dust.NewDustDirect(pos, 0, 0, DustID.Blood);
				blood.noGravity = true;
				blood.velocity = (pos - target.Center).SafeNormalize(Vector2.Zero) * 3;
				blood.scale = Main.rand.NextFloat(.75f, 2);
			}
		}
	}
}
public class BloodStarProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.FallingStar);
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.FallingStar);
		Projectile.aiStyle = -1;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		Projectile.penetrate = 5;
	}
	Vector2 vel = Vector2.Zero;
	public override void AI() {
		if (Projectile.timeLeft > 600) {
			Projectile.timeLeft = 600;
			vel = Projectile.velocity;
			Projectile.velocity = Vector2.Zero;
			if (Projectile.ai[2] == 0) {
				Projectile.ai[2] = 20;
			}
		}
		for (int i = 0; i < 3; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center.Add(4, 4) + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.Blood);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			dust.color = Color.Red with { A = 0 };
		}
		Projectile.rotation += MathHelper.ToRadians(10);
		if (++Projectile.ai[0] > 20) {
			Projectile.velocity = vel.SafeNormalize(Vector2.Zero) * Projectile.ai[2];
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff<InfectiousStella>(Main.rand.Next(30, 90));
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		var texture = TextureAssets.Projectile[Type].Value;
		var origin = texture.Size() * .5f;
		var drawpos = Projectile.Center - Main.screenPosition;
		if (Projectile.velocity != Vector2.Zero) {
			var drawposInner = drawpos.PositionOFFSET(Projectile.velocity, -30);
			var origin2 = origin * .5f;
			float rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			float rotateOrigin = MathHelper.ToRadians(Projectile.timeLeft * 10);
			float extraOffset = MathHelper.PiOver2 + MathHelper.PiOver4;
			var textureExtra = TextureAssets.Extra[ExtrasID.FallingStar].Value;
			var extraOrigin = textureExtra.Size() * .5f;
			Main.EntitySpriteDraw(TextureAssets.Extra[ExtrasID.FallingStar].Value, drawposInner + origin2.RotatedBy(rotateOrigin), null, Color.Red.ScaleRGB(.3f) with { A = 0 }, rotation, extraOrigin, 1f, SpriteEffects.None);
			Main.EntitySpriteDraw(TextureAssets.Extra[ExtrasID.FallingStar].Value, drawposInner + origin2.RotatedBy(rotateOrigin + extraOffset), null, Color.Red.ScaleRGB(.3f) with { A = 0 }, rotation, extraOrigin, 1f, SpriteEffects.None);
			Main.EntitySpriteDraw(TextureAssets.Extra[ExtrasID.FallingStar].Value, drawposInner + origin2.RotatedBy(rotateOrigin + extraOffset * 2), null, Color.Red.ScaleRGB(.3f) with { A = 0 }, rotation, extraOrigin, 1f, SpriteEffects.None);
		}
		Main.EntitySpriteDraw(texture, drawpos, null, Color.Red with { A = 150 }, -Projectile.rotation, origin, 1f, SpriteEffects.None);
		return false;
	}
}
public class InfectiousStella : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 20;
	}
}
public class SmallBloodStar_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetTheSameTextureAs<BloodyStella>("SmallBloodStar");
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 34;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 61;
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (!Shoot) {
			Projectile.Center.Subtract(0, Projectile.height * .6f);
		}
		if (Stop) {
			Shoot = true;
		}
		return false;
	}
	public Vector2 AimedToPosition = Vector2.Zero;
	bool Stop = false;
	bool Shoot = false;
	public override void AI() {
		if (Projectile.timeLeft > 60) {
			AimedToPosition = Main.MouseWorld;
			Projectile.timeLeft = 60;
			return;
		}
		if (Projectile.Center.IsCloseToPosition(AimedToPosition, 20)) {
			Stop = true;
		}
		else {
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
			AimedToPosition.Y = Projectile.Center.Y;
		}
		MovementHandler();
		ShootHandler();
	}
	private void MovementHandler() {
		if (!Stop) {
			var distance = AimedToPosition - Projectile.Center;
			float speed = 30;
			float length = distance.Length();
			if (length <= speed) {
				speed *= (speed - length) / 30f;
			}
			Projectile.velocity.X = distance.SafeNormalize(Vector2.Zero).X * speed;
			Projectile.velocity.Y *= .9f;
		}
		else {
			Projectile.tileCollide = true;
			Projectile.velocity.X = 0;
			if (++Projectile.ai[1] >= 20 && !Shoot) {
				Projectile.velocity.Y += 2;
			}
			else {
				Projectile.velocity *= .9f;
			}
		}
		Projectile.rotation = MathHelper.PiOver4 + Vector2.UnitX.ToRotation() + MathHelper.PiOver2;
	}
	private void ShootHandler() {
		if (!Shoot) {
			return;
		}
		for (int i = 0; i < 10; i++) {
			var blood = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood);
			blood.noGravity = true;
			blood.velocity = Vector2.UnitY.Vector2RotateByRandom(60) * -Main.rand.NextFloat(20, 30);
			blood.scale = Main.rand.NextFloat(.75f, 2);
		}
		var newPos = Projectile.Center.Add(0, Projectile.height * .75f);
		if (++Projectile.ai[0] < 5) {
			return;
		}
		Projectile.ai[0] = 0;
		for (int i = 0; i < 3; i++) {
			Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * .5f, -Vector2.UnitY.Vector2RotateByRandom(30) * Main.rand.NextFloat(10, 20), ProjectileID.BloodArrow, (int)(Projectile.damage * .35f) + 1, 2, Projectile.owner);
		}
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), newPos + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * .5f, -Vector2.UnitY.Vector2RotateByRandom(30) * Main.rand.NextFloat(10, 20), ProjectileID.StarCannonStar, Projectile.damage, 4, Projectile.owner);
	}
}
