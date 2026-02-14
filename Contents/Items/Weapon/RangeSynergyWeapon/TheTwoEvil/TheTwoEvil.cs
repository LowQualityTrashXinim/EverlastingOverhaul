using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.BuffAndDebuff;
using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.TheTwoEvil;
internal class TheTwoEvil : SynergyModItem
{
    public override void SetDefaults()
    {
        Item.BossRushDefaultRange(30, 30, 27, 6, 35, 35, ItemUseStyleID.Shoot, ProjectileID.WoodenArrowFriendly, 17f, false, AmmoID.Arrow);
        Item.UseSound = SoundID.Item5;
    }
    public int Counter = 0;
    public int Decay = 0;
    public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer)
    {
        if (++Decay >= 120)
        {
            Counter = 0;
        }
    }
    public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
    {
        CanShootItem = false;
        Decay = -player.itemAnimationMax;
        Vector2 offsetPos = Vector2.UnitY.RotatedBy(velocity.ToRotation());
        Vector2 pos = position.PositionOFFSET(velocity, Item.width);
        Projectile.NewProjectile(source, pos + offsetPos * 10, velocity, type, damage, knockback, player.whoAmI);
        Projectile.NewProjectile(source, pos + offsetPos * -10, velocity, type, damage, knockback, player.whoAmI);
        //Spawning the projectile lmao

        int colorchosing = Main.rand.NextBool().ToInt();
        int color = colorchosing == 0 ? DustID.GemAmethyst : DustID.GemRuby;
        Vector2 newpos = pos + Main.rand.NextVector2CircularEdge(100, 100) * Main.rand.NextFloat(.5f, 1.25f);
        Vector2 vel = (Main.MouseWorld - newpos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(5f, 7f);
        float randomrotation = Main.rand.NextFloat(90);
        Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
        for (int i = 0; i < 4; i++)
        {
            Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * (5 + Main.rand.NextFloat());
            for (int l = 0; l < 12; l++)
            {
                float multiplier = Main.rand.NextFloat();
                float scale = MathHelper.Lerp(1.5f, .1f, multiplier);
                int dust = Dust.NewDust(newpos + randomPosOffset, 0, 0, color, 0, 0, 0, default, scale);
                Main.dust[dust].velocity = Toward * multiplier;
                Main.dust[dust].noGravity = true;
            }
        }
        Projectile.NewProjectile(source, newpos, vel, ModContent.ProjectileType<EvilShot>(), (int)(damage * 1.5f), knockback, player.whoAmI, colorchosing);
        Counter = Math.Clamp(++Counter, 0, 15);
        int amount = Counter / 3;
        for (int i = 0; i < amount; i++)
        {
            Vector2 newpos2 = position + Main.rand.NextVector2CircularEdge(100, 100) * Main.rand.NextFloat(.5f, 1.25f);
            Vector2 newvel2 = (Main.MouseWorld - newpos2).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(4, 6);
            Projectile projectile = Projectile.NewProjectileDirect(source, newpos2, newvel2, ModContent.ProjectileType<EvilShot>(), (int)(damage * .33f) + 1, knockback, player.whoAmI, colorchosing);
            projectile.scale -= .5f;
            projectile.penetrate = 2;
        }
    }
    public override Vector2? HoldoutOffset()
    {
        return new(-3f, 0);
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TendonBow)
            .AddIngredient(ItemID.DemonBow)
            .Register();
    }
}

internal class EvilShot : ModProjectile
{
    public override string Texture => ModTexture.SMALLWHITEBALL;
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 50;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 36;
        Projectile.penetrate = 4;
        Projectile.timeLeft = 1200;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.extraUpdates = 3;
    }
    int dustID => Projectile.ai[0] == 0 ? DustID.GemAmethyst : DustID.GemRuby;
    Color color => Projectile.ai[0] == 0 ? new(255, 50, 255) : Color.Red;
    public override bool? CanDamage()
    {
        return Projectile.penetrate > 1;
    }
    public override void AI()
    {
        if (Projectile.timeLeft <= 60)
        {
            Projectile.ProjectileAlphaDecay(60);
            Projectile.velocity *= .96f;
        }
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (Main.rand.NextBool())
        {
            int dust = Dust.NewDust(Projectile.position, 36, 36, dustID, Scale: Main.rand.NextFloat(.9f, 1.1f));
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].noGravity = true;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Projectile.ai[0] == 0)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 vel = Main.rand.NextVector2CircularEdge(1, 1);
            Projectile.NewProjectile(player.GetSource_OnHit(target), target.Center.PositionOFFSET(vel, -30), vel, ProjectileID.LightsBane, (int)(hit.Damage * .5f), hit.Knockback, player.whoAmI, 1);
        }
        else
        {
            target.AddBuff(ModContent.BuffType<BloodButchererEnchantmentDebuff>(), ModUtils.ToSecond(Main.rand.Next(3, 8)));
        }
        Projectile.timeLeft = 60;
        float randomrotation = Main.rand.NextFloat(90);
        Vector2 randomPosOffset = Main.rand.NextVector2Circular(20f, 20f);
        for (int i = 0; i < 4; i++)
        {
            Vector2 Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i + randomrotation)) * (3 + Main.rand.NextFloat());
            for (int l = 0; l < 8; l++)
            {
                float multiplier = Main.rand.NextFloat();
                float scale = MathHelper.Lerp(1.5f, .1f, multiplier);
                int dust = Dust.NewDust(target.Center + randomPosOffset, 0, 0, dustID, 0, 0, 0, default, scale);
                Main.dust[dust].velocity = Toward * multiplier;
                Main.dust[dust].noGravity = true;
            }
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
        Texture2D texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
        Vector2 origin = texture.Size() * .5f;
        Vector2 drawPos = Projectile.position - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
        Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(color), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        DrawTrail2(texture, origin);
        return false;
    }
    public void DrawTrail2(Texture2D texture, Vector2 origin)
    {
        for (int k = 0; k < Projectile.oldPos.Length; k++)
        {
            Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
            Color color = this.color * .45f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
            Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(color), Projectile.oldRot[k], origin, (Projectile.scale - k / 100f) * .5f, SpriteEffects.None, 0);
        }
    }
}
