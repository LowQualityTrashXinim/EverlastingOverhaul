using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Roguelike.Common.Utils;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace EverlastingOverhaul.Common.Global;
internal class RoguelikeGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;
	public bool CanThisProjectileBeDevoured = true;
	public int Source_ItemType = -1;
	public int Source_ProjectileType = -1;
	public string Source_CustomContextInfo = string.Empty;
	public bool Source_FromDeathScatterShot = false;
	public bool IsFromMinion = false;
	public bool IsFromRelic = false;
	public bool IsFromBoss = false;
	public int OnKill_ScatterShot = -1;
	public float TravelDistanceBeforeKill = -1f;
	public float VelocityMultiplier = 1f;
	public float CritDamage = 0;
	public int EnergyRegainOnHit = 0;
	public int SetCrit = 0;
	public int CustomDataValue = 0;
	/// <summary>
	/// This is for projectile that is spawned via duplicate projectile method<br/><br/>
	/// <b>Return true if it is from duplication</b>
	/// </summary>
	public bool IsASubProjectile = false;
	public int InitialTimeLeft { get; private set; } = 0;
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source is null) {
			return;
		}
		InitialTimeLeft = projectile.timeLeft;
		if (source is EntitySource_ItemUse parent) {
			Source_ItemType = parent.Item.type;
		}
		if (source is EntitySource_Misc parent2 && parent2.Context == "OnKill_ScatterShot") {
			Source_FromDeathScatterShot = true;
		}
		if (source is EntitySource_Parent parent3) {
			if (parent3.Entity is Projectile possibly) {
				Source_ProjectileType = possibly.type;
				if (possibly.minion) {
					IsFromMinion = true;
				}
				//Attempt to get item source from that minion
				if (possibly.TryGetGlobalProjectile(out RoguelikeGlobalProjectile global)) {
					if (global.Source_ItemType != 0) {
						Source_ItemType = global.Source_ItemType;
					}
				}
			}
			if (parent3.Context != null) {
				if (parent3.Context == "subProj") {
					IsASubProjectile = true;
				}
			}
			if (parent3.Entity is NPC npc) {
				if (npc.boss) {
					IsFromBoss = true;
				}
			}
		}
		Source_CustomContextInfo = source.Context;
	}
	public override bool PreAI(Projectile projectile) {
		if (VelocityMultiplier != 0) {
			projectile.velocity /= VelocityMultiplier;
		}
		else {
			projectile.velocity /= .001f;
		}
		return base.PreAI(projectile);
	}
	public override void PostAI(Projectile projectile) {
		if (VelocityMultiplier != 0) {
			projectile.velocity *= VelocityMultiplier;
		}
		else {
			projectile.velocity *= .001f;
		}
		var player = Main.player[projectile.owner];
		if (projectile.hostile) {
			VelocityMultiplier = 1f + player.GetModPlayer<PlayerStatsHandle>().Hostile_ProjectileVelocityAddition;
			player.GetModPlayer<PlayerStatsHandle>().Hostile_ProjectileVelocityAddition = 0;
		}
		else {
			VelocityMultiplier = 1f;
		}
		if (TravelDistanceBeforeKill > 0 && Vector2.DistanceSquared(player.Center, projectile.Center) >= TravelDistanceBeforeKill * TravelDistanceBeforeKill) {
			projectile.Kill();
		}
	}
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Source_CustomContextInfo == "Skill" && projectile.type == ProjectileID.Blizzard) {
			var player = Main.player[projectile.owner];
			target.AddBuff(BuffID.Frozen, ModUtils.ToSecond(Main.rand.Next(4, 7)));
			target.Center.LookForHostileNPC(out var npclist, 75);
			var hitweaker = hit;
			hitweaker.Damage = (int)(hit.Damage * .54f);
			foreach (var npc in npclist) {
				if (npc.whoAmI == target.whoAmI) {
					continue;
				}
				player.StrikeNPCDirect(npc, hitweaker);
			}
		}
		if (Source_ItemType == ItemID.CopperBow && projectile.type != ProjectileID.Electrosphere) {
			if (Main.rand.NextFloat() <= .15f) {
				int min = Math.Max(projectile.damage / 4, 1);
				Projectile proj = Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), projectile.Center, Vector2.Zero, ProjectileID.Electrosphere, min, projectile.knockBack, projectile.owner);
				proj.timeLeft = 30;
			}
		}
	}
	public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		if (projectile.type == ProjectileID.StarCannonStar
			|| projectile.type == ProjectileID.Starfury
			|| projectile.type == ProjectileID.StarWrath) {
			fallThrough = true;
		}
		return base.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}
	public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
		Player player = Main.player[projectile.owner];
		modifiers.CritDamage += CritDamage;
	}
	public override void OnKill(Projectile projectile, int timeLeft) {
		var player = Main.player[projectile.owner];
		if (Source_FromDeathScatterShot
			|| OnKill_ScatterShot <= 0
			|| player.heldProj == projectile.owner
			|| ProjectileID.Sets.SingleGrappleHook[projectile.type]
			|| projectile.hostile
			|| !projectile.friendly
			|| projectile.minion
			|| projectile.aiStyle == 4
			|| projectile.aiStyle == 19
			|| projectile.aiStyle == 39
			|| projectile.aiStyle == 46
			|| projectile.aiStyle == 75
			|| projectile.aiStyle == 99
			|| projectile.aiStyle == 101
			|| projectile.minion
			|| projectile.sentry
			|| projectile.type == ProjectileID.PhantasmArrow
			|| projectile.type == ProjectileID.IchorDart
			|| projectile.type == ProjectileID.ExplosiveBunny
			|| projectile.type == ProjectileID.FinalFractal
			|| projectile.type == ProjectileID.PortalGun
			|| projectile.type == ProjectileID.PortalGunBolt
			|| projectile.type == ProjectileID.PortalGunGate
			|| projectile.type == ProjectileID.LightsBane
			|| projectile.aiStyle == ProjAIStyleID.Hook) {
			return;
		}
		if (!projectile.velocity.IsLimitReached(1)) {
			projectile.velocity *= Main.rand.NextFloat(5, 7);
		}
		for (int i = 0; i < OnKill_ScatterShot; i++) {
			int proj = Projectile.NewProjectile(projectile.GetSource_Misc("OnKill_ScatterShot"), projectile.Center, projectile.velocity.Vector2RotateByRandom(360), projectile.type, (int)(projectile.damage * .65f), projectile.knockBack * .55f, projectile.owner);
			Main.projectile[proj].timeLeft = 120;
		}
	}
}
