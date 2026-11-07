using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_GolemFist : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.GolemFist;
	public override void SetDefaults(Item entity) {
		entity.damage = 110;
	}
}

public class EverlastingOverhaul_GolemFist_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int GolemFist_HitCount = 0;
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (projectile.type == ProjectileID.GolemFist) {
			if (++GolemFist_HitCount % 3 == 0) {
				modifiers.SourceDamage += 1.5f;
			}
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.type == ProjectileID.GolemFist) {
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
}
