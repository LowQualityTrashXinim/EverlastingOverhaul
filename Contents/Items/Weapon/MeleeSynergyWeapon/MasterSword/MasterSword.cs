using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.ItemOverhaul;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.MasterSword;
internal class MasterSword : SynergyModItem {
	public override void SetDefaults() {
		Item.BossRushDefaultMeleeShootCustomProjectile(62, 62, 56, 7f, 47, 47, ItemUseStyleID.Swing, 1, 1, true);
		var global = Item.GetGlobalItem<MeleeWeaponOverhaul>();
		global.SwingType = BossRushUseStyle.SwipeDown;
		global.SwingStrength = 11f;
	}
	int ComboChain_ResetTimer = 0;
	int ComboChain_Count = 0;
	public override bool CanUseItem(Player player) {
		if (!player.ItemAnimationActive) {
			var overhaul = Item.GetGlobalItem<MeleeWeaponOverhaul>();
			ComboChain_ResetTimer = 60 + player.itemAnimationMax;
			switch (++ComboChain_Count) {
				case 1:
					overhaul.HideSwingVisual = false;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					overhaul.SwingStrength = 11;
					overhaul.SwingDegree = 140;
					overhaul.OffSetAnimationPercentage = 1;
					break;
				case 2:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					break;
				case 3:
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					break;
				case 4:
					overhaul.HideSwingVisual = true;
					overhaul.SwingType = BossRushUseStyle.Thrust_Style1;
					overhaul.SwingStrength = 15;
					overhaul.IframeDivision = 1;
					overhaul.DistanceThrust = 10;
					break;
				case 5:
					overhaul.HideSwingVisual = false;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					overhaul.SwingStrength = 11;
					break;
				case 6:
					overhaul.SwingType = BossRushUseStyle.SwipeUp;
					break;
				case 7:
					overhaul.OffSetAnimationPercentage = 3;
					overhaul.SwingDegree = 160;
					overhaul.SwingType = BossRushUseStyle.SwipeDown;
					break;
			}
			if (ComboChain_Count >= 7) {
				ComboChain_Count = 0;
			}
		}
		return base.CanUseItem(player);
	}
	public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) {
		CanShootItem = false;
	}
	public override void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) {
		if (--ComboChain_ResetTimer <= 0) {
			ComboChain_Count = 0;
			ComboChain_ResetTimer = 0;
		}
		if (!player.ItemAnimationActive) {
			return;
		}
		if (ComboChain_Count == 4) {
			var distance = Main.MouseWorld - player.Center;
			var vel = distance.SafeNormalize(Vector2.Zero);
			var pos = player.Center + vel.Vector2RotateByRandom(30) * Main.rand.NextFloat(-30, 50);
			var projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), pos, vel * 10,
				ModContent.ProjectileType<SwordProjectile2>(), player.GetWeaponDamage(Item), 1, player.whoAmI);
			if (projectile.ModProjectile is SwordProjectile2 spear) {
				spear.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
			}
			projectile.scale = .77f;
		}
		if (player.itemAnimation == player.itemAnimationMax) {
			switch (ComboChain_Count) {
				case 1:
					BasicGhostSwordSwing(player, 1);
					break;
				case 2:
					BasicGhostSwordSwing(player, -1);
					break;
				case 3:
					CircleSwordAttack(player);
					break;
				case 5:
					UniversalSplitter(player);
					break;
				case 6:
					var distance = Main.MouseWorld - player.Center;
					var vel = distance.SafeNormalize(Vector2.Zero);
					int len = TerrariaArrayID.AllWoodSword.Length;
					for (int i = 0; i < len; i++) {
						var pos = player.Center + vel.Vector2DistributeEvenlyPlus(len, 180, i) * 60;
						int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel * 10, ModContent.ProjectileType<SwordProjectile2>(), player.GetWeaponDamage(Item), 1f, player.whoAmI);
						if (Main.projectile[projec].ModProjectile is SwordProjectile2 spear) {
							spear.ItemIDtextureValue = TerrariaArrayID.AllWoodSword[i];
						}
					}
					break;
				case 0:
					WoodSwordAttack(player);
					break;
			}
		}
	}
	private void UniversalSplitter(Player player) {
		int len = TerrariaArrayID.AllWoodSword.Length;
		int directionOfLooking = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, Main.MouseWorld.X);
		for (int i = 0; i < len; i++) {
			var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center.Add(0, 30 * i), Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI);
			if (proj.ModProjectile is SwordProjectile woodproj) {
				woodproj.directionLooking = directionOfLooking;
				woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
				woodproj.Set_TimeLeft = player.itemAnimationMax;
				woodproj.oldCenter = player.Center;
			}
			proj.ai[2] = 60 + i * 90;
			proj.rotation = -MathHelper.PiOver4;
		}
	}
	private void BasicGhostSwordSwing(Player player, int direction) {
		int directionOfLooking = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, Main.MouseWorld.X);
		var toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 50;
		int proj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI);
		if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj) {
			woodproj.directionLooking = directionOfLooking * direction;
			woodproj.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
		}
		Main.projectile[proj].ai[2] = 120;
	}
	private void CircleSwordAttack(Player player) {
		int directionOfLooking = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, Main.MouseWorld.X);
		var toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 50;
		int len = TerrariaArrayID.AllWoodSword.Length;
		for (int i = 0; i < len; i++) {
			int proj = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI);
			if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj) {
				woodproj.ItemIDtextureValue = TerrariaArrayID.AllWoodSword[i];
				woodproj.rotationSwing = 150 + i * (360 / (float)len);
				woodproj.directionLooking = directionOfLooking * (ComboChain_Count % 2 == 0 ? -1 : 1);
				woodproj.Set_TimeLeft = 60;
				woodproj.Set_AnimationTimeEnd = 30;
			}
			Main.projectile[proj].usesLocalNPCImmunity = true;
			Main.projectile[proj].localNPCHitCooldown = 5;
			Main.projectile[proj].ai[2] = 120;
		}
	}
	private void WoodSwordAttack(Player player) {
		int damage = player.GetWeaponDamage(Item);
		float knockback = player.GetWeaponKnockback(Item);
		for (int i = 0; i < 40; i++) {
			var pos = new Vector2(Main.MouseWorld.X + Main.rand.NextFloat(-100, 100), player.Center.Y - 1000 - 100 * i);
			var vel = Vector2.UnitY.Vector2RotateByRandom(5) * 20;
			int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), damage, knockback, player.whoAmI, 1);
			if (Main.projectile[projec].ModProjectile is SwordProjectile2 spear) {
				spear.ItemIDtextureValue = Main.rand.Next(TerrariaArrayID.AllWoodSword);
			}
		}
	}
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.WoodenSword)
			.AddIngredient(ItemID.RichMahoganySword)
			.AddIngredient(ItemID.BorealWoodSword)
			.AddIngredient(ItemID.EbonwoodSword)
			.AddIngredient(ItemID.ShadewoodSword)
			.AddIngredient(ItemID.AshWoodSword)
			.AddIngredient(ItemID.PearlwoodSword)
			.Register();
	}
}
