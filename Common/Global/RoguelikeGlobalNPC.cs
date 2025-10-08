using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global;
internal class RoguelikeGlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int HeatRay_Decay = 0;
	public int HeatRay_HitCount = 0;

	public int GolemFist_HitCount = 0;

	public StatModifier StatDefense = new StatModifier();
	public float Endurance = 0;
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
	public int PositiveLifeRegen = 0;
	public int PositiveLifeRegenCount = 0;
	public int Perpetuation_PointStack = 0;
	public override void ResetEffects(NPC npc) {
		StatDefense = new();
		Endurance = 0;
	}
	public override bool PreAI(NPC npc) {
		if (VelocityMultiplier != 0) {
			npc.velocity /= VelocityMultiplier + static_velocityMultiplier - 1;
		}
		else {
			npc.velocity /= .001f;
		}
		return base.PreAI(npc);
	}
	public override void PostAI(NPC npc) {
		if (VelocityMultiplier != 0) {
			npc.velocity *= VelocityMultiplier + static_velocityMultiplier - 1;
		}
		else {
			npc.velocity *= .001f;
		}
		VelocityMultiplier = 1;
		if (HeatRay_HitCount > 0) {
			HeatRay_Decay = ModUtils.CountDown(HeatRay_Decay);
			if (HeatRay_Decay <= 0) {
				HeatRay_HitCount--;
			}
		}
		if (++PositiveLifeRegenCount >= 60) {
			PositiveLifeRegenCount = 0;
			npc.life = Math.Clamp(npc.life + PositiveLifeRegen, 0, npc.lifeMax);
		}
	}
	public override void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers) {
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
		modifiers.FinalDamage *= 1 - Endurance;
	}
	public int CursedSkullStatus = 0;
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (projectile.type == ProjectileID.HeatRay) {
			modifiers.SourceDamage += HeatRay_HitCount * .02f;
		}
		modifiers.Defense = modifiers.Defense.CombineWith(StatDefense);
		modifiers.FinalDamage *= 1 - Endurance;
		if (projectile.type == ProjectileID.GolemFist) {
			if (++GolemFist_HitCount % 3 == 0) {
				modifiers.SourceDamage += 1.5f;
			}
		}
	}
	public int HitCount = 0;
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		HitCount++;
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		HitCount++;
		if (projectile.type == ProjectileID.HeatRay) {
			HeatRay_HitCount = Math.Clamp(HeatRay_HitCount + 1, 0, 200);
			HeatRay_Decay = 30;
		}
		else if (projectile.type == ProjectileID.GolemFist) {
			if (GolemFist_HitCount % 3 == 0) {
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.HeatRay);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2Circular(20, 20);
					dust.scale += Main.rand.NextFloat();
				}
				for (int i = 0; i < 100; i++) {
					Dust dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.HeatRay);
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2CircularEdge(25, 25);
					dust.scale += Main.rand.NextFloat();
				}
				SoundEngine.PlaySound(SoundID.Item14, npc.Center);
				npc.Center.LookForHostileNPC(out List<NPC> npclist, 150);
				npc.TargetClosest();
				Player player = Main.player[npc.target];
				foreach (var target in npclist) {
					if (target.whoAmI != npc.whoAmI) {
						player.StrikeNPCDirect(target, target.CalculateHitInfo(hit.Damage, -1));
					}
				}
			}
		}
	}
	public override void OnKill(NPC npc) {
		int playerIndex = npc.lastInteraction;
		if (!Main.player[playerIndex].active || Main.player[playerIndex].dead) {
			playerIndex = npc.FindClosestPlayer();
		}
		var player = Main.player[playerIndex];
		player.GetModPlayer<PlayerStatsHandle>().successfullyKillNPCcount++;
		player.GetModPlayer<PlayerStatsHandle>().NPC_HitCount = HitCount;
	}
	public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
		return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
	}
}
