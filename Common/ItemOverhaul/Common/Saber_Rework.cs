using EverlastingOverhaul.Common;
using EverlastingOverhaul.Common.Graphics;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
internal class EverlastingOverhaul_Saber : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return CheckSaber(entity.type);
    }
    public bool CheckSaber(int type)
    {
        switch (type)
        {
            case ItemID.PurplePhaseblade:
            case ItemID.BluePhaseblade:
            case ItemID.GreenPhaseblade:
            case ItemID.YellowPhaseblade:
            case ItemID.OrangePhaseblade:
            case ItemID.RedPhaseblade:
            case ItemID.WhitePhaseblade:
            case ItemID.PurplePhasesaber:
            case ItemID.BluePhasesaber:
            case ItemID.GreenPhasesaber:
            case ItemID.YellowPhasesaber:
            case ItemID.OrangePhasesaber:
            case ItemID.RedPhasesaber:
            case ItemID.WhitePhasesaber:
                return true;
        }
        return false;
    }
    public override void SetDefaults(Item entity)
    {
        entity.shoot = ModContent.ProjectileType<StarWarSwordProjectile>();
        entity.shootSpeed = 1;
        entity.useAnimation = entity.useTime = 15;
        entity.ArmorPenetration = 30;
    }
    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        var modplayer = player.GetModPlayer<GlobalItemPlayer>();
        if (++modplayer.PhaseSaberBlade_Counter >= 3)
        {
            modplayer.PhaseSaberBlade_Counter = 0;
            var projectile = Projectile.NewProjectileDirect(source, position, velocity.SafeNormalize(Vector2.Zero) * 1, type, damage, knockback, player.whoAmI);
            if (projectile.ModProjectile is StarWarSwordProjectile starwarProjectile)
            {
                starwarProjectile.ColorOfSaber = SwordSlashTrail.averageColorByID[item.type] * 2;
                starwarProjectile.ItemTextureID = item.type;
            }
            projectile.width = item.width;
            projectile.height = item.height;
        }
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
