using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.BuffAndDebuff;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class Roguelike_BloodLustCluster : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.BloodLustCluster;
	}
	public override void SetDefaults(Item entity) {
		entity.scale += .25f;
		entity.damage = 70;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, item.type + "_Rework", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff<BloodButchererEnchantmentDebuff>(ModUtils.ToSecond(Main.rand.Next(3, 7)));
	}
	public override void MeleeEffects(Item item, Player player, Rectangle hitbox) {
		if (player.altFunctionUse == 2) {
			return;
		}
		if (player.itemAnimation == player.itemAnimationMax / 2 || player.itemAnimation == player.itemAnimationMax / 3 * 2 || player.itemAnimation == player.itemAnimationMax / 3) {
			if (!Main.rand.NextBool(5)) {
				return;
			}
			Projectile.NewProjectile(player.GetSource_ItemUse(item), Main.rand.NextVector2FromRectangle(hitbox), Vector2.Zero, ModContent.ProjectileType<SentientBlood>(), (int)(item.damage * .77f), .2f, player.whoAmI);
		}
	}
}
public class SentientBlood : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.BloodShot);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 5;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
		Projectile.hide = true;
		Projectile.timeLeft = 300;
	}
	public override void AI() {
		var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood);
		dust.noGravity = true;
		dust.velocity = Main.rand.NextVector2Circular(3, 3);
		dust.scale = Main.rand.NextFloat(.9f, 1.1f);
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 250f)) {
			Projectile.velocity += (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
			Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
		}
		Projectile.velocity *= .99f;
	}
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.ScalingArmorPenetration += 1f;
	}
	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 8; i++) {
			var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Blood);
			dust.noGravity = true;
			dust.velocity = Main.rand.NextVector2Circular(3, 3);
			dust.scale = Main.rand.NextFloat(.9f, 1.1f);
		}
	}
}
