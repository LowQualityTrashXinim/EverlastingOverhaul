using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.ItemOverhaul;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.NoneSynergy.EnhancedKatana {
	internal class Katahanced : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultMeleeShootCustomProjectile(50, 52, 43, 4, 20, 20, ItemUseStyleID.Swing, ModContent.ProjectileType<KatanaSlash>(), 3, true);
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(gold: 50);
			Item.UseSound = SoundID.Item1;
			if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul melee)) {
				melee.SwingType = BossRushUseStyle.Swipe;
				melee.UseSwipeTwo = true;
			}
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, Item.Size.Length());
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, player.GetModPlayer<MeleeOverhaulPlayer>().ComboNumber);
			return false;
		}
	}
	public class KatanaSlash : ModProjectile {
		protected virtual float HoldoutRangeMax => 50f;
		protected virtual float HoldoutRangeMin => 10f;
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 10;
		}
		public override void SetDefaults() {
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 80;
			Projectile.height = 104;
			Projectile.penetrate = -1;
			Projectile.light = 0.1f;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.wet = false;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
		}
		Vector2 offset = Vector2.Zero;
		public override void AI() {
			SelectFrame();
			Projectile.ai[1]++;
			var player = Main.player[Projectile.owner];
			player.heldProj = Projectile.whoAmI;
			int duration = player.itemAnimationMax;
			if (Projectile.timeLeft > duration) {
				Projectile.timeLeft = duration;
				offset = Projectile.Center - player.MountedCenter;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.ai[0] % 2 == 0) {
				if (Projectile.timeLeft == duration) {
					Projectile.spriteDirection = -Projectile.spriteDirection;
				}
				Projectile.rotation += MathHelper.ToRadians(180);
			}
			Projectile.velocity = Vector2.Normalize(Projectile.velocity);
			float halfDuration = duration * .5f;
			float progress = (duration - Projectile.timeLeft) / halfDuration;
			Projectile.Center = player.MountedCenter + offset + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
			if (Projectile.ai[1] >= 5) {
				Projectile.scale -= 0.03f;
				Projectile.alpha += 20;
			}
		}
		public void SelectFrame() {
			if (++Projectile.frameCounter >= 1) {
				Projectile.frameCounter = 0;
				Projectile.frame += 1;
				if (Projectile.frame >= Main.projFrames[Projectile.type]) {
					Projectile.frame = Main.projFrames[Projectile.type] - 1;
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		}
	}
}
