using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.HeartPistol {
	internal class HeartPistol : SynergyModItem {
		public override void Synergy_SetStaticDefaults() {
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.Vilethorn, $"[i:{ItemID.Vilethorn}] Heart projectile inflict venom");
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.CandyCaneSword, $"[i:{ItemID.CandyCaneSword}] Heart projectile are significantly more likely to drop heart");
			SynergyBonus_System.Add_SynergyBonus(Type, ItemID.Musket, $"[i:{ItemID.Musket}] Heart projectile fly much faster, have much more life time and have much tigher spread");
		}
		public override void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) {
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.Vilethorn);
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.CandyCaneSword);
			SynergyBonus_System.Write_SynergyTooltip(ref tooltips, this, ItemID.Musket);
		}
		public override void SetDefaults() {
			Item.BossRushDefaultRange(26, 52, 31, 3f, 5, 25, ItemUseStyleID.Shoot, ModContent.ProjectileType<HeartP>(), 10, false, AmmoID.Bullet);
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item11;
			Item.reuseDelay = 22;
			Item.Set_InfoItem();
		}
		int counter = 0, spreadDifferent = 0;
		public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
			if (player.Check_SwitchedWeapon(Type)) {
				player.AddBuff<HeartPistolPassive>(ModUtils.ToSecond(5));
			}
		}
		public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			bool CheckMusketSynergy = SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Musket);
			if (player.ItemAnimationJustStarted && player.ItemAnimationActive) {
				if (CheckMusketSynergy) {
					spreadDifferent = Main.rand.Next(2, 4) * (5 + Main.rand.Next(3));
				}
				else {
					spreadDifferent = Main.rand.Next(2, 7) * (10 + Main.rand.Next(6));
				}
			}
			position = position.PositionOFFSET(velocity, 30);
			if (++counter < 5) {
				if (CheckMusketSynergy) {
					velocity = velocity.Vector2DistributeEvenlyPlus(4, spreadDifferent, counter - 1) * 1.4f;
				}
				else {
					velocity = velocity.Vector2DistributeEvenlyPlus(4, spreadDifferent, counter - 1);
				}
			}
		}
		public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
			SoundEngine.PlaySound(Item.UseSound, player.Center);
			if (counter >= 5) {
				int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HeartP>(), damage * 2, knockback, player.whoAmI);
				if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Musket)) {
					Main.projectile[proj].timeLeft += 20;
					Main.projectile[proj].velocity *= 1.4f;
				}
				if (player.HasBuff<HeartPistolPassive>()) {
					for (int i = 0; i < 5; i++) {
						if (i == 2) {
							continue;
						}
						Vector2 vel = velocity.Vector2DistributeEvenlyPlus(5, 120, i);
						proj = Projectile.NewProjectile(source, position.PositionOFFSET(vel, 20), vel, ModContent.ProjectileType<HeartP>(), damage * 2, knockback, player.whoAmI);
						if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Musket)) {
							Main.projectile[proj].timeLeft += 20;
							Main.projectile[proj].velocity *= 1.4f;
						}
					}
				}
				counter = 0;
				if (player.GetModPlayer<HeartPistol_ModPlayer>().DamageBucket >= 5000) {
					player.GetModPlayer<HeartPistol_ModPlayer>().DamageBucket = 0;
					Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<HeartPistol_LifeCrystal>(), 100 + damage * 5, knockback, player.whoAmI);
				}
			}
			else {
				int proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<smallerHeart>(), damage, knockback, player.whoAmI, 1);
				Main.projectile[proj].scale = 1.5f;
				if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Musket)) {
					Main.projectile[proj].timeLeft += 20;
				}
			}
			if (player.HasBuff<HeartPistolPassive>()) {
				Vector2 vel = (Main.MouseWorld - position).SafeNormalize(Vector2.Zero) * velocity.Length();
				for (int i = 0; i < 3; i++) {
					if (i == 1) {
						continue;
					}
					int proj = Projectile.NewProjectile(source, position, vel.Vector2DistributeEvenlyPlus(3, 50, i), ModContent.ProjectileType<smallerHeart>(), damage, knockback, player.whoAmI, 1);
					Main.projectile[proj].scale = 1.5f;
					if (SynergyBonus_System.Check_SynergyBonus(Type, ItemID.Musket)) {
						Main.projectile[proj].timeLeft += 20;
					}
				}
			}
			CanShootItem = false;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-2, 0);
		}

		public override void AddRecipes() {
			CreateRecipe()
			.AddIngredient(ItemID.FlintlockPistol)
			.AddIngredient(ItemID.BandofRegeneration)
			.Register();
		}
	}
	public class HeartPistol_ModPlayer : ModPlayer {
		public int DamageBucket = 0;
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (proj.Check_ItemTypeSource<HeartPistol>()) {
				DamageBucket += hit.Damage;
			}
		}
	}
	public class HeartPistolPassive : ModBuff {
		public override void SetStaticDefaults() {
			this.BossRushSetDefaultBuff();
		}
		public override void Update(Player player, ref int buffIndex) {
			player.ModPlayerStats().UpdateHPRegen += .25f;
		}
	}
}
