using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
public class Roguelike_WoodSword : GlobalItem
{
    public override bool InstancePerEntity => true;
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return CheckItem(entity.type);
    }
    private bool CheckItem(int type)
    {
        switch (type)
        {
            case ItemID.BorealWoodSword:
            case ItemID.PalmWoodSword:
            case ItemID.ShadewoodSword:
            case ItemID.EbonwoodSword:
            case ItemID.RichMahoganySword:
                return true;
        }
        return false;
    }
    int swingCount = 0;
    public override void SetDefaults(Item entity)
    {
        entity.scale += .45f;
        entity.damage += 10;
        entity.GetGlobalItem<MeleeWeaponOverhaul>().ShaderOffSetLength += 1;
    }
    public override void HoldItem(Item item, Player player)
    {
        if (player.itemAnimation == player.itemAnimationMax && player.ItemAnimationActive)
        {
            int damage = player.GetWeaponDamage(item);
            float knockback = player.GetWeaponKnockback(item);
            if (++swingCount >= 5)
            {
                swingCount = 0;
                int direction = ModUtils.DirectionFromEntityAToEntityB(player.Center.X, Main.MouseWorld.X);
                for (int i = 0; i < 40; i++)
                {
                    Vector2 pos = new Vector2(player.Center.X + (50 * i + 100) * direction, player.Center.Y - 1000 - 100 * i);
                    Vector2 vel = Vector2.UnitY.Vector2RotateByRandom(5) * 20;
                    int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), damage, knockback, player.whoAmI, 1);
                    if (Main.projectile[projec].ModProjectile is SwordProjectile2 spear)
                    {
                        spear.ItemIDtextureValue = item.type;
                    }
                }
            }
            Vector2 toward = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 100;
            int proj = Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center + toward, Vector2.Zero, ModContent.ProjectileType<SwordProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI);
            if (Main.projectile[proj].ModProjectile is SwordProjectile woodproj)
                woodproj.ItemIDtextureValue = item.type;
            Main.projectile[proj].ai[2] = 120;
        }
    }
}
