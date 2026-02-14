using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class Roguelike_AnkletOfTheWind : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.AnkletoftheWind;
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
    }
    public override void UpdateEquip(Item item, Player player)
    {
        player.GetModPlayer<Roguelike_AnkletOfTheWind_ModPlayer>().AnkletOfTheWind = true;
        player.ModPlayerStats().UpdateJumpBoost += 1.5f;
    }
}
public class Roguelike_AnkletOfTheWind_ModPlayer : ModPlayer
{
    public const int DashRight = 2;
    public const int DashLeft = 3;

    public const int DashCooldown = 50;
    public const int DashDuration = 35;

    public const float DashVelocity = 7.5f;

    public int DashDir = -1;

    public int DashDelay = 0;
    public int DashTimer = 0;
    public override void PreUpdateMovement()
    {
        if (CanUseDash() && DashDir != -1 && DashDelay == 0)
        {
            var newVelocity = Player.velocity;
            DashDelay = DashCooldown;
            DashTimer = DashDuration;
            Vector2 vel = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero) * 15;
            newVelocity.X = vel.X;
            newVelocity.Y = vel.Y;
            Player.velocity = newVelocity;
            Player.AddBuff<WindDashCoolDown>(ModUtils.ToSecond(5));
            Modify_WindHealth(25);
        }

        if (DashDelay > 0)
            DashDelay--;

        if (DashTimer > 0)
        {
            Player.eocDash = DashTimer;
            DashTimer--;
        }
    }
    private bool CanUseDash()
    {
        return Player.CheckDashType("Wind")
            && !Player.setSolar
            && !Player.mount.Active
            && !Player.HasBuff<WindDashCoolDown>();
    }
    public bool AnkletOfTheWind = false;
    public const int WindShield_Health_Default = 100;
    public int WindShield_Health = 0;
    public int WindShield_Cooldown = 0;
    public int WindShield_Health_Count = 0;
    public override void ResetEffects()
    {
        if (!AnkletOfTheWind)
        {
            WindShield_Health = 0;
            WindShield_Cooldown = 0;
        }
        else
        {
            if (WindShield_Health <= 0 && WindShield_Cooldown <= 0)
            {
                WindShield_Health = WindShield_Health_Default;
            }
            WindShield_Cooldown = ModUtils.CountDown(WindShield_Cooldown);
            // ResetEffects is called not long after player.doubleTapCardinalTimer's values have been set
            // When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
            // If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
            {
                DashDir = DashRight;
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
            {
                DashDir = DashLeft;
            }
            else
            {
                DashDir = -1;
            }
            Player.ModPlayerStats().CurrentDashType = "Wind";
            Player.AddBuff<WindShield>(2);
        }
        AnkletOfTheWind = false;
    }
    public override void UpdateEquips()
    {
        if (WindShield_Health > 0 && WindShield_Cooldown <= 0)
        {
            if (WindShield_Health >= WindShield_Health_Default)
            {
                WindShield_Health = WindShield_Health_Default;
            }
            else if (++WindShield_Health_Count >= 3)
            {
                WindShield_Health++;
                WindShield_Health_Count = 0;
            }
        }
    }
    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        if (!AnkletOfTheWind)
        {
            return;
        }
        if (WindShield_Health > 0)
        {
            modifiers.SourceDamage -= .2f;
            modifiers.Knockback *= .7f;
        }
    }
    public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
    {
        if (!AnkletOfTheWind)
        {
            return;
        }
        if (WindShield_Health > 0)
        {
            modifiers.SourceDamage -= .8f;
            modifiers.Knockback *= 0;
        }
    }
    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        if (!AnkletOfTheWind)
        {
            return;
        }
        Modify_WindHealth(hurtInfo.Damage);
    }
    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
    {
        if (!AnkletOfTheWind)
        {
            return;
        }
        Modify_WindHealth(hurtInfo.Damage);
    }
    private void Modify_WindHealth(int health)
    {
        WindShield_Health -= health;
        if (WindShield_Health <= 0)
        {
            WindShield_Cooldown = 300;
        }
    }
}
public class WindDashCoolDown : ModBuff
{
    public override string Texture => ModTexture.EMPTYDEBUFF;
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = false;
    }
}

public class WindShield : ModBuff
{
    public override string Texture => ModTexture.EMPTYBUFF;
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = false;
        Main.buffNoSave[Type] = true;
        Main.buffNoTimeDisplay[Type] = true;
    }
    public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
    {
        string hexColor;
        Player player = Main.LocalPlayer;
        Roguelike_AnkletOfTheWind_ModPlayer shieldplayer = player.GetModPlayer<Roguelike_AnkletOfTheWind_ModPlayer>();
        int healthPoint = shieldplayer.WindShield_Health;
        if (healthPoint >= Roguelike_AnkletOfTheWind_ModPlayer.WindShield_Health_Default * .55f)
        {
            hexColor = Color.LawnGreen.Hex3();
        }
        else if (healthPoint >= Roguelike_AnkletOfTheWind_ModPlayer.WindShield_Health_Default * .2f)
        {
            hexColor = Color.Yellow.Hex3();
        }
        else
        {
            hexColor = Color.Red.Hex3();
        }
        tip = string.Format(tip, [hexColor, healthPoint.ToString()]);
    }
}
