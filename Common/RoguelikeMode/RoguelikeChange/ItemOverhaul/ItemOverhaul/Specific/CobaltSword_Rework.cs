using EverlastingOverhaul.Common.Graphics;
using EverlastingOverhaul.Common.Projectiles;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;

public class Roguelike_CobaltSword : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.CobaltSword;
	}
	public override void SetDefaults(Item entity) {
		entity.shoot = ModContent.ProjectileType<SimplePiercingProjectile2>();
		entity.shootSpeed = 1;
		entity.damage += 20;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		tooltips.Add(new(Mod, $"RoguelikeOverhaul_{item.Name}", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		int counter = player.GetModPlayer<Roguelike_CobaltSword_ModPlayer>().CobaltSword_Counter;
		player.GetModPlayer<Roguelike_CobaltSword_ModPlayer>().CobaltSword_Counter = -player.itemAnimationMax;
		if (counter >= 150) {
			if (player.GetModPlayer<Roguelike_CobaltSword_ModPlayer>().PerfectStrike) {
				counter = 150;
			}
			else {
				counter -= 150;
			}
			for (int i = 0; i < 16; i++) {
				var velocityToward = velocity.RotatedBy(MathHelper.PiOver2).Vector2RotateByRandom(55) * Main.rand.NextBool().ToDirectionInt();
				var Swordprojectile = Projectile.NewProjectileDirect(source, position + velocity * item.Size.Length() * (i * .25f), velocityToward, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(damage * .85f + counter), 2f, player.whoAmI, 2f + Main.rand.NextFloat(2), 5 + i, 3 + i * .5f);
				if (Swordprojectile.ModProjectile is SimplePiercingProjectile2 modproj) {
					modproj.ProjectileColor = SwordSlashTrail.averageColorByID[ItemID.CobaltSword] * 2;
					Swordprojectile.scale += .2f;
				}
			}
			return false;
		}
		for (int i = 0; i < 2; i++) {
			var velocityToward = velocity.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).Vector2RotateByRandom(55);
			var Swordprojectile = Projectile.NewProjectileDirect(source, position + velocity * item.Size.Length() * Main.rand.NextFloat(.4f, 1.2f), velocityToward, ModContent.ProjectileType<SimplePiercingProjectile2>(), (int)(damage * .85f), 2f, player.whoAmI, 2f + Main.rand.NextFloat(2));
			if (Swordprojectile.ModProjectile is SimplePiercingProjectile2 modproj) {
				modproj.ProjectileColor = SwordSlashTrail.averageColorByID[ItemID.CobaltSword] * 2;
			}
		}
		return false;
	}
}
public class Roguelike_CobaltSword_ModPlayer : ModPlayer {
	public int CobaltSword_Counter = 0;
	public bool PerfectStrike = false;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		var item = Player.HeldItem;
		CobaltSword_Counter++;
		if (CobaltSword_Counter > 300) {
			CobaltSword_Counter = 300;
		}
		if (item.type != ItemID.CobaltSword) {
			return;
		}
		PerfectStrike = CobaltSword_Counter >= 150 && CobaltSword_Counter <= 165;
		if (PerfectStrike && CobaltSword_Counter == 150) {
			SpawnSpecialCobaltDustEffect();
		}
	}
	public void SpawnSpecialCobaltDustEffect() {
		SoundEngine.PlaySound(SoundID.Item71 with { Pitch = .5f }, Player.Center);
		for (int o = 0; o < 10; o++) {
			for (int i = 0; i < 4; i++) {
				var Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * (3 + Main.rand.NextFloat()) * 5;
				for (int l = 0; l < 8; l++) {
					float multiplier = Main.rand.NextFloat();
					float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
					int dust = Dust.NewDust(Player.Center.Add(0, -60), 0, 0, DustID.GemDiamond, 0, 0, 0, Color.Blue, scale);
					Main.dust[dust].velocity = Toward * multiplier;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].Dust_GetDust().FollowEntity = true;
					Main.dust[dust].Dust_BelongTo(Player);
				}
			}
		}
	}
}
