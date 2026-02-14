using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_Greatsword : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(30);
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Greatsword].Contains(item.type)) {
			damage += .25f;
		}
	}
	public override void WeaponKnockBack(Player player, Item item, ref StatModifier knockback) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Greatsword].Contains(item.type)) {
			knockback += .25f;
		}
	}
	public override void ModifyHitItem(Player player, NPC npc, ref NPC.HitModifiers mod) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Greatsword].Contains(player.HeldItem.type)) {
			mod.SourceDamage += npc.life * .01f;
		}
	}
	public override void ModifyHitProj(Player player, Projectile proj, NPC npc, ref NPC.HitModifiers mod) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Greatsword].Contains(proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			mod.SourceDamage += npc.life * .01f;
		}
	}
}
