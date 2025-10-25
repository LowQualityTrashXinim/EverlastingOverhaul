using EverlastingOverhaul.Common.Projectiles;
using EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Common;
public class Roguelike_Axe : GlobalItem
{
    public override void SetDefaults(Item entity)
    {
        AxeWeaponOverhaul(entity);
    }
    private bool CheckAxeCommon(int type)
    {
        switch (type)
        {
            //common ore axe
            case ItemID.CopperAxe:
            case ItemID.TinAxe:
            case ItemID.IronAxe:
            case ItemID.LeadAxe:
            case ItemID.SilverAxe:
            case ItemID.TungstenAxe:
            case ItemID.GoldAxe:
            case ItemID.PlatinumAxe:
            //uncommon ore axe
            case ItemID.BloodLustCluster:
            case ItemID.WarAxeoftheNight:
            case ItemID.MoltenPickaxe:
            case ItemID.MeteorHamaxe:
            //Hardmode ore axe
            case ItemID.CobaltWaraxe:
            case ItemID.PalladiumWaraxe:
            case ItemID.MythrilWaraxe:
            case ItemID.OrichalcumWaraxe:
            case ItemID.AdamantiteWaraxe:
            case ItemID.TitaniumWaraxe:
                return true;
        }
        return false;
    }
    public void AxeWeaponOverhaul(Item item)
    {
        if (item.axe <= 0 || item.noMelee)
        {
            return;
        }
        //Attempt to fix weapon size
        if (CheckAxeCommon(item.type))
        {
            item.damage += 13;
            item.scale += .45f;
            item.useTurn = false;
            item.Set_ItemCriticalDamage(1.5f);
            var global = item.GetGlobalItem<MeleeWeaponOverhaul>();
            global.SwingType = BossRushUseStyle.SwipeDown;
            global.SwingDegree = 155;
            global.Ignore_AttackSpeed = true;
            global.AnimationEndTime = 25;
        }
    }
    public override bool AltFunctionUse(Item item, Player player)
    {
        if (CheckAxeCommon(item.type))
        {
            if (player.altFunctionUse == 2)
            {
                item.noUseGraphic = true;
            }
            else
            {
                item.noUseGraphic = false;
            }
            return true;
        }
        return base.AltFunctionUse(item, player);
    }
    public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
    {
        if (CheckAxeCommon(item.type))
        {
            if (player.altFunctionUse == 2)
            {
                item.noUseGraphic = true;
            }
            else
            {
                item.noUseGraphic = false;
            }
        }
    }
    public override void UseAnimation(Item item, Player player)
    {
        if (CheckAxeCommon(item.type))
        {
            if (Main.mouseRight)
            {
                item.noUseGraphic = true;
            }
            else
            {
                item.noUseGraphic = false;
            }
        }
    }
    public override bool CanUseItem(Item item, Player player)
    {
        if (CheckAxeCommon(item.type))
        {
            if (player.altFunctionUse == 2)
            {
                item.noUseGraphic = true;
            }
            else
            {
                item.noUseGraphic = false;
            }
            return player.ownedProjectileCounts[ModContent.ProjectileType<TomahawkProjectile>()] < 1;
        }
        return base.CanUseItem(item, player);
    }
    public override void HoldItem(Item item, Player player)
    {
        if (CheckAxeCommon(item.type))
        {
            if (player.altFunctionUse == 2 && player.ItemAnimationActive && player.itemAnimation == player.itemAnimationMax)
            {
                Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero);
                Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity * 17, ModContent.ProjectileType<TomahawkProjectile>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI, ai2: item.type);
            }
        }
    }
}
