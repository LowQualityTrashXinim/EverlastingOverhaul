using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles.Rework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_EnchantedBoomerang : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.EnchantedBoomerang;
	public override void SetDefaults(Item entity) {
		entity.shoot = ModContent.ProjectileType<EverlastingOverhaul_EnchantedBoomerang_ModProjectile>();
		entity.useTime = entity.useAnimation = 30;
		entity.damage = 30;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, $"EverlastingOverhaulOverhaul_{item.Name}", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
	}
	public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (OutroEffect_ModPlayer.Check_IntroEffect(player, item.type)) {
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 1);
			return false; 
		}
		return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
	}
	public override void HoldItem(Item item, Player player) {
		if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player)) {
			OutroEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(3));
		}
	}
	public override bool AltFunctionUse(Item item, Player player) => true;
}
public class EverlastingOverhaul_EnchantedBoomerang_ModProjectile : Rework_Boomerang {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.EnchantedBoomerang);
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.EnchantedBoomerang);
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 900;
	}
	public override void Boomerang_SpecialAltClickInteraction(Player player) {
		if (player.HeldItem.type == ItemID.EnchantedBoomerang) {
			CanShoot = true;
			ReturnToPlayer = false;
			Projectile.tileCollide = true;
			GoToPosition = Main.MouseWorld - Projectile.Center + Projectile.Center;
		}
	}
	public override bool Boomerang_OnCloseToPlayer(Player player) {
		return Projectile.ai[0] != 1;
	}
	public override bool PreAI() {
		Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), 0, 0, Main.rand.Next(new int[] { DustID.Enchanted_Gold, DustID.Enchanted_Pink }), newColor: Color.White with { A = 0 });
		dust.noGravity = true;
		dust.velocity = Vector2.Zero;
		dust.scale += Main.rand.NextFloat(.1f, .4f);
		dust.rotation += Main.rand.NextFloat(MathHelper.PiOver2);
		return base.PreAI();
	}
	public override void Boomerang_OnShoot(Player player) {
		SoundEngine.PlaySound(SoundID.Item43 with { Volume = .2f });
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * 10, ModContent.ProjectileType<EverlastingOverhaul_EnchantedBoomerang_Beam_ModProjectile>(),
			Projectile.damage, Projectile.knockBack, Projectile.owner);
	}
}
public class EverlastingOverhaul_EnchantedBoomerang_Beam_ModProjectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.EnchantedBoomerang);
	public override void SetDefaults() {
		Projectile.CloneDefaults(ProjectileID.EnchantedBoomerang);
		Projectile.aiStyle = -1;
		Projectile.tileCollide = true;
		Projectile.penetrate = 3;
		Projectile.timeLeft = 900;
	}
	public override void AI() {
		Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(15, 15), 0, 0, Main.rand.Next(new int[] { DustID.Enchanted_Gold, DustID.Enchanted_Pink }), newColor: Color.White with { A = 0 });
		dust.noGravity = true;
		dust.velocity = Vector2.Zero;
		dust.scale += Main.rand.NextFloat(.1f, .3f);
		dust.rotation += Main.rand.NextFloat(MathHelper.PiOver2);
		Projectile.rotation += MathHelper.ToRadians(30);
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Vector2 drawpos = Projectile.Center - Main.screenPosition;
		Vector2 origin = texture.Size() * .5f;
		Main.EntitySpriteDraw(texture, drawpos, null, Color.Cyan with { A = 0 }, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		return false;
	}
}
