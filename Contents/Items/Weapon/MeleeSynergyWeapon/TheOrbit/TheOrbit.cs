using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.General;
using EverlastingOverhaul.Common.Utils;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.TheOrbit;
internal class TheOrbit : SynergyModItem
{
    public override void SetDefaults()
    {
        Item.BossRushDefaultMeleeCustomProjectile(32, 32, 23, 4f, 38, 38, ItemUseStyleID.Swing, ModContent.ProjectileType<TheOrbitProjectile>(), true);
        Item.noUseGraphic = true;
        Item.shootSpeed = 16;
        Item.UseSound = SoundID.Item1;
    }
    int counter = 0;
    public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
    {
        CanShootItem = false;
        int valid = 1;
        if (counter == 3)
        {
            valid = 3;
        }
        Projectile.NewProjectile(source, position, velocity, type, damage + (int)(counter % 2 == 1 ? damage * .5f : 0), knockback, player.whoAmI, valid, counter % 2);
        counter = ModUtils.Safe_SwitchValue(counter, 3);
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.EnchantedBoomerang)
            .AddIngredient(ItemID.FlamingMace)
            .Register();
    }
}
public class TheOrbitProjectile : ModProjectile
{
    public override string Texture => ModUtils.GetTheSameTextureAsEntity<TheOrbit>();
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 32;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 999;
        Projectile.penetrate = -1;
    }
    public override void AI()
    {
        var player = Main.player[Projectile.owner];
        if (Projectile.timeLeft == 999)
        {
            for (int i = 0; i < Projectile.ai[0]; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<OrbitProjectile>(), (int)(Projectile.damage * .55f), 0, player.whoAmI, Projectile.whoAmI, Projectile.ai[0], i);
            }
        }
        float distance = 450;
        Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 10);
        if (Projectile.timeLeft <= 100)
        {
            Projectile.timeLeft += 360;
        }
        if (!Projectile.Center.IsCloseToPosition(player.Center, distance) || Projectile.ai[2] == 1)
        {
            Projectile.ai[2] = 1;
            var velto = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
            if (Projectile.Center.IsCloseToPosition(player.Center, distance))
            {
                Projectile.velocity = velto * 15;
            }
            else
            {
                Projectile.velocity += velto;
                Projectile.velocity = Projectile.velocity.LimitedVelocity(15f);
            }
            if (Projectile.Center.IsCloseToPosition(player.Center, 30))
            {
                Projectile.Kill();
            }
        }
    }
    public override void OnKill(int timeLeft)
    {
        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TheOrbitTheProjectile>(), Projectile.damage / 4 + 1, 0, Projectile.owner);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        if (Projectile.ai[1] == 1)
        {
            Projectile.ProjectileDefaultDrawInfo(out var texture, out var origin);
            var drawpos = Projectile.position - Main.screenPosition + origin;
            Main.EntitySpriteDraw(texture, drawpos, null, new(255, 255, 255, 0), Projectile.rotation, origin, 1.3f, SpriteEffects.None);
        }
        Projectile.DrawTrail(lightColor * .25f, .05f);
        return base.PreDraw(ref lightColor);
    }
}
public class TheOrbitTheProjectile : ModProjectile
{
    public override string Texture => ModUtils.GetTheSameTextureAsEntity<TheOrbit>();
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 32;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 300;
        Projectile.penetrate = -1;
        Projectile.light = 1f;
    }
    public override void AI()
    {
        if (Projectile.ai[1] == 0)
        {
            Projectile.ai[0] = Main.rand.Next(0, 360);
            Projectile.ai[1] = Main.rand.Next(50, 150);
            Projectile.ai[2] = Main.rand.NextFloat(3, 5);
        }
        var player = Main.player[Projectile.owner];
        Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 10);
        Projectile.Center = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.ai[0] + Projectile.timeLeft * Projectile.ai[2])) * Projectile.ai[1];
    }
    public override Color? GetAlpha(Color lightColor)
    {
        if (Projectile.timeLeft <= 60)
        {
            float progress = Projectile.timeLeft / 60f;
            Projectile.alpha = (byte)(255 * progress);
            lightColor = lightColor.ScaleRGB(progress);
            lightColor.A = (byte)(Projectile.alpha);
        }
        return lightColor;
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Projectile.ProjectileDefaultDrawInfo(out var texture, out var origin);
        var player = Main.player[Projectile.owner];
        var drawpos = Projectile.position - Main.screenPosition + origin;
        Color color = Color.White;
        color.A = 0;
        if (Projectile.timeLeft <= 60)
        {
            color = color.ScaleRGB(Projectile.timeLeft / 60f);
        }
        Main.EntitySpriteDraw(texture, drawpos, null, color, Projectile.rotation, origin, 1.3f, SpriteEffects.None);

        Vector2[] oldPosCached = new Vector2[Projectile.oldPos.Length];
        Array.Copy(Projectile.oldPos, oldPosCached, oldPosCached.Length);
        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            Projectile.oldPos[i] = player.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(Projectile.ai[0] + (Projectile.timeLeft + i + 1) * Projectile.ai[2])) * Projectile.ai[1] - origin;
        }
        Projectile.DrawTrailWithoutAlpha(color.ScaleRGB(.45f), .05f);
        Array.Copy(oldPosCached, Projectile.oldPos, oldPosCached.Length);
        lightColor = Projectile.GetAlpha(lightColor);

        return base.PreDraw(ref lightColor);
    }
}
public class OrbitProjectile : ModProjectile
{
    public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.FlamingMace);
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 16;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 999;
        Projectile.penetrate = -1;
    }
    public override void AI()
    {
        int Projectile_WhoAmI = (int)Projectile.ai[0];
        Projectile.rotation = MathHelper.ToRadians(Projectile.timeLeft * 10);
        if (Projectile.timeLeft <= 100)
        {
            Projectile.timeLeft += 360;
        }
        var proj = Main.projectile[Projectile_WhoAmI];
        Projectile.Center = proj.Center + Vector2.One.RotatedBy(MathHelper.ToRadians(360f / Projectile.ai[1] * Projectile.ai[2] + Projectile.timeLeft * 10)) * 50;
        for (int i = 0; i < 10; i++)
        {
            var dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
            dust.noGravity = true;
            dust.position += Main.rand.NextVector2Circular(16, 16);
        }
        if (!proj.active)
        {
            Projectile.Kill();
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.OnFire, ModUtils.ToSecond(Main.rand.Next(1, 9)));
    }
}
