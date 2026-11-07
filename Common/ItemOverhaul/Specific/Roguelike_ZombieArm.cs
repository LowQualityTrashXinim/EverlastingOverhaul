using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Systems.ReviveSystem;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.BuffAndDebuff;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class ZombieArm_Rework : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.ZombieArm;
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
    }
    public override void SetDefaults(Item entity)
    {
        entity.damage = 33;
    }
    public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        player.Heal(Main.rand.Next(1, 4));
        target.AddBuff<RA_Rotting>(ModUtils.ToSecond(3));
    }
}
public class ZombieArm_Rework_Revive : Revive
{
    public override bool IsActive(Player player)
    {
        return player.HeldItem.type == ItemID.ZombieArm;
    }
    public override void OnRevive(Player player)
    {
        player.Heal(player.statLifeMax2);
        player.AddImmuneTime(-1, 120);
        player.immune = true;
        if (player.HeldItem.type == ItemID.ZombieArm)
        {
            player.HeldItem.TurnToAir();
        }
    }
}
public class ZombieArm_Rework_ModPlayer : ModPlayer
{
    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        if (Player.HeldItem.type == ItemID.ZombieArm)
        {
            Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Player.GetWeaponDamage(Player.HeldItem), 1));
        }
    }
}
