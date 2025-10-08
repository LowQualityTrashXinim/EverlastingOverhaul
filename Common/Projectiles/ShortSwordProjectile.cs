using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Texture;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.Projectiles;
internal class ShortSwordProjectile : ModProjectile {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = true;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.timeLeft = 120;
	}
	public int ItemIDtextureValue = ItemID.CopperShortsword;

	public override void AI() {
		Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.timeLeft / 120f);
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		if (++Projectile.ai[0] >= 20) {
			Projectile.velocity.Y += .5f;
		}
	}
	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Projectile.type);
		Texture2D texture = ModContent.Request<Texture2D>(ModUtils.GetVanillaTexture<Item>(ItemIDtextureValue)).Value;
		Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
		Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
		Main.EntitySpriteDraw(texture, drawPos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		return false;
	}
}
