using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents.MainBranch;
internal class OutroEffect_Rifle : OutroEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(40);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Rifle].Contains(item.type)) {
			damage += .15f;
		}
	}
	public override void ModifyHitProj(Player player, Projectile proj, NPC npc, ref NPC.HitModifiers mod) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Rifle].Contains(proj.GetGlobalProjectile<EverlastingOverhaulGlobalProjectile>().Source_ItemType)) {
			if (Main.rand.NextBool()) {
				mod.SourceDamage += .4f;
			}
			mod.ScalingArmorPenetration += .1f;
		}
	}
	public override void ModifyHitItem(Player player, NPC npc, ref NPC.HitModifiers mod) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Rifle].Contains(player.HeldItem.tileBoost)) {
			if (Main.rand.NextBool()) {
				mod.SourceDamage += .4f;
			}
			mod.ScalingArmorPenetration += .1f;
		}
	}
}
