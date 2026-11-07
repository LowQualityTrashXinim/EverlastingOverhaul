using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Contents.Projectiles;

namespace EverlastingOverhaul.Common .ItemOverhaul.Common;
internal class EverlastingOverhaul_WoodSword : GlobalItem {
	public override bool InstancePerEntity => true;
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return CheckItem(entity.type);
	}
	private bool CheckItem(int type) {
		switch (type) {
			case ItemID.BorealWoodSword:
			case ItemID.PalmWoodSword:
			case ItemID.ShadewoodSword:
			case ItemID.EbonwoodSword:
			case ItemID.RichMahoganySword:
				return true;
		}
		return false;
	}
	int swingCount = 0;
	public override void SetDefaults(Item entity) {
		entity.scale += .45f;
		entity.damage += 10;
		entity.GetGlobalItem<MeleeWeaponOverhaul>().ShaderOffSetLength += 1;
	}
	public override void HoldItem(Item item, Player player) {
		if (player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive) {
			if (++swingCount >= 5) {
				swingCount = 0;
				WoodSwordAttack(item, player);
			}
			var toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 100;
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
				woodproj.ItemIDtextureValue = item.type;
			Main.projectile[proj].ai[2] = 120;
		}
	}
	private void WoodSwordAttack(Item item, Player player) {
		int damage = player.GetWeaponDamage(item);
		float knockback = player.GetWeaponKnockback(item);
		int direction = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, Main.MouseWorld.X);
		for (int i = 0; i < 40; i++) {
			var pos = new Vector2(player.Center.X + (50 * i + 100) * direction, player.Center.Y - 1000 - 100 * i);
			var vel = Vector2.UnitY.Vector2RotateByRandom(5) * 20;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), damage, knockback, player.whoAmI, 1);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 spear) {
				spear.ItemIDtextureValue = item.type;
			}
		}
	}
}
