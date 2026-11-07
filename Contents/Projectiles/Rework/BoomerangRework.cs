using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Projectiles.Rework;
public class Rework_Boomerang : ModProjectile
{
    public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.EnchantedBoomerang);
    public bool CanShoot = true;
    public Vector2 GoToPosition = Vector2.Zero;
    public bool ReturnToPlayer = false;
    public override void OnSpawn(IEntitySource source)
    {
        GoToPosition = Main.MouseWorld;
    }
    public override sealed bool OnTileCollide(Vector2 oldVelocity)
    {
        if (!ReturnToPlayer)
        {
            Collision.HitTiles(Projectile.Center, Projectile.velocity, 10, 10);
            Projectile.velocity = -oldVelocity;
            SoundEngine.PlaySound(SoundID.Dig);
            ReturnToPlayer = true;
            Boomerang_OnTileCollide();
            return false;
        }
        return false;
    }
    public int HomingStrength = 10;
    /// <summary>
    /// Add your own cool down condition
    /// </summary>
    public virtual void Boomerang_SpecialAltClickInteraction(Player player)
    {

    }
    public virtual void Boomerang_OnTileCollide()
    {

    }
    public override sealed void AI()
    {
        Projectile.rotation += MathHelper.ToRadians(30);
        Player player = Main.player[Projectile.owner];
        if (player.altFunctionUse == 2 && player.ItemAnimationJustStarted)
        {
            Boomerang_SpecialAltClickInteraction(player);
        }
        if (Projectile.Center.IsCloseToPosition(GoToPosition, 30) || ReturnToPlayer)
        {
            if (CanShoot)
            {
                CanShoot = false;
                ReturnToPlayer = true;
                Boomerang_OnShoot(player);
                Projectile.tileCollide = false;
            }
            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(player.Center);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(HomingStrength)).ToRotationVector2() * length;
            if (Projectile.Center.IsCloseToPosition(player.Center, 30))
            {
                Projectile.tileCollide = true;
                if (Boomerang_OnCloseToPlayer(player))
                    Projectile.Kill();
            }
        }
        else
        {
            Projectile.velocity = (GoToPosition - Projectile.Center).SafeNormalize(Vector2.Zero) * 15;
        }
    }
    public virtual void Boomerang_OnShoot(Player player)
    {

    }
    public virtual bool Boomerang_OnCloseToPlayer(Player player)
    {
        return true;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadProjectile(Type);
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Vector2 drawpos = Projectile.Center - Main.screenPosition;
        Vector2 origin = texture.Size() * .5f;
        Main.EntitySpriteDraw(texture, drawpos, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
        return false;
    }
}
