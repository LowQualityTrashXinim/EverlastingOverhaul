using EverlastingOverhaul.Common.Utils;
using System;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents.SubBranch;
internal class OutroEffect_DeathSickle : OutroEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(10);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		damage += .5f;
	}
	public override void WeaponCrit(Player player, Item item, ref float crit) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ReaperMark].Contains(item.type)) {
			crit += 50f;
		}
	}
	public override void Update(Player player) {
		player.ModPlayerStats().UpdateCritDamage += .5f;
	}
	public override void ModifyHitItem(Player player, NPC npc, ref NPC.HitModifiers mod) {
		mod.SourceDamage.Flat += Math.Clamp(npc.life * .05f, 1, 2000);
	}

	public override void ModifyHitProj(Player player, Projectile proj, NPC npc, ref NPC.HitModifiers mod) {
		mod.SourceDamage.Flat += Math.Clamp(npc.life * .05f, 1, 2000);
	}
}
