using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global;
internal class EverlastingOverhaulGlobalNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public bool DRFromFatalAttack = false;
    public bool OneTimeDR = false;
    public int DRTimer = 0;
    public int ExtraUpdate = 0;
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
    /// <summary>
    /// Currently this is broken, highly not recommand to use it.
    /// </summary>
    public bool CanDenyYouFromLoot = false;
    /// <summary>
    /// This doesn't reset anywhere, it is a static value that allow npc to regenerate hp
    /// </summary>
    public int PositiveLifeRegen = 0;
    public int PositiveLifeRegenCount = 0;
    /// <summary>
    /// Set this to true if you don't want the mod to apply boss NPC fixed boss's stats
    /// </summary>
    public bool NPC_SpecialException = false;
    public int InvincibilityFrame = 0;

    public float Static_PercentageDamage = 0;

    public float PercentageDamage = 0;

    public float MaxDamageTaken = 1;
    /// <summary>
    /// This is always reset back to 0, if you are looking for static damage reduciton, use <see cref="Static_Endurance"/> instead
    /// </summary>
    public float Endurance = 0;
    public float Static_Endurance = 0;
    /// <summary>
    /// This is a always reset value, will increases damage NPC via ModifyHit hook and can be applied to projectile
    /// </summary>
    public StatModifier DamageIncrease = new();

    public StatModifier Static_DamageIncrease = new();

    public StatModifier StatDefense = new StatModifier();
    /// <summary>
    /// Use this for uniform debuff scaling<br/>
    /// Modify it as you would to any other statmodifier as the code within <see cref="EverlastingOverhaulGlobalNPC"/> will handle all
    /// </summary>
    public StatModifier Poison_Inner = StatModifier.Default;
    Dictionary<string, StatModifier> Poison_Global = new();
    /// <summary>
    /// Use this over <see cref="Poison_Inner"/> if the debuff change you made is global for your player
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add_PoisonGlobal(string key, StatModifier value)
    {
        if (Poison_Global.ContainsKey(key))
        {
            Poison_Global[key] = value;
        }
        else
        {
            Poison_Global.Add(key, value);
        }
    }
    public StatModifier Poison_Additional = StatModifier.Default - 1;
    public float Poison_Additional_Timer = 0;
    public float Poison_Additional_MaxTimer = 60;
    public int Amount_CurrentDebuffInflicted = 0;
    public override void SetDefaults(NPC entity)
    {
        StatDefense = new();
        DamageIncrease = new();
    }
    public int TimeLimit = -1;
    public override void ResetEffects(NPC npc)
    {
        //var player = Main.player[npc.target];
        //if (npc.Center.IsCloseToPosition(player.Center, 1500)) {
        //	npc.timeLeft = 600;
        //}
        StatDefense = new();
        DamageIncrease = new();
        MaxDamageTaken = 1;
        if (IsAGhostEnemy)
        {
            npc.dontTakeDamage = true;
            npc.chaseable = false;
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
        //if (npc.boss) {
        //	if (npc.life <= npc.lifeMax / 2) {
        //		DamageIncrease += 1;
        //		MaxDamageTaken -= .99f;
        //	}
        //}
    }
    public override void UpdateLifeRegen(NPC npc, ref int damage)
    {
        foreach (var item in Poison_Global.Values)
        {
            Poison_Inner = Poison_Inner.CombineWith(item);
        }
        npc.lifeRegen = (int)((npc.lifeRegen - Poison_Inner.Base) * Poison_Inner.Additive * Poison_Inner.Multiplicative - Poison_Inner.Flat);
        Poison_Inner = StatModifier.Default;
        Amount_CurrentDebuffInflicted = 0;
        for (int i = 0; i < npc.buffType.Length; i++)
        {
            if (npc.buffType[i] <= 0)
            {
                continue;
            }
            if (Main.debuff[npc.buffType[i]])
            {
                Amount_CurrentDebuffInflicted++;
            }
        }
        if (++Poison_Additional_Timer >= Poison_Additional_MaxTimer)
        {
            int additional = (int)((npc.lifeRegen - Poison_Additional.Base) * Poison_Additional.Additive * Poison_Additional.Multiplicative - Poison_Additional.Flat) * -1;
            npc.lifeRegen -= additional;
            Poison_Additional = StatModifier.Default - 1;
            Poison_Additional_Timer = 0;
        }
        damage = (int)Math.Ceiling(npc.lifeRegen * -.5f);
    }
    public int Grapefruit = 0;
    public override bool? CanBeHitByItem(NPC npc, Player player, Item item)
    {
        if (IsAGhostEnemy || InvincibilityFrame > 0)
        {
            return false;
        }
        return base.CanBeHitByItem(npc, player, item);
    }
    public override bool CanBeHitByNPC(NPC npc, NPC attacker)
    {
        if (IsAGhostEnemy || InvincibilityFrame > 0)
        {
            return false;
        }
        return base.CanBeHitByNPC(npc, attacker);
    }
    public override bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
    {
        if (IsAGhostEnemy || InvincibilityFrame > 0)
        {
            return false;
        }
        return base.CanBeHitByProjectile(npc, projectile);
    }
    public override Color? GetAlpha(NPC npc, Color drawColor)
    {
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
        if (BelongToWho >= 0 && BelongToWho < Main.maxNPCs)
        {
            var parent = Main.npc[BelongToWho];
            if (parent != null)
            {
                if (!parent.active || parent.life <= 0)
                {
                    npc.life = 0;
                    npc.realLife = 0;
                    npc.active = false;
                    return;
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
        InvincibilityFrame = ModUtils.CountDown(InvincibilityFrame);
    }
    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        modifiers.FinalDamage.Flat += (int)(target.statLifeMax2 * (PercentageDamage + Static_PercentageDamage));
        modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(DamageIncrease.CombineWith(Static_DamageIncrease));
    }
    public int ResistHitCount = 0;
    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        NPC_Debuff(npc, ref modifiers);
    }
    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        NPC_Debuff(npc, ref modifiers);
    }
    private void NPC_Debuff(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (MaxDamageTaken < 1)
        {
            modifiers.SetMaxDamage((int)(npc.lifeMax * MaxDamageTaken));
        }
        modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
        modifiers.SourceDamage *= Math.Clamp(1 - (Endurance + Static_Endurance), 0, 1f);
        if (--ResistHitCount > 0)
        {
            modifiers.SetMaxDamage(1);
        }
    }
    public int HitCount = 0;
    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        HitCount++;
    }
    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        HitCount++;
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
        if (InvincibilityFrame > 0 && InvincibilityFrame % 5 == 0)
        {
            return false;
        }
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
