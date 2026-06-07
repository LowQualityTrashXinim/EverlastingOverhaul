using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Systems;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
public class Roguelike_Musket : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.Musket;
    }
    public override void SetDefaults(Item entity)
    {
        entity.damage = 120;
        entity.crit = 20;
        entity.useTime = entity.useAnimation = 60;
    }
    public static readonly WeaponProgress progress = new()
    {

    };
    public override void SetStaticDefaults()
    {
        progress.Set_Progress(60 / 180f, 61 / 180f, new Color(255, 100, 0));
        progress.Charge = true;
    }
    public override void HoldItem(Item item, Player player)
    {
        if (OutroEffect_ModPlayer.Check_ValidForIntroEffect(player))
        {
            OutroEffect_ModPlayer.Set_IntroEffect(player, item.type, ModUtils.ToSecond(3));
        }
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.SetWeaponProgress(progress);
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.barProgress = player.GetModPlayer<Roguelike_Musket_ModPlayer>().Timer / 180f;
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientA = Color.Brown;
        ModContent.GetInstance<UniversalSystem>().defaultUI.WeaponBar.gradientB = Color.Gray;
    }
    public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (OutroEffect_ModPlayer.Check_IntroEffect(player, item.type))
        {
            if (type == ProjectileID.Bullet)
            {
                type = ProjectileID.BulletHighVelocity;
            }
            damage += 10;
        }
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
    }
    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        int timer = player.GetModPlayer<Roguelike_Musket_ModPlayer>().Timer;
        var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
        player.GetModPlayer<Roguelike_Musket_ModPlayer>().Timer = -player.itemAnimationMax;
        if (timer >= 60)
        {
            proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().SetCrit++;
            if (timer >= 180)
            {
                proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().CritDamage += 1.5f;
            }
        }
        return false;
    }
}
public class Roguelike_Musket_ModPlayer : ModPlayer {
	public int Timer = 0;
	public override void ResetEffects() {
		if (++Timer >= 180) {
			Timer = 180;
		}
		if (Player.active && !Player.dead && Player.HeldItem.type == ItemID.Musket) {
			if (Timer == 60) {
				SoundEngine.PlaySound(SoundID.Item102 with { Pitch = 1 });
			}
		}
	}
}
