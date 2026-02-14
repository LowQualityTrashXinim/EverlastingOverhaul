using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.MagicBow {
	internal abstract class MagicBow : SynergyModItem {
		int DustType = 0;
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.VampireKnives, $"[i:{ItemID.VampireKnives}] Everytime using this weapon heal you for a random amount ranging from 1 to 50");
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.PlatinumBow, $"[i:{ItemID.PlatinumBow}] Bow will shoot out burst of gem staff projectiles deal 45% weapon damage");
		}
		public override void SetDefaults() {
			MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType);
			Item.BossRushSetDefault(18, 32, damage, 1f, useTime, useTime, ItemUseStyleID.Shoot, true);
			DustType = dustType;
			Item.shoot = shoot;
			Item.shootSpeed = shootspeed;
			Item.mana = mana;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item75;
			Item.DamageType = DamageClass.Magic;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.VampireKnives);
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.PlatinumBow);
		}
		public override sealed void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			for (int i = 0; i < 20; i++) {
				var CircularRan = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(20)) + Main.rand.NextVector2Circular(3f, 3f);
				Dust.NewDustPerfect(position, DustType, CircularRan, 100, default, 0.5f);
			}
			position -= new Vector2(0, 5);
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.VampireKnives)) {
				player.Heal(Main.rand.Next(1, 51));
			}
		}
		public override sealed void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.PlatinumBow)) {
				int amount = Main.rand.Next(3, 9);
				velocity = velocity.SafeNormalize(Vector2.Zero) * 10;
				for (int i = 0; i < amount; i++) {
					Projectile.NewProjectile(source, position, velocity.Vector2RotateByRandom(40) * Main.rand.NextFloat(.5f, 1.1f), Main.rand.Next(TerrariaArrayID.AllGemStafProjectilePHM), (int)(damage * .45f), knockback, player.whoAmI);
				}
			}
		}
		public virtual void MagicBowSetDefault(out int mana, out int shoot, out float shootspeed, out int damage, out int useTime, out int dustType) {
			mana = 1;
			shoot = 1;
			shootspeed = 1;
			damage = 1;
			useTime = 1;
			dustType = 1;
		}
	}
}
