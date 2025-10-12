using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Mode.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
internal class Roguelike_PlatinumBow : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.PlatinumBow;
    }
    public override void SetDefaults(Item entity)
    {
        entity.useTime = entity.useAnimation = 22;
        entity.damage += 20;
        entity.shootSpeed += 3;
        entity.crit += 6;
    }
    public override bool AltFunctionUse(Item item, Player player)
    {
        return true;
    }
    public override float UseSpeedMultiplier(Item item, Player player)
    {
        float speed = base.UseTimeMultiplier(item, player);
        if (player.altFunctionUse == 2)
        {
            return speed - .5f;
        }
        Roguelike_PlatinumBow_ModPlayer modplayer = player.GetModPlayer<Roguelike_PlatinumBow_ModPlayer>();
        switch (modplayer.Combo)
        {
            case 0:
                return speed + .75f;
            case 1:
                return speed - .5f;
            case 2:
                return speed - .1f;
            case 3:
                return speed + .1f;
            case 4:
                return speed - .45f;
        }
        return base.UseTimeMultiplier(item, player);
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        ModUtils.AddTooltip(ref tooltips, new(Mod, $"Roguelike_{item.Name}", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
    }
    public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (player.altFunctionUse == 2)
        {
            return;
        }
        Roguelike_PlatinumBow_ModPlayer modplayer = player.GetModPlayer<Roguelike_PlatinumBow_ModPlayer>();
        player.velocity = Vector2.Zero;
        switch (modplayer.Combo)
        {
            case 0:
                player.velocity -= velocity.SafeNormalize(Vector2.Zero);
                damage = (int)(damage * .35f) + 1;
                break;
            case 1:
                player.velocity -= velocity.SafeNormalize(Vector2.Zero);
                damage = (int)(damage * .25f) + 1;
                break;
            case 2:
                player.velocity -= velocity.SafeNormalize(Vector2.Zero);
                damage = (int)(damage * .65f) + 1;
                break;
            case 3:
                player.velocity -= velocity.SafeNormalize(Vector2.Zero) * 2;
                damage = (int)(damage * .5f) + 1;
                break;
            case 4:
                player.velocity -= velocity.SafeNormalize(Vector2.Zero) * 3;
                damage = (int)(damage);
                break;
        }
        player.velocity.Y = 0;
    }
    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Roguelike_PlatinumBow_ModPlayer modplayer = player.GetModPlayer<Roguelike_PlatinumBow_ModPlayer>();
        if (player.altFunctionUse == 2)
        {
            modplayer.Combo = 0;
        }
        else
        {
            modplayer.Cooldown = -player.itemAnimationMax;
            modplayer.Combo = ModUtils.Safe_SwitchValue(modplayer.Combo, 4);
        }
        var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        if (projectile.arrow)
        {
            projectile.extraUpdates += 1;
        }
        return false;
    }
}
public class Roguelike_PlatinumBow_ModPlayer : ModPlayer
{
    public int Cooldown = 0;
    public int Combo = 0;
    public override void ResetEffects()
    {
        if (++Cooldown >= 10)
        {
            Combo = 0;
        }
        if (Player.HeldItem.type == ItemID.PlatinumBow)
        {
            if (Player.ItemAnimationActive && Player.altFunctionUse != 2)
            {
                Player.controlLeft = false;
                Player.controlRight = false;
                Player.controlUp = false;
                Player.controlJump = false;
                Player.controlDown = false;
            }
        }
    }
}
