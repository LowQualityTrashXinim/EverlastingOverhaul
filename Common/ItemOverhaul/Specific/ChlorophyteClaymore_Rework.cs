using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;

public class Roguelike_ChlorophyteClaymore : GlobalItem
{
    public static readonly WeaponProgress progress = new()
    {

    };
    public override void SetStaticDefaults()
    {
        progress.Set_Progress(150 / 240f, 170 / 240f, new Color(10, 200, 10));
    }
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.ChlorophyteClaymore;
    }
    public override void SetDefaults(Item entity)
    {
        entity.damage += 20;
        entity.knockBack += 5;
        entity.useTime = entity.useAnimation = 35;
        entity.shootsEveryUse = true;
        entity.Set_ItemOutroEffect<OutroEffect_ChlorophyteEmpowerment>();
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        ModUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_ChlorophyteClaymore", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
    }
    public override void HoldItem(Item item, Player player)
    {
        if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player))
        {
            OutroEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(9));
        }
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.SetWeaponProgress(progress);
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.barProgress = player.GetModPlayer<Roguelike_ChlorophyteClaymore_ModPlayer>().ChlorophyteClaymore_Counter / 240f;
    }
    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        int counter = player.GetModPlayer<Roguelike_ChlorophyteClaymore_ModPlayer>().ChlorophyteClaymore_Counter - 150;
        player.GetModPlayer<Roguelike_ChlorophyteClaymore_ModPlayer>().ChlorophyteClaymore_Counter = -player.itemAnimationMax;
        if (player.GetModPlayer<Roguelike_ChlorophyteClaymore_ModPlayer>().PerfectStrike)
        {
            damage *= 2;
            counter = 90;
        }
        if (counter >= 90)
        {
            int amount = counter / 10;
            for (int i = 0; i < amount; i++)
            {
                Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero).Vector2RotateByRandom(45) * Main.rand.NextFloat(12f, 19f), ProjectileID.ChlorophyteOrb, damage, knockback, player.whoAmI, Main.rand.NextFloat(3, 5), 180);
            }
        }
        if (counter >= 0)
        {
            var proj = Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero) * 15, ModContent.ProjectileType<FlyingSlashProjectile>(), damage * 3, knockback, player.whoAmI);
            if (proj.ModProjectile is FlyingSlashProjectile slash)
            {
                slash.projectileColor = new(90, 255, 90, 0);
            }
            proj.scale += .5f;
            proj.Resize(120, 120);
            return false;
        }
        return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
    }
}
public class Roguelike_ChlorophyteClaymore_ModPlayer : ModPlayer
{
    public int ChlorophyteClaymore_Counter = 0;
    public bool PerfectStrike = false;
    public override void ResetEffects()
    {
        if (!Player.active)
        {
            return;
        }
        if (++ChlorophyteClaymore_Counter > 240)
        {
            ChlorophyteClaymore_Counter = 240;
        }
        if (Player.HeldItem.type != ItemID.ChlorophyteClaymore)
        {
            return;
        }
        PerfectStrike = ChlorophyteClaymore_Counter >= 150 && ChlorophyteClaymore_Counter <= 170;
        if (PerfectStrike && ChlorophyteClaymore_Counter == 150)
        {
            SpawnSpecialDustEffect();
        }
    }
    public void SpawnSpecialDustEffect()
    {
        SoundEngine.PlaySound(SoundID.Item71 with { Pitch = .5f }, Player.Center);
        for (int o = 0; o < 10; o++)
        {
            for (int i = 0; i < 4; i++)
            {
                var Toward = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(90 * i)) * (3 + Main.rand.NextFloat()) * 5;
                for (int l = 0; l < 8; l++)
                {
                    float multiplier = Main.rand.NextFloat();
                    float scale = MathHelper.Lerp(1.1f, .1f, multiplier);
                    int dust = Dust.NewDust(Player.Center.Add(0, -60), 0, 0, DustID.GemDiamond, 0, 0, 0, Color.Green, scale);
                    Main.dust[dust].velocity = Toward * multiplier;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].Dust_GetDust().FollowEntity = true;
                    Main.dust[dust].Dust_BelongTo(Player);
                }
            }
        }
    }
}
public class Roguelike_ChlorophyteClaymore_GlobalProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.type == ProjectileID.ChlorophyteOrb;
    }
    public override void SetDefaults(Projectile entity)
    {
        entity.tileCollide = true;
        entity.timeLeft = 600;
    }
    public override void PostAI(Projectile projectile)
    {
        var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.ChlorophyteWeapon);
        dust.noGravity = true;
        dust.velocity = Vector2.Zero;

    }
    public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
    {
        if (projectile.velocity.Y != oldVelocity.Y)
        {
            projectile.velocity.Y = -oldVelocity.Y;
        }
        if (projectile.velocity.X != oldVelocity.X)
        {
            projectile.velocity.X = -oldVelocity.X;
        }
        Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Main.rand.NextVector2Circular(5, 5), ProjectileID.SporeCloud, projectile.damage / 2, projectile.knockBack, projectile.owner, 5, 180);
        return false;
    }
    public override void OnKill(Projectile projectile, int timeLeft)
    {
        for (int i = 0; i < 5; i++)
        {
            Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Main.rand.NextVector2Circular(5, 5), ProjectileID.SporeCloud, projectile.damage / 2, projectile.knockBack, projectile.owner, 5, 180);
        }
    }
}
/// <summary>
/// Ai0 : shoot velocity<br/>
/// Ai1 : time left of a AI, recommend setting it above 0<br/>
/// Ai2 : Do not touch ai2
/// </summary>
public class ChlorophyteOrb_SimplePiercingTrailProjectile : ModProjectile
{
    public Color ProjectileColor = Color.Green;
    public override string Texture => ModUtils.GetVanillaTexture<Projectile>(ProjectileID.PiercingStarlight);
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 20;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 36;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 60;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.extraUpdates = 2;
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
        ProjectileColor.A = 0;
        Projectile.ai[2] = Projectile.timeLeft / Projectile.ai[1];
        if (Projectile.timeLeft > Projectile.ai[1] * .8f)
        {
            return;
        }
        if (Projectile.Center.LookForHostileNPC(out NPC npc, 700))
        {
            float length = (npc.Center - Projectile.Center).Length() / 32f;
            if (length < 3)
            {
                length = 3;
            }
            Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * length;
            Projectile.velocity = Projectile.velocity.LimitedVelocity(5);
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.timeLeft = 90;
        }
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadProjectile(ProjectileID.PiercingStarlight);
        var texture = TextureAssets.Projectile[ProjectileID.PiercingStarlight].Value;
        var origin = texture.Size() * .5f;
        var drawPos = Projectile.position - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
        Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(ProjectileColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        DrawTrail2(texture, lightColor, origin);
        return false;
    }
    public void DrawTrail2(Texture2D texture, Color color, Vector2 origin)
    {
        for (int k = 0; k < Projectile.oldPos.Length; k++)
        {
            var drawPos = Projectile.oldPos[k] - Main.screenPosition + origin * .5f + new Vector2(0f, Projectile.gfxOffY);
            color = color * .45f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
            Main.EntitySpriteDraw(texture, drawPos, null, Projectile.GetAlpha(ProjectileColor), Projectile.oldRot[k], origin, (Projectile.scale - k * .05f) * .5f, SpriteEffects.None, 0);
        }
    }
}