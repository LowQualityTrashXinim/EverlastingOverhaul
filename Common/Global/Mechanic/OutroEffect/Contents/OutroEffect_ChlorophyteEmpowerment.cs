using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
internal class OutroEffect_ChlorophyteEmpowerment : OutroEffect {
	public override void SetStaticDefaults() {
		Duration = ModUtils.ToSecond(15);
	}
	public override void Update(Player player) {
		player.ModPlayerStats().UpdateHPRegen.Base += 5;
	}
	public override void WeaponDamage(Player player, Item item, ref StatModifier damage) {
		damage += .1f;
	}
	public override void ModifyHitItem(Player player, NPC npc, ref NPC.HitModifiers mod) {
		if (Main.rand.NextBool(10)) {
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), npc.Center, Main.rand.NextVector2RectangleEdge(5, 5), ProjectileID.SporeCloud, (int)(player.GetWeaponDamage(player.HeldItem) * .65f), 0, player.whoAmI);
		}
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ChlorophyteEmpowerment].Contains(player.HeldItem.type)) {
			mod.FinalDamage += .2f;
		}
	}
	public override void ModifyHitProj(Player player, Projectile proj, NPC npc, ref NPC.HitModifiers mod) {
		if (Main.rand.NextBool(10) && proj.type != ProjectileID.SporeCloud) {
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), npc.Center, Main.rand.NextVector2RectangleEdge(5, 5), ProjectileID.SporeCloud, (int)(player.GetWeaponDamage(player.HeldItem) * .65f), 0, player.whoAmI);
		}
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.ChlorophyteEmpowerment].Contains(proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			mod.FinalDamage += .2f;
		}
	}
}
