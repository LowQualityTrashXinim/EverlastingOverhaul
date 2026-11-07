using EverlastingOverhaul.Common.General;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using Roguelike.Common.Global;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_AnkletOfTheWind : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.AnkletoftheWind;
    }
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("EverlastingOverhaulRework", item.Name)));
    }
    public override void UpdateEquip(Item item, Player player)
    {
        player.GetModPlayer<EverlastingOverhaul_AnkletOfTheWind_ModPlayer>().AnkletOfTheWind = true;
        player.ModPlayerStats().UpdateJumpBoost += 1.5f;
    }
}
public class EverlastingOverhaul_AnkletOfTheWind_ModPlayer : ModPlayer
{
    public const int DashRight = 2;
    public const int DashLeft = 3;

    public const int DashCooldown = 50;
    public const int DashDuration = 35;

    public const float DashVelocity = 7.5f;

    public bool DashDir = false;

    public int DashDelay = 0;
    public int DashTimer = 0;
    public Vector2 ReappliedMovement = Vector2.Zero;
    public override void PreUpdateMovement()
    {
        if (CanUseDash() && DashDir && DashDelay == 0)
        {
            DashTimer = 10;
            DashDelay = DashTimer * 3 + DashTimer;
            var vel = (Main.MouseWorld - Player.Center).SafeNormalize(Vector2.Zero) * 35;
            ReappliedMovement = vel;
            Player.velocity = vel;
            Modify_WindHealth(25);
            Player.direction = Player.velocity.X > 0 ? 1 : -1;
        }

        if (DashDelay > 0)
            DashDelay--;

        if (DashTimer > 0)
        {
            Player.ModPlayerStats().Hide = true;
            Player.velocity = ReappliedMovement;
            if (DashTimer == 1)
            {
                Player.velocity = Player.velocity * .25f;
            }
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(Player.Center + Main.rand.NextVector2Circular(30, 30), 0, 0, DustID.Cloud);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2CircularEdge(3, 3) * Main.rand.NextFloat(.75f, 1.2f);
                dust.scale = Main.rand.NextFloat(1, 1.4f) + 1;
                dust.color = Color.White with { A = 0 };
            }
            //Player.eocDash = DashTimer;
            DashTimer--;
        }
        else
        {
            ReappliedMovement = Vector2.Zero;
        }
    }
    private bool CanUseDash()
    {
        return Player.CheckDashType("Wind")
            && !Player.setSolar
            && !Player.mount.Active;
    }
    public bool AnkletOfTheWind = false;
    public const int WindShield_Health_Default = 100;
    public int WindShield_Health = 0;
    public int WindShield_Cooldown = 0;
    public int WindShield_Health_Count = 0;
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (AnkletOfTheWind)
        {
            DashDir = ProcessTriggerSystem.Key_Dash.JustPressed;
        }
    }
    public override void ResetEffects()
    {
        if (!AnkletOfTheWind)
        {
            WindShield_Health = 0;
            WindShield_Cooldown = 0;
            DashDir = false;
        }
        else
        {
            if (WindShield_Health <= 0 && WindShield_Cooldown <= 0)
            {
                WindShield_Health = WindShield_Health_Default;
            }
            WindShield_Cooldown = ModUtils.CountDown(WindShield_Cooldown);
            Player.ModPlayerStats().CurrentDashType = "Wind";
            Player.AddBuff<WindShield>(2);
        }
        if (DashTimer > 0)
        {
            Player.immune = true;
            Player.immuneTime = 2;
            Player.immuneNoBlink = true;
            Player.ModPlayerStats().Hide = true;
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
        var player = Main.LocalPlayer;
        var shieldplayer = player.GetModPlayer<EverlastingOverhaul_AnkletOfTheWind_ModPlayer>();
        int healthPoint = shieldplayer.WindShield_Health;
        if (healthPoint >= EverlastingOverhaul_AnkletOfTheWind_ModPlayer.WindShield_Health_Default * .55f)
        {
            hexColor = Color.LawnGreen.Hex3();
        }
        else if (healthPoint >= EverlastingOverhaul_AnkletOfTheWind_ModPlayer.WindShield_Health_Default * .2f)
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
