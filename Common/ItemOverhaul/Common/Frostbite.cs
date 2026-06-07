using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
public class Frostbite_ModPlayer : ModPlayer {
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Frostbite].Contains(item.type)) {
			target.AddBuff<Frostbite>(ModUtils.ToSecond(5));
		}
	}
	public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.Frostbite].Contains(proj.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			target.AddBuff<Frostbite>(ModUtils.ToSecond(5));
		}
	}
}
internal class Frostbite_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int Frostbite_Stack = 0;
	public bool Frostbite = false;
	public override void ResetEffects(NPC npc) {
		Frostbite = npc.HasBuff<Frostbite>();
	}
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		Modify_Frostbite(npc, ref modifiers);
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		Modify_Frostbite(npc, ref modifiers);
	}
	private void Modify_Frostbite(NPC npc, ref NPC.HitModifiers modifiers) {
		if (Frostbite) {
			if (npc.HasBuff(BuffID.Chilled) || npc.HasBuff(BuffID.Frozen)) {
				modifiers.SourceDamage += 10;
				int index = npc.FindBuffIndex(BuffID.Chilled);
				if (index != -1) {
					npc.DelBuff(index);
				}
				else {
					npc.DelBuff(npc.FindBuffIndex(BuffID.Frozen));
				}

			}
			modifiers.SourceDamage += Frostbite_Stack * .02f;
			if (Main.rand.NextBool()) {
				modifiers.SourceDamage += .5f;
			}
		}
		if (Frostbite_Stack >= 20) {
			modifiers.SourceDamage += 4.2f;
			Frostbite_Stack = 0;
			Frostbite = false;
		}
	}
}
public class Frostbite : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<RoguelikeGlobalNPC>().VelocityMultiplier -= .34f;
		if (npc.buffTime[buffIndex] <= 0) {
			Frostbite_GlobalNPC global = npc.GetGlobalNPC<Frostbite_GlobalNPC>();
			global.Frostbite_Stack = 0;
		}
	}
	public override bool ReApply(NPC npc, int time, int buffIndex) {
		Frostbite_GlobalNPC global = npc.GetGlobalNPC<Frostbite_GlobalNPC>();
		if (global.Frostbite_Stack < 20) {
			global.Frostbite_Stack++;
		}
		return base.ReApply(npc, time, buffIndex);
	}
}
