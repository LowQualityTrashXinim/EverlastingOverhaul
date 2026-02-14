using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Contents.Projectiles;
using System.Collections.Generic;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
public class Roguelike_PearlWoodSword : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.PearlwoodSword;
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 45;
		entity.scale = 1.5f;
		entity.useTime = entity.useAnimation = 24;
		entity.GetGlobalItem<MeleeWeaponOverhaul>().ShaderOffSetLength += 1;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void HoldItem(Item item, Player player) {
		if (player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive) {
			int damage = player.GetWeaponDamage(item);
			var modplayer = player.GetModPlayer<Roguelike_PearlWoodSword_ModPlayer>();
			if (++modplayer.swingCount >= 3) {
				modplayer.swingCount = 0;
				var distance = Main.MouseWorld - player.Center;
				var vel = distance.SafeNormalize(Vector2.Zero);
				for (int i = 0; i < 6; i++) {
					var pos = player.Center + vel.Vector2DistributeEvenlyPlus(6, 180, i) * 60;
					var projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), pos, vel, ModContent.ProjectileType<pearlSwordProj>(), damage, 1, player.whoAmI);
					projectile.penetrate = 10;
					projectile.maxPenetrate = 10;
				}
			}
			var toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 100;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = item.type;
			Main.projectile[proj].ai[2] = 120;
			if (modplayer.hitCount >= 60) {
				modplayer.hitCount = -30;
				modplayer.duration = 120;
			}
		}
	}
}
public class Roguelike_PearlWoodSword_ModPlayer : ModPlayer {
	public int swingCount = 0;
	public int hitCount = 0;
	public int duration = 0;
	Vector2 lastHitLocation = Vector2.Zero;
	public override void ResetEffects() {
		if (Player.HeldItem.type != ItemID.PearlwoodSword) {
			hitCount = 0;
			swingCount = 0;
		}
		else {
			duration = ModUtils.CountDown(duration);
		}
	}
	public override void UpdateEquips() {
		if (duration > 0 && duration % 5 == 0) {
			int damage = (int)(Player.GetWeaponDamage(Player.HeldItem) * .5f) + 1;
			var pos = lastHitLocation + Main.rand.NextVector2CircularEdge(250, 250) * Main.rand.NextFloat(1, 3);
			var vel = (Main.MouseWorld + Main.rand.NextVector2Circular(20, 20) - pos).SafeNormalize(Vector2.Zero);
			var projectile = Projectile.NewProjectileDirect(Player.GetSource_ItemUse(Player.HeldItem), pos, vel, ModContent.ProjectileType<pearlSwordProj>(), damage, 1, Player.whoAmI);
			projectile.penetrate = 2;
			projectile.maxPenetrate = 2;
		}
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type == ItemID.PearlwoodSword) {
			hitCount++;
			if (duration <= 0)
				lastHitLocation = target.Center;
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (proj.Check_ItemTypeSource(ItemID.PearlwoodSword)) {
			hitCount++;
			if (duration <= 0)
				lastHitLocation = target.Center;
		}
	}
}
