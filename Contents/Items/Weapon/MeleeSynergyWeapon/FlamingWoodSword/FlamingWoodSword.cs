using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.ItemOverhaul;

namespace EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.FlamingWoodSword;
internal class FlamingWoodSword : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushSetDefault(64, 68, 22, 5f, 30, 30, 1, false);
		Item.DamageType = DamageClass.Melee;

		Item.crit = 5;
		Item.useTurn = false;
		Item.UseSound = SoundID.Item1;
		Item.rare = ItemRarityID.Green;
		Item.value = Item.buyPrice(gold: 50);
		Item.shoot = ProjectileID.WandOfSparkingSpark;
		Item.shootSpeed = 6;

		Item.GetGlobalItem<MeleeWeaponOverhaul>().SwingType = BossRushUseStyle.Swipe;
		Item.GetGlobalItem<MeleeWeaponOverhaul>().UseSwipeTwo = true;
	}
	public override void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire, 180);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		for (int i = 0; i < 7; i++) {
			Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(150, 150), -Vector2.UnitY, type, (int)(damage * 0.45f), knockback, player.whoAmI);
			if (Main.rand.NextBool(5)) {
				Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(150, 150), -Vector2.UnitY, ModContent.ProjectileType<FlamingFireSpark>(), (int)(damage * .85f), knockback, player.whoAmI);
			}
		}
		for (int i = 0; i < 5; i++) {
			Vector2 newvelocity = velocity.Vector2DistributeEvenlyPlus(5, 15, i);
			int proj = Projectile.NewProjectile(source, position, newvelocity, type, damage, knockback, player.whoAmI);
			Main.projectile[proj].extraUpdates += 2;
		}
		CanShootItem = false;
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddRecipeGroup("Wood Sword")
			.AddIngredient(ItemID.WandofSparking)
			.Register();
	}
}
public class FlamingFireSpark : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 10;
		Projectile.tileCollide = true;
		Projectile.light = .5f;
		Projectile.timeLeft = 120;
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.hide = true;
	}
	public override void AI() {
		for (int i = 0; i < 2; i++) {
			int dust = Dust.NewDust(Projectile.Center, 10, 10, DustID.Torch);
			Main.dust[dust].noGravity = true;
		}
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 225)) {
			Projectile.timeLeft = 120;
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 5;
		}
		else {
			Projectile.velocity.Y += .05f;
		}

	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire3, ModUtils.ToSecond(Main.rand.Next(7, 11)));
	}
}
