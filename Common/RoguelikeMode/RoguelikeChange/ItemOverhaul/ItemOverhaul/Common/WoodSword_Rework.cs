using EverlastingOverhaul.Common.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Common;
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
            case ItemID.PearlwoodSword:
            case ItemID.BorealWoodSword:
            case ItemID.PalmWoodSword:
            case ItemID.ShadewoodSword:
            case ItemID.EbonwoodSword:
            case ItemID.RichMahoganySword:
            case ItemID.WoodenSword:
            case ItemID.AshWoodSword:
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
    public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
    {
        if (player.itemAnimation == player.itemAnimationMax)
        {
            int damage = player.GetWeaponDamage(item);
            float knockback = player.GetWeaponKnockback(item);
            if (++swingCount >= 5)
            {
                swingCount = 0;
                Vector2 pos = new Vector2(Main.MouseWorld.X, player.Center.Y - 1000) + Main.rand.NextVector2Circular(100, 100);
                Vector2 vel = (Main.MouseWorld - pos).SafeNormalize(Vector2.Zero) * 20;
                int projec = Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), pos, vel, ModContent.ProjectileType<SwordProjectile2>(), damage, knockback, player.whoAmI, 1);
                if (Main.projectile[projec].ModProjectile is SwordProjectile2 spear)
                {
                    spear.ItemIDtextureValue = item.type;
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
