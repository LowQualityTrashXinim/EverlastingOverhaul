using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.NoneSynergy.OvergrownMinishark {
	internal class OvergrownMinishark : ModItem {
		public override void SetDefaults() {
			Item.BossRushDefaultRange(54, 24, 14, 2f, 11, 11, ItemUseStyleID.Shoot, ProjectileID.Bullet, 15, true, AmmoID.Bullet);
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(gold: 50);
			Item.UseSound = SoundID.Item11;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-4, 0);
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			position = position.PositionOFFSET(velocity, 40);
			velocity = ModUtils.RoguelikeSpread(velocity, 7);
		}
	}
	public class OvergrownMinishark_ModPlayer : ModPlayer {
		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
			if (!proj.Check_ItemTypeSource(ModContent.ItemType<OvergrownMinishark>())) {
				return;
			}
			target.AddBuff(BuffID.Poisoned, 420);
			if (Main.rand.NextBool(10)) {
				float randomRotation = Main.rand.Next(90);
				Vector2 velocity;
				for (int i = 0; i < 6; i++) {
					velocity = proj.velocity.RotatedBy(MathHelper.ToRadians(i * 60 + randomRotation)) * .5f;
					Projectile.NewProjectile(proj.GetSource_FromAI(), proj.Center, velocity, ProjectileID.VilethornTip, (int)(hit.Damage * .35f), hit.Knockback, Player.whoAmI);
					Projectile.NewProjectile(proj.GetSource_FromAI(), proj.Center, velocity, ProjectileID.VilethornBase, (int)(hit.Damage * .35f), hit.Knockback, Player.whoAmI);
				}
			}
		}
	}
}
