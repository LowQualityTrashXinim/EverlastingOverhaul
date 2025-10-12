using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ItemOverhaul.Specific;
public class Roguelike_CopperBow : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.CopperBow;
	}
	public override void SetDefaults(Item entity) {
		entity.damage += 5;
		entity.useTime = entity.useAnimation = 23;
		entity.shootSpeed = 15;
	}
    public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        int Counter = player.GetModPlayer<Roguelike_CopperBow_ModPlayer>().CopperBow_Counter;
        if (Counter >= 90)
        {
            int amount = 8;
            if (Counter == 150)
            {
                amount += 15;
            }
            for (int i = 0; i < amount; i++)
            {
                var projectile = Projectile.NewProjectileDirect(source, position, velocity.Vector2RotateByRandom(30) * Main.rand.NextFloat(.7f, 1f), ProjectileID.ThunderSpearShot, (int)(damage * 1.25f), knockback, player.whoAmI);
                projectile.DamageType = DamageClass.Ranged;
                projectile.extraUpdates = 2;
                projectile.alpha -= 1020;
            }
        }
        if (item.type == ItemID.CopperBow)
        {
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, ProjectileID.ThunderSpearShot, (int)(damage * 1.25f), knockback, player.whoAmI);
            projectile.DamageType = DamageClass.Ranged;
            projectile.extraUpdates = 2;
            projectile.alpha -= 1020;
        }
        player.GetModPlayer<Roguelike_CopperBow_ModPlayer>().CopperBow_Counter = -player.itemAnimationMax;
        return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "Roguelike_CoperBow", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
}
public class Roguelike_CopperBow_ModPlayer : ModPlayer {
	public int CopperBow_Counter = 0;
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		if (++CopperBow_Counter > 150) {
			CopperBow_Counter = 150;
		}
	}
}
