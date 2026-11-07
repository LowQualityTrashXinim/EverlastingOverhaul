using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_GrapeJuice : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.GrapeJuice;
	public override byte Tier() => 2;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(7);
	public override int EnergyAmount() => 550;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.5f);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_GrapeJuice_Buff>(), ModUtils.ToMinute(37));
	}
}
public class EverlastingOverhaul_GrapeJuice_Buff : FoodItemTier3 {
	public override int TypeID => ItemID.GrapeJuice;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<EverlastingOverhaul_GrapeJuice_ModPlayer>().GrapeJuice = true;
	}
}
public class EverlastingOverhaul_GrapeJuice_ModPlayer : ModPlayer {
	public bool GrapeJuice = false;
	public int Cooldown = 0;
	public override void ResetEffects() {
		GrapeJuice = false;
		Cooldown = ModUtils.CountDown(Cooldown);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		OnHitEffect();
	}
	public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
		OnHitEffect();
	}
	private void OnHitEffect() {
		if (GrapeJuice && Cooldown <= 0) {
			int damage = Player.statLifeMax2 / 4 + 5;
			for (int i = 0; i < 8; i++) {
				Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.One.Vector2DistributeEvenlyPlus(8, 360, i) * 25, ModContent.ProjectileType<EverlastingOverhaul_GrapeJuice_ModProjectile>(), damage, 1, Player.whoAmI);
			}
			Cooldown = 300;
		}
	}
}
public class EverlastingOverhaul_GrapeJuice_ModProjectile : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 600;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 10;
	}
	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		Player player = Main.player[Projectile.owner];
		if (player.Center.Y > Projectile.Center.Y) {
			fallThrough = true;
		}
		else {
			fallThrough = false;
		}
		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}
	public override void AI() {
		Projectile.velocity = Projectile.velocity.LimitedVelocity(20);
	}
	public override Color? GetAlpha(Color lightColor) {
		return Color.MediumPurple;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Player player = Main.player[Projectile.owner];
		target.Center.LookForHostileNPC(out List<NPC> npclist, 300);
		foreach (NPC npc in npclist) {
			if (npc.whoAmI == target.whoAmI) {
				continue;
			}
			player.StrikeNPCDirect(target, hit);
		}
	}
	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = -oldVelocity.X;
		}
		if (Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = -oldVelocity.Y;
		}
		return false;
	}
}
