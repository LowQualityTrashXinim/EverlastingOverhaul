using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Graphics;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Projectiles;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Contents.Items.Weapon;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_Katana : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.Katana;
    public override void SetDefaults(Item entity)
    {
        entity.damage = 20;
        entity.shoot = ModContent.ProjectileType<SimplePiercingProjectile2>();
        entity.shootSpeed = 1;
        progress.Charge = true;
        if (entity.TryGetGlobalItem(out GlobalItemHandle handler))
        {
            handler.CriticalDamage += 3;
        }
    }
    public static readonly WeaponProgress progress = new()
    {

    };
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        ModUtils.AddTooltip(ref tooltips, new(Mod, $"EverlastingOverhaulOverhaul_{item.Name}", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
    }
    public override void HoldItem(Item item, Player player)
    {
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.SetWeaponProgress(progress);
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.barProgress = player.GetModPlayer<EverlastingOverhaul_Katana_ModPlayer>().Counter / 60f;
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientA = Color.Gray;
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientB = Color.White;
    }
    public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
    {
        float count = player.GetModPlayer<EverlastingOverhaul_Katana_ModPlayer>().Counter;
        if (count >= 60)
        {
            crit += 50;
        }
    }
    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        float count = player.GetModPlayer<EverlastingOverhaul_Katana_ModPlayer>().Counter;
        if (count >= 60)
        {
            Vector2 velocityToward = velocity.RotatedBy(MathHelper.PiOver2 * Main.rand.NextBool().ToDirectionInt()).Vector2RotateByRandom(55);
            Projectile Swordprojectile = Projectile.NewProjectileDirect(
                source,
                Main.MouseWorld,
                velocityToward,
                ModContent.ProjectileType<SimplePiercingProjectile2>(),
                damage * 5,
                1,
                player.whoAmI,
                5f, 15, 10);
            if (Swordprojectile.ModProjectile is SimplePiercingProjectile2 modproj)
            {
                modproj.ProjectileColor = SwordSlashTrail.averageColorByID[ItemID.Katana] * 2;
                modproj.ScaleX = 9 + Main.rand.NextFloat();
            }
            Swordprojectile.usesIDStaticNPCImmunity = false;
            Swordprojectile.usesLocalNPCImmunity = true;
            Swordprojectile.localNPCHitCooldown = 60;
        }
        return false;
    }
}
public class EverlastingOverhaul_Katana_ModPlayer : ModPlayer
{
    public int Counter = 0;
    public override void ResetEffects()
    {
        if (!Player.active)
        {
            return;
        }
        if (++Counter >= 60)
        {
            Counter = 60;
        }
        if (Player.HeldItem.type != ItemID.Katana)
        {
            return;
        }
        if (Player.ItemAnimationActive && Player.ItemAnimationEndingOrEnded)
        {
            Counter = -Player.itemAnimationMax;
        }
    }
    public void SpawnSpecialDustEffect()
    {
        SoundEngine.PlaySound(SoundID.Item71 with { Pitch = .5f }, Player.Center);
        ModUtils.DustStar(Player.Center + Vector2.UnitY * -50, DustID.GemDiamond, Color.White);
    }
}
