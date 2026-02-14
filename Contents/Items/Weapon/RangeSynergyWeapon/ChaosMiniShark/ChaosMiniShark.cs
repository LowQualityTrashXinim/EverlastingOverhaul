using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.ChaosMiniShark {
	internal class ChaosMiniShark : SynergyModItem {
		int counter = 0;
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.Megashark, $"[i:{ItemID.Megashark}] Shoot additional bullet?");
		}
		public override void SetDefaults() {
			Item.BossRushDefaultRange(72, 32, 24, 2f, 6, 6, ItemUseStyleID.Shoot, ProjectileID.Bullet, 10f, true, AmmoID.Bullet);
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.buyPrice(platinum: 5);
			Item.UseSound = SoundID.Item11;
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.Megashark);
		}
		public override bool CanConsumeAmmo(Item ammo, Player player) {
			return Main.rand.NextFloat() >= 0.43f;
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			velocity = ModUtils.RoguelikeSpread(velocity, 10);
			position = position.PositionOFFSET(velocity, 70);
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			counter++;
			if (counter >= 2) {
				for (int i = 0; i < 2 + Main.rand.Next(9); i++) {
					int type2 = Main.rand.Next(new int[] { ProjectileID.StarCannonStar, ProjectileID.BookOfSkullsSkull, ProjectileID.ClothiersCurse, ProjectileID.GiantBee, ProjectileID.Bee, ProjectileID.Grenade, ProjectileID.BallofFire, ProjectileID.WaterBolt, ProjectileID.DemonScythe, ProjectileID.IceBolt, ProjectileID.EnchantedBeam, ProjectileID.BoneGloveProj });
					Vector2 velocity2 = velocity.RotatedByRandom(MathHelper.ToRadians(10));
					Projectile.NewProjectile(source, position, velocity2, type2, damage, knockback, player.whoAmI);
					counter = 0;
				}
			}
			if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Megashark)) {
				Projectile.NewProjectile(source, position, velocity, Main.rand.Next(TerrariaArrayID.Bullet), damage, knockback, player.whoAmI);
			}
			CanShootItem = true;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-4, 0);
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.Minishark)
				.AddIngredient(ItemID.FlowerofFire)
				.AddIngredient(ItemID.WaterBolt)
				.AddIngredient(ItemID.BookofSkulls)
				.AddIngredient(ItemID.DemonScythe)
				.AddIngredient(ItemID.EnchantedSword)
				.AddIngredient(ItemID.IceBlade)
				.AddIngredient(ItemID.Grenade)
				.AddIngredient(ItemID.BeeGun)
				.AddIngredient(ItemID.StarCannon)
				.Register();
		}
	}
}
