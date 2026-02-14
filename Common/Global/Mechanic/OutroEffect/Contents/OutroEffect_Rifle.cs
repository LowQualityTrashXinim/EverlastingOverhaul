using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_Rifle : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(30);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Rifle].Contains(item.type)) {
			damage += .15f;
		}
	}
	public override void ModifyHitProj(Player player, Projectile proj, NPC npc, ref NPC.HitModifiers mod) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Rifle].Contains(proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			if (Main.rand.NextBool()) {
				mod.SourceDamage += .1f;
			}
			mod.ScalingArmorPenetration += .1f;
		}
	}
}
