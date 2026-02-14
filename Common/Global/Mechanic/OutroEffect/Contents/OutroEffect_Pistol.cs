using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_Pistol : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(30);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Pistol].Contains(item.type)) {
			damage += .15f;
		}
	}
	public override void WeaponCrit(Player player, Item item, ref float crit) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Pistol].Contains(item.type)) {
			crit += 5;
		}
	}
	public override void ModifyHitProj(Player player, Projectile proj, NPC npc, ref NPC.HitModifiers mod) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Pistol].Contains(proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			if(Main.rand.NextFloat() <= .15f) {
				mod.SourceDamage += .5f;
			}
		}
	}
}
