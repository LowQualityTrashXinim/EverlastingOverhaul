using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Projectiles;

/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// Ai2 : Do not touch ai2
/// </summary>
public class SimplePiercingProjectile : ModProjectile
{
    public Color ProjectileColor = Color.White;
    public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 36;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 60;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 40;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
    }
    public override void OnSpawn(IEntitySource source)
    {
        if (Projectile.ai[0] <= 0)
        {
            Projectile.ai[0] = 1;
        }
        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.ai[0];
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Projectile.ai[1] <= 0)
        {
            Projectile.ai[1] = 15;
        }
        Projectile.timeLeft = (int)Projectile.ai[1];
    }
    public override Color? GetAlpha(Color lightColor)
    {
        ProjectileColor.A = 0;
        return ProjectileColor * Projectile.ai[2];
    }
    public override void AI()
    {
        Projectile.ai[2] = Projectile.timeLeft / Projectile.ai[1];
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
        Texture2D texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
        Vector2 origin = texture.Size() * .5f;
        Vector2 drawPos = Projectile.position - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
        Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        //DrawTrail2(texture, lightColor, origin);
        return false;
    }
    public void DrawTrail2(Texture2D texture, Color color, Vector2 origin)
    {
        for (int k = 0; k < Projectile.oldPos.Length; k++)
        {
            Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
            color = color * .45f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
            Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(color), Projectile.oldRot[k], origin, (Projectile.scale - k / 100f) * .5f, SpriteEffects.None, 0);
        }
    }
}
/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// Ai2 : Re-adjust scaleX of the projectile
/// </summary>
public class SimplePiercingProjectile2 : ModProjectile
{
    public Color ProjectileColor = Color.White;
    public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
    float InitialScaleXValue = 0f;
    public float ScaleX = 3f;
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 36;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 60;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 40;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.scale = .25f;
    }
    public override void OnSpawn(IEntitySource source)
    {
        if (Projectile.ai[2] != 0)
        {
            ScaleX = Projectile.ai[2];
        }
        InitialScaleXValue = ScaleX;
        if (Projectile.ai[0] <= 0)
        {
            Projectile.ai[0] = 1;
        }
        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.ai[0];
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Projectile.ai[1] <= 0)
        {
            Projectile.ai[1] = 15;
        }
        Projectile.timeLeft = (int)Projectile.ai[1];
    }
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        Vector2 pointEdgeOfProjectile = Projectile.Center.IgnoreTilePositionOFFSET(Projectile.rotation.ToRotationVector2(), 18 * ScaleX);
        Vector2 pointEdgeOfProjectile2 = Projectile.Center.IgnoreTilePositionOFFSET((Projectile.rotation + MathHelper.Pi).ToRotationVector2(), 18 * ScaleX);
        return ModUtils.Collision_PointAB_EntityCollide(targetHitbox, pointEdgeOfProjectile, pointEdgeOfProjectile2);
    }
    public override Color? GetAlpha(Color lightColor)
    {
        ProjectileColor.A = 0;
        return ProjectileColor * (Projectile.timeLeft / Projectile.ai[1]);
    }
    public override void AI()
    {
        ScaleX = InitialScaleXValue * (Projectile.timeLeft / (float)Projectile.Get_ProjectileTimeInitial());
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
        Texture2D texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
        Vector2 origin = texture.Size() * .5f;
        Vector2 drawPos = Projectile.position - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
        Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, new Vector2(ScaleX, Projectile.scale), SpriteEffects.None, 0);
        return false;
    }
}
internal class StarWarSwordProjectile : ModProjectile
{
    public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.BluePhaseblade);
    public int ItemTextureID = ItemID.BluePhaseblade;
    public Color ColorOfSaber = Color.White;
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 50;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.width = 48;
        Projectile.height = 48;
        Projectile.timeLeft = ModUtils.ToSecond(.5f) * 30;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.extraUpdates = 30;
    }
    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        float length = Projectile.width * .5f;
        Projectile.rotation = MathHelper.ToRadians(-Projectile.timeLeft) * 5;
        if (Projectile.timeLeft <= ModUtils.ToSecond(0.4f) * 30)
        {
            Projectile.velocity *= .99f;
        }
        Projectile.scale = Math.Clamp(1 * (Projectile.timeLeft / 200f), 0, 1);
        ColorOfSaber.A = 0;
        Dust dust = Dust.NewDustDirect(Projectile.Center.PositionOFFSET((Projectile.rotation - MathHelper.PiOver4).ToRotationVector2(), Projectile.width * .7f * Projectile.scale) - Vector2.One * 2.5f, 0, 0, DustID.WhiteTorch, newColor: ColorOfSaber);
        dust.noGravity = true;
        dust.velocity = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * Main.rand.NextFloat(2, 3);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.immune[Projectile.owner] = 5;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadItem(ItemTextureID);
        Texture2D texture = TextureAssets.Item[ItemTextureID].Value;
        Vector2 origin = texture.Size() * .5f;
        for (int k = 0; k < Projectile.oldPos.Length; k++)
        {
            Vector2 drawPos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
            Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * .5f;
            color.A = (byte)Projectile.alpha;
            Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], origin, Projectile.scale, SpriteEffects.None, 0);
        }
        return false;
    }
}
