using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.BuffAndDebuff;
using EverlastingOverhaul.Contents.Items.Consumable.Throwable;
using EverlastingOverhaul.Contents.Items.NoneSynergy;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.SkullRevolver;
using EverlastingOverhaul.Contents.Projectiles;
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
internal class RoguelikeGlobalNPC : GlobalNPC
{
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
    public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
    {
        if (npc.HasBuff<WrathOfBlueMoon>())
        {
            modifiers.SourceDamage -= .4f;
        }
    }
    public int HallowedGaze_Count = 0;
    public int WrathOfBlueMoon = 0;
    public int FuryOfTheSun = 0;
    public int ElectricConductor = 0;
    public bool ElectricConductorUpgrade = false;
    public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
    {
        NPC_Debuff(npc, ref modifiers);
    }
    public int CursedSkullStatus = 0;
    public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
    {
        NPC_Debuff(npc, ref modifiers);
        if (projectile.Check_ItemTypeSource(ModContent.ItemType<SkullRevolver>()))
        {
            if (npc.HasBuff<CursedStatus>())
            {
                if (++CursedSkullStatus >= 3)
                {
                    CursedSkullStatus = 3;
                }
                if (projectile.type == ProjectileID.BookOfSkullsSkull)
                {
                    modifiers.SourceDamage += 1;
                }
            }
        }
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
        if (npc.HasBuff<WrathOfBlueMoon>())
        {
            modifiers.SourceDamage += .1f * WrathOfBlueMoon;
        }
        if (npc.HasBuff<FuryOfTheSun>())
        {
            modifiers.SourceDamage += .1f * FuryOfTheSun;
        }
        if (npc.HasBuff<HallowedGaze>())
        {
            modifiers.SourceDamage += .05f * HallowedGaze_Count;
        }
        modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
        modifiers.SourceDamage *= Math.Clamp(1 - Endurance, 0, 1f);
    }
    public int HitCount = 0;
    public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
    {
        HitCount++;
        if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ElectricConductor].Contains(item.type))
        {
            if (npc.HasBuff(BuffID.Electrified))
            {
                ElectricConductorUpgrade = true;
                ElectricConductor = 0;
            }
            else
            {
                ElectricConductorUpgrade = false;
            }
            if (++ElectricConductor >= 10)
            {
                ElectricConductor = 10;
                if (Main.rand.NextBool(10))
                {
                    npc.AddBuff(BuffID.Electrified, 60 + player.itemAnimationMax);
                }
            }
            if (ElectricConductorUpgrade)
            {
                npc.Center.LookForHostileNPC(out List<NPC> listnpc, 150 + npc.Size.Length());
                foreach (var target in listnpc)
                {
                    if (target.whoAmI == npc.whoAmI)
                    {
                        continue;
                    }
                    player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(hit.Damage * .1f) + 1, 1));
                    if (Main.rand.NextBool(35))
                    {
                        Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), npc.Center, (target.Center - npc.Center).SafeNormalize(Vector2.Zero) * 10, ProjectileID.ThunderSpearShot, hit.Damage, 0, player.whoAmI);
                        proj.penetrate = 10;
                        proj.maxPenetrate = 10;
                    }
                }
            }
        }
        if (npc.HasBuff<WrathOfBlueMoon>())
        {
            if (++WrathOfBlueMoon >= 20)
            {
                WrathOfBlueMoon = 20;
                if (Main.rand.NextBool(10))
                {
                    Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item), npc.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<SimplePiercingProjectile2>(), 30 + (int)(npc.life * .01f), 0, player.whoAmI, 2, 30, 5);
                    if (proj.ModProjectile is SimplePiercingProjectile2 modproj)
                    {
                        modproj.ProjectileColor = Color.Blue;
                    }
                }
            }
        }
        if (npc.HasBuff<FuryOfTheSun>())
        {
            if (++FuryOfTheSun >= 20)
            {
                FuryOfTheSun = 20;
            }
            if (Main.rand.NextBool(10))
            {
                OnHitEffect(npc, player, hit);
            }
        }
        if (npc.HasBuff<HallowedGaze>())
        {
            if (HallowedGaze_Count >= 12)
            {
                Vector2 playerPos = player.Center;
                Vector2 pos = new Vector2(npc.Center.X + Main.rand.Next(-100, 100), playerPos.Y - 800);
                Projectile.NewProjectile(player.GetSource_ItemUse(item), pos, (npc.Center - pos), ModContent.ProjectileType<HitScanShotv2>(), 1, 0, player.whoAmI);
            }
        }
    }
    private void OnHitEffect(NPC npc, Player player, NPC.HitInfo hit)
    {
        npc.Center.LookForHostileNPC(out List<NPC> npclist, 175);
        foreach (NPC target in npclist)
        {
            if (npc.whoAmI == target.whoAmI)
            {
                continue;
            }
            player.StrikeNPCDirect(target, hit);
        }
        for (int i = 0; i < 150; i++)
        {
            int smokedust = Dust.NewDust(npc.Center, 0, 0, DustID.Smoke);
            Main.dust[smokedust].noGravity = true;
            Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(14, 14);
            Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
            int dust = Dust.NewDust(npc.Center, 0, 0, DustID.Torch);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Main.rand.NextVector2Circular(14, 14);
            Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
        }
    }
    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        HitCount++;
        if (projectile.owner == Main.myPlayer)
        {
            Player player = Main.player[projectile.owner];
            if (npc.HasBuff<HallowedGaze>())
            {
                if (HallowedGaze_Count >= 12)
                {
                    Vector2 playerPos = Main.player[projectile.owner].Center;
                    Vector2 pos = new Vector2(npc.Center.X + Main.rand.Next(-100, 100), playerPos.Y - 800);
                    Projectile.NewProjectile(projectile.GetSource_FromAI(), pos, npc.Center - pos, ModContent.ProjectileType<HitScanShotv2>(), 1, 0, projectile.owner);
                }
            }

            if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ElectricConductor].Contains(projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType))
            {
                if (npc.HasBuff(BuffID.Electrified))
                {
                    ElectricConductorUpgrade = true;
                    ElectricConductor = 0;
                }
                else
                {
                    ElectricConductorUpgrade = false;
                }
                if (++ElectricConductor >= 10)
                {
                    ElectricConductor = 10;
                    if (Main.rand.NextBool(10))
                    {
                        npc.AddBuff(BuffID.Electrified, 60 + player.itemAnimationMax);
                    }
                }
                if (ElectricConductorUpgrade)
                {
                    npc.Center.LookForHostileNPC(out List<NPC> listnpc, 100);
                    foreach (var target in listnpc)
                    {
                        if (target.whoAmI == npc.whoAmI)
                        {
                            continue;
                        }
                        player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(hit.Damage * .1f) + 1, 1));
                    }
                }
            }
            if (npc.HasBuff<WrathOfBlueMoon>())
            {
                if (++WrathOfBlueMoon >= 20)
                {
                    WrathOfBlueMoon = 20;
                    if (Main.rand.NextBool(10))
                    {
                        Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), npc.Center, Main.rand.NextVector2CircularEdge(1, 1), ModContent.ProjectileType<SimplePiercingProjectile2>(), 30 + (int)(npc.life * .01f), 0, projectile.owner, 2, 30, 5);
                        if (proj.ModProjectile is SimplePiercingProjectile2 modproj)
                        {
                            modproj.ProjectileColor = Color.Blue;
                        }
                    }
                }
            }
            if (npc.HasBuff<FuryOfTheSun>())
            {
                if (++FuryOfTheSun >= 20)
                {
                    FuryOfTheSun = 20;
                }
                if (Main.rand.NextBool(10))
                {
                    OnHitEffect(npc, player, hit);
                }
            }
        }
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
