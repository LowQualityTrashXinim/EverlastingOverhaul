using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
public class Roguelike_ElectricConductor_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int ElectricConductor = 0;
	public bool ElectricConductorUpgrade = false;
	public override void PostAI(NPC npc) {
		if (npc.HasBuff(BuffID.Electrified)) {
			if (ElectricConductorUpgrade) {
				if (Main.rand.NextBool(35)) {
					int damage = Math.Clamp(npc.life / 20 + 1, 1, int.MaxValue);
					Projectile proj = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, Main.rand.NextVector2CircularEdge(10, 10), ProjectileID.ThunderSpearShot, damage, 0, npc.target);
					proj.penetrate = 10;
					proj.maxPenetrate = 10;
				}
			}
		}
		else {
			ElectricConductorUpgrade = false;
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.owner != Main.myPlayer) {
			return;
		}
		Player player = Main.player[projectile.owner];
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ElectricConductor].Contains(projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			if (npc.HasBuff(BuffID.Electrified)) {
				ElectricConductorUpgrade = true;
				ElectricConductor = 0;
			}
			else {
				ElectricConductorUpgrade = false;
			}
			if (++ElectricConductor >= 10) {
				ElectricConductor = 10;
				if (Main.rand.NextBool(10)) {
					npc.AddBuff(BuffID.Electrified, 60 + player.itemAnimationMax);
				}
			}
			if (ElectricConductorUpgrade) {
				npc.Center.LookForHostileNPC(out List<NPC> listnpc, 100);
				foreach (var target in listnpc) {
					if (target.whoAmI == npc.whoAmI) {
						continue;
					}
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(hit.Damage * .1f) + 1, 1));
				}
			}
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ElectricConductor].Contains(item.type)) {
			if (npc.HasBuff(BuffID.Electrified)) {
				ElectricConductorUpgrade = true;
				ElectricConductor = 0;
			}
			else {
				ElectricConductorUpgrade = false;
				if (++ElectricConductor >= 10) {
					ElectricConductor = 10;
					if (Main.rand.NextBool(4)) {
						npc.AddBuff(BuffID.Electrified, 600 + player.itemAnimationMax);
					}
				}
			}
			if (ElectricConductorUpgrade) {
				npc.Center.LookForHostileNPC(out List<NPC> listnpc, 150 + npc.Size.Length());
				foreach (var target in listnpc) {
					if (target.whoAmI == npc.whoAmI) {
						continue;
					}
					player.StrikeNPCDirect(npc, npc.CalculateHitInfo((int)(hit.Damage * .1f) + 1, 1));
				}
			}
		}
	}
}
