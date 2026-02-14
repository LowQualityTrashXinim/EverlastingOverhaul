using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.ItemOverhaul;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using EverlastingOverhaul.Texture;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.DrainingVeilBlade;

public class DrainingVeilBlade : SynergyModItem
{
    public override void SetDefaults()
    {
        Item.BossRushDefaultMeleeShootCustomProjectile(80, 83, 145, 1, 50, 50, ItemUseStyleID.Swing, ModContent.ProjectileType<DrainingVeilBlade_Wave_Projectile>(), 10, false);
        if (Item.TryGetGlobalItem(out MeleeWeaponOverhaul melee))
        {
            melee.SwingType = BossRushUseStyle.Swipe;
        }
    }
    public override void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        position = position.PositionOFFSET(velocity, 20);
    }
    int cooldown = 120;
    public override void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer)
    {
        cooldown = ModUtils.CountDown(cooldown);
    }
    public override void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem)
    {
        if (cooldown <= 0)
        {
            Projectile.NewProjectile(source, position + Main.rand.NextVector2CircularEdge(300, 300) * Main.rand.NextFloat(1, 1.5f), Vector2.Zero, ModContent.ProjectileType<DrainingVeilBlade_AfterImage_Projectile>(), damage, knockback, player.whoAmI);
            cooldown = 300 + player.itemAnimationMax;
        }
        base.SynergyShoot(player, modplayer, source, position, velocity, type, damage, knockback, out CanShootItem);
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SoulDrain)
            .AddIngredient(ItemID.ClingerStaff)
            .AddRecipeGroup("Wood Sword")
            .Register();
    }
}
public class DrainingVeilBlade_AfterImage_Projectile : ModProjectile
{
    public override string Texture => ModUtils.GetTheSameTextureAsEntity<DrainingVeilBlade>();
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 25;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 32;
        Projectile.penetrate = -1;
        Projectile.friendly = true;
        Projectile.tileCollide = false;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.timeLeft = 1200;
    }
    public int ItemIDtextureValue = ItemID.WoodenSword;
    Player player;
    Vector2 directionToMouse = Vector2.Zero;
    public int directionLooking = 0;
    Vector2 oldCenter = Vector2.Zero;
    public float rotationSwing = 150;
    public float delay = 0;
    public int Set_AnimationTimeEnd = -1;
    public int AttackAinimationTime = 0;
    float outrotation = 0;
    public override void OnSpawn(IEntitySource source)
    {
        if (Projectile.ai[2] == 0)
        {
            Projectile.ai[2] = 60f;
        }
    }
    public override bool? CanDamage()
    {
        if (player == null)
        {
            return false;
        }
        return player.ItemAnimationActive;
    }
    public override void AI()
    {
        player = Main.player[Projectile.owner];
        if (Projectile.timeLeft == 1200)
        {
            directionToMouse = Projectile.velocity;
            if (directionToMouse == Vector2.Zero)
            {
                directionToMouse = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero);
            }
            oldCenter = Projectile.Center.PositionOFFSET(directionToMouse, -30);
            if (Set_AnimationTimeEnd == -1)
            {
                Set_AnimationTimeEnd = 60;
            }
        }
        if (player.dead || !player.active)
        {
            Projectile.Kill();
            return;
        }
        if (player.ItemAnimationActive && player.itemAnimation == player.itemAnimationMax)
        {
            AttackAinimationTime = player.itemAnimationMax;
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * 10, ModContent.ProjectileType<DrainingVeilBlade_Wave_Projectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -600);
        }
        EnergySword_Code1AI();
    }
    private void EnergySword_Code1AI()
    {
        float percentDone = AttackAinimationTime / (float)Set_AnimationTimeEnd;
        percentDone = Math.Clamp(ModUtils.InOutExpo(percentDone), 0, 1);
        if (--AttackAinimationTime <= 0)
        {
            AttackAinimationTime = 0;
            directionToMouse = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero);
            directionLooking = ModUtils.DirectionFromEntityAToEntityB(Projectile.Center.X, Main.MouseWorld.X);
        }
        if (Projectile.timeLeft <= 100)
        {
            Projectile.ProjectileAlphaDecay(100);
        }
        float baseAngle = directionToMouse.ToRotation();
        float angle = MathHelper.ToRadians(rotationSwing) * directionLooking;
        float start = baseAngle + angle;
        float end = baseAngle - angle;
        float rotation = MathHelper.Lerp(start, end, percentDone);
        outrotation = rotation;
        Projectile.rotation = rotation - MathHelper.PiOver4 + MathHelper.Pi;
        Projectile.velocity.X = directionLooking;
        Projectile.Center = oldCenter + Vector2.UnitX.RotatedBy(rotation) * Projectile.ai[2];
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        Player player = Main.player[Projectile.owner];

        int directionTo = (player.Center.X < target.Center.X).ToDirectionInt();
        modifiers.HitDirectionOverride = directionTo;
    }
    public override void ModifyDamageHitbox(ref Rectangle hitbox)
    {
        ModUtils.ModifyProjectileDamageHitbox(ref hitbox, oldCenter, outrotation, Projectile.width, Projectile.height, Projectile.ai[2]);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadProjectile(Type);
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Vector2 origin = texture.Size() * .5f;
        lightColor = new Color(179, 0, 0, 150);
        if (player.ItemAnimationActive)
        {
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * .25f;
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.oldRot[k], origin, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
            }
        }
        Vector2 drawPosMain = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
        Main.EntitySpriteDraw(texture, drawPosMain, null, lightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
        return false;
    }
}
public class DrainingVeilBlade_Hidden_Projectile : ModProjectile
{
    public override string Texture => ModTexture.MissingTexture_Default;
    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 32;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 70;
        Projectile.hide = true;
    }
    public override bool? CanDamage()
    {
        return false;
    }
    public override void AI()
    {
        for (int i = 0; i < 5; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.WhiteTorch);
            dust.noGravity = true;
            dust.velocity = Main.rand.NextVector2CircularEdge(3, 3);
            dust.color = new Color(100, 255, 100, 0);
            dust.scale = Main.rand.NextFloat(1, 1.5f);
        }
        if (Projectile.timeLeft == 70)
        {
            Projectile.ai[1] = Projectile.velocity.X;
            Projectile.ai[2] = Projectile.velocity.Y;
        }
        if (++Projectile.ai[0] >= 60)
        {
            Projectile.ai[0] = 0;
            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FlameProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
            if (proj.ModProjectile is FlameProjectile flame3)
            {
                flame3.FlameColor = new Color(100, 255, 100, 0);
                flame3.DebuffType = BuffID.CursedInferno;
            }
        }
        if (Projectile.timeLeft == 1)
        {
            Projectile.velocity = new(Projectile.ai[1], Projectile.ai[2]);
            var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, -Projectile.velocity.RotatedBy(MathHelper.PiOver2), ModContent.ProjectileType<FlameProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
            if (proj.ModProjectile is FlameProjectile flame2)
            {
                flame2.FlameColor = new Color(100, 255, 100, 0);
                flame2.DebuffType = BuffID.CursedInferno;
            }
            proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(MathHelper.PiOver2), ModContent.ProjectileType<FlameProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
            if (proj.ModProjectile is FlameProjectile flame)
            {
                flame.FlameColor = new Color(100, 255, 100, 0);
                flame.DebuffType = BuffID.CursedInferno;
            }
        }
        Projectile.velocity = Vector2.Zero;
    }
}
public class DrainingVeilBlade_Wave_Projectile : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 100;
        ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
    }
    public override void SetDefaults()
    {
        Projectile.width = 68;
        Projectile.height = 112;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 180;
        Projectile.light = 0.5f;
        Projectile.extraUpdates = 10;
        Projectile.alpha = 255;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 25;
    }
    public override void AI()
    {
        if (Projectile.timeLeft <= 75)
        {
            Projectile.velocity *= .96f;
            Projectile.alpha = Math.Clamp(Projectile.alpha - 3, 0, 255);
        }
        Projectile.rotation = Projectile.velocity.ToRotation();
        if (++Projectile.ai[0] >= 5)
        {
            Projectile.ai[0] = 0;
            Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<DrainingVeilBlade_Hidden_Projectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, 0);
        }
    }
    public override void OnKill(int timeLeft)
    {
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        modifiers.SetMaxDamage(1);
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Player player = Main.player[Projectile.owner];
        player.Heal(1);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Main.instance.LoadProjectile(Type);
        var texture = TextureAssets.Projectile[Type].Value;
        var origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
        for (int k = 1; k < Projectile.oldPos.Length + 1; k++)
        {
            var drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + new Vector2(Projectile.gfxOffY) + origin;
            var color = new Color(255, 0, 0, 0).ScaleRGB(1 - k * .01f);
            Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
        }
        return false;
    }
}

