using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global.Mechanic;
internal class OverCrit_Player : ModPlayer {
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		int crit = Player.GetWeaponCrit(item);
		while (crit > 100) {
			crit -= 100;
			if (Main.rand.Next(1, 101) <= crit) {
				modifiers.CritDamage += 1;
			}
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		int crit = proj.CritChance;
		while (crit > 100) {
			crit -= 100;
			if (Main.rand.Next(1, 101) <= crit) {
				modifiers.CritDamage += 1;
			}
		}
	}
}
