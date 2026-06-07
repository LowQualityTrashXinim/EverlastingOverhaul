using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.BuffAndDebuff;
using EverlastingOverhaul.Contents.Items.Consumable.Throwable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global;
internal class RoguelikeGlobalNPC : GlobalNPC
{
    public int Grapefruit = 0;
    public override bool InstancePerEntity => true;
    public int HeatRay_Decay = 0;
    public int HeatRay_HitCount = 0;

    public int GolemFist_HitCount = 0;

    public StatModifier StatDefense = new StatModifier();
    public float Endurance = 0;
    public bool DRFromFatalAttack = false;
    public bool OneTimeDR = false;
    public int DRTimer = 0;
    public const int BossHP = 8000;
    public const int BossDMG = 40;
    public const int BossDef = 5;
    /// <summary>
    /// Use this for always update velocity
    /// </summary>
    public float VelocityMultiplier = 1;
    /// <summary>
    /// Use this for permanent effect
    /// </summary>
    public float static_velocityMultiplier = 1;
    /// <summary>
    /// Set this to true if your NPC is a ghost NPC which can't be kill<br/>
    /// Uses this along with <see cref="BelongToWho"/> to make it so that this NPC will die when the parent NPC is killed
    /// </summary>
    public bool IsAGhostEnemy = false;
    public int BelongToWho = -1;
    public bool CanDenyYouFromLoot = false;
    public int PositiveLifeRegen = 0;
    public int PositiveLifeRegenCount = 0;
    public int Perpetuation_PointStack = 0;
    /// <summary>
    /// Set this to true if you don't want the mod to apply boss NPC fixed boss's stats
    /// </summary>
    public bool NPC_SpecialException = false;
    public override void SetDefaults(NPC entity)
    {
        StatDefense = new();
    }
    public override void ResetEffects(NPC npc)
    {
        npc.buffImmune[ModContent.BuffType<Anti_Immunity>()] = false;
        StatDefense = new();
        if (IsAGhostEnemy)
        {
            npc.dontTakeDamage = true;
        }
        if (--DRTimer <= 0)
        {
            DRFromFatalAttack = false;
        }
        else
        {
            DRFromFatalAttack = true;
        }
        Endurance = 0;
    }
    public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
    {
        if (IsAGhostEnemy)
        {
            return false;
        }
        return base.CanBeHitByItem(npc, player, item);
    }
    public override bool CanBeHitByNPC(NPC npc, NPC attacker)
    {
        if (IsAGhostEnemy)
        {
            return false;
        }
        return base.CanBeHitByNPC(npc, attacker);
    }
    public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
    {
        if (IsAGhostEnemy)
        {
            return false;
        }
        return base.CanBeHitByProjectile(npc, projectile);
    }
    public override Color? GetAlpha(NPC npc, Color drawColor)
    {
        if (npc.HasBuff<Urine_Debuff>())
        {
            drawColor.R = 255;
            drawColor.G = 255;
            drawColor.B = 90;
            return drawColor;
        }
        if (IsAGhostEnemy)
        {
            drawColor.A = 0;
            drawColor.ScaleRGB(.25f);
            drawColor.B = 255;
            return drawColor;
        }
        return base.GetAlpha(npc, drawColor);
    }
    public override bool PreAI(NPC npc)
    {
        if (VelocityMultiplier != 0)
        {
            npc.velocity /= VelocityMultiplier + static_velocityMultiplier - 1;
        }
        else
        {
            npc.velocity /= .001f;
        }
        return base.PreAI(npc);
    }
    public override void PostAI(NPC npc)
    {
        if (VelocityMultiplier != 0)
        {
            npc.velocity *= VelocityMultiplier + static_velocityMultiplier - 1;
        }
        else
        {
            npc.velocity *= .001f;
        }
        VelocityMultiplier = 1;
        if (HeatRay_HitCount > 0)
        {
            HeatRay_Decay = ModUtils.CountDown(HeatRay_Decay);
            if (HeatRay_Decay <= 0)
            {
                HeatRay_HitCount--;
            }
        }
        if (BelongToWho >= 0 && BelongToWho < Main.maxNPCs)
        {
            var parent = Main.npc[BelongToWho];
            if (parent != null)
            {
                if (!parent.active || parent.life <= 0)
                {
                    npc.StrikeInstantKill();
                }
            }
            else
            {
                BelongToWho = -1;
            }
        }
        if (++PositiveLifeRegenCount >= 60)
        {
            PositiveLifeRegenCount = 0;
            npc.life = Math.Clamp(npc.life + PositiveLifeRegen, 0, npc.lifeMax);
        }
    }

    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        NPC_Debuff(npc, ref modifiers);
    }
    public int CursedSkullStatus = 0;
    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        NPC_Debuff(npc, ref modifiers);
        if (projectile.type == ProjectileID.HeatRay)
        {
            modifiers.SourceDamage += HeatRay_HitCount * .02f;
        }
        if (projectile.type == ProjectileID.GolemFist)
        {
            if (++GolemFist_HitCount % 3 == 0)
            {
                modifiers.SourceDamage += 1.5f;
            }
        }
    }
    private void NPC_Debuff(NPC npc, ref NPC.HitModifiers modifiers)
    {
        modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
        modifiers.SourceDamage *= Math.Clamp(1 - Endurance, 0, 1f);
    }
    public int HitCount = 0;
    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        HitCount++;
    }
    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        HitCount++;
        if (projectile.type == ProjectileID.HeatRay)
        {
            HeatRay_HitCount = Math.Clamp(HeatRay_HitCount + 1, 0, 200);
            HeatRay_Decay = 30;
        }
        else if (projectile.type == ProjectileID.GolemFist)
        {
            if (GolemFist_HitCount % 3 == 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.HeatRay);
                    dust.noGravity = true;
                    dust.velocity = Main.rand.NextVector2Circular(20, 20);
                    dust.scale += Main.rand.NextFloat();
                }
                for (int i = 0; i < 100; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.HeatRay);
                    dust.noGravity = true;
                    dust.velocity = Main.rand.NextVector2CircularEdge(25, 25);
                    dust.scale += Main.rand.NextFloat();
                }
                SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                npc.Center.LookForHostileNPC(out List<NPC> npclist, 150);
                npc.TargetClosest();
                Player player = Main.player[npc.target];
                foreach (var target in npclist)
                {
                    if (target.whoAmI != npc.whoAmI)
                    {
                        player.StrikeNPCDirect(target, target.CalculateHitInfo(hit.Damage, -1));
                    }
                }
            }
        }
    }
    public override void OnKill(NPC npc)
    {
        int playerIndex = npc.lastInteraction;
        if (!Main.player[playerIndex].active || Main.player[playerIndex].dead)
        {
            playerIndex = npc.FindClosestPlayer();
        }
        var player = Main.player[playerIndex];
        player.GetModPlayer<PlayerStatsHandle>().successfullyKillNPCcount++;
        player.GetModPlayer<PlayerStatsHandle>().NPC_HitCount = HitCount;
    }
    public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        //TODO : this is very broken, I couldn't get the outline to work so I gave up
        //if (npc.boss) {
        //	Main.instance.LoadNPC(npc.type);
        //	Texture2D texture = TextureAssets.Npc[npc.type].Value;
        //	SpriteEffects effect = SpriteEffects.None;
        //	Vector2 origin = npc.frame.Size() * .5f;
        //	Vector2 drawpos = npc.position - Main.screenPosition;
        //	spriteBatch.Draw(texture, drawpos + Vector2.One * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
        //	spriteBatch.Draw(texture, drawpos - Vector2.One * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
        //	spriteBatch.Draw(texture, drawpos + Vector2.One.Add(-2, 0) * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
        //	spriteBatch.Draw(texture, drawpos + Vector2.One.Add(0, -2) * 3, npc.frame, Color.Red * .25f, npc.rotation, origin, npc.scale, effect, 0);
        //}
        return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
    }
}
