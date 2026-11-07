using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common ;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Common;
public class EverlastingOverhaul_Axe : GlobalItem {
	public override void SetDefaults(Item entity) {
		AxeWeaponOverhaul(entity);
	}
	private bool CheckAxeCommon(int type) {
		switch (type) {
			//common ore axe
			case ItemID.CopperAxe:
			case ItemID.TinAxe:
			case ItemID.IronAxe:
			case ItemID.LeadAxe:
			case ItemID.SilverAxe:
			case ItemID.TungstenAxe:
			case ItemID.GoldAxe:
			case ItemID.PlatinumAxe:
			//uncommon ore axe
			case ItemID.BloodLustCluster:
			case ItemID.WarAxeoftheNight:
			case ItemID.MoltenPickaxe:
			case ItemID.MeteorHamaxe:
			//Hardmode ore axe
			case ItemID.CobaltWaraxe:
			case ItemID.PalladiumWaraxe:
			case ItemID.MythrilWaraxe:
			case ItemID.OrichalcumWaraxe:
			case ItemID.AdamantiteWaraxe:
			case ItemID.TitaniumWaraxe:
				return true;
		}
		return false;
	}
	public void AxeWeaponOverhaul(Item item) {
		if (item.axe <= 0 || item.noMelee) {
			return;
		}
		//Attempt to fix weapon size
		if (CheckAxeCommon(item.type)) {
			item.useTime = item.useAnimation = 40;
			item.damage += 13;
			item.scale += .45f;
			item.useTurn = false;
			item.Set_ItemCriticalDamage(1.5f);
			item.DamageType = DamageClass.Melee;
			var global = item.GetGlobalItem<MeleeWeaponOverhaul>();
			global.SwingType = BossRushUseStyle.SwipeDown;
			global.SwingDegree = 155;
			global.Ignore_AttackSpeed = true;
			global.AnimationEndTime = 25;
		}
	}
	public override bool AltFunctionUse(Item item, Player player) {
		if (CheckAxeCommon(item.type)) {
			return true;
		}
		return base.AltFunctionUse(item, player);
	}
	public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target) {
		if (CheckAxeCommon(item.type)) {
			return player.altFunctionUse != 2;
		}
		return base.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target);
	}
	public override void UseStyle(Item item, Player player, Rectangle heldItemFrame) {
		if (CheckAxeCommon(item.type)) {
			if (player.altFunctionUse == 2) {
				item.noUseGraphic = true;
			}
			else {
				item.noUseGraphic = false;
			}
		}
	}
	public override bool CanUseItem(Item item, Player player) {
		if (CheckAxeCommon(item.type)) {
			if (player.altFunctionUse == 2) {
				item.noUseGraphic = true;
			}
			else {
				item.noUseGraphic = false;
			}
			int amount = player.ownedProjectileCounts[ModContent.ProjectileType<TomahawkProjectile>()];
			return amount < 1;
		}
		return base.CanUseItem(item, player);
	}
	public override void HoldItem(Item item, Player player) {
		if (CheckAxeCommon(item.type)) {
			if (player.altFunctionUse == 2 && player.ItemAnimationActive && player.itemAnimation == player.itemAnimationMax) {
				var velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity * 17, ModContent.ProjectileType<TomahawkProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI, ai2: item.type);
			}
		}
	}
}
