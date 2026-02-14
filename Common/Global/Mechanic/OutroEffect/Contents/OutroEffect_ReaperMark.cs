using Terraria;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_ReaperMark : WeaponEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(12);
	}
	public override void Update(Player player) {
		player.ModPlayerStats().UpdateFullHPDamage += 1;
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ReaperMark].Contains(item.type)) {
			damage += .35f;
		}
	}
	public override void WeaponCrit(Player player, Item item, ref float crit) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ReaperMark].Contains(item.type)) {
			crit += 15f;
		}
	}
	public override void ModifyHitItem(Player player, NPC npc, ref NPC.HitModifiers mod) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ReaperMark].Contains(player.HeldItem.type)) {
			if (npc.GetLifePercent() <= .1f && !npc.boss) {
				npc.StrikeInstantKill();
			}
		}
	}
	public override void ModifyHitProj(Player player, Projectile proj, NPC npc, ref NPC.HitModifiers mod) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ReaperMark].Contains(proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			if (npc.GetLifePercent() <= .1f && !npc.boss) {
				npc.StrikeInstantKill();
			}
		}
	}
}
