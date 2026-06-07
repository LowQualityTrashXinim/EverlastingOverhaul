using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Common;
internal class Roguelike_FuryOfTheSun : GlobalItem {
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (item.type == ItemID.FieryGreatsword) {
			target.AddBuff<FuryOfTheSun>(ModUtils.ToSecond(3));
		}
	}
}
public class Roguelike_FuryOfTheSun_Projectile : GlobalProjectile {
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.FuryOfTheSun].Contains(projectile.GetGlobalProjectile<RoguelikeGlobalProjectile>().Source_ItemType)) {
			target.AddBuff<FuryOfTheSun>(ModUtils.ToSecond(5));
		}
	}
}
public class Roguelike_FuryOfTheSun_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int FuryOfTheSun = 0;
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<FuryOfTheSun>()) {
			modifiers.SourceDamage += .01f * FuryOfTheSun;
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<FuryOfTheSun>()) {
			modifiers.SourceDamage += .01f * FuryOfTheSun;
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (npc.HasBuff<FuryOfTheSun>()) {
			if (++FuryOfTheSun >= 20) {
				FuryOfTheSun = 20;
			}
			if (Main.rand.NextBool(10)) {
				OnHitEffect(npc, player, hit);
			}
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (projectile.owner != Main.myPlayer) {
			return;
		}
		Player player = Main.player[projectile.owner];
		if (npc.HasBuff<FuryOfTheSun>()) {
			if (++FuryOfTheSun >= 20) {
				FuryOfTheSun = 20;
			}
			if (Main.rand.NextBool(10)) {
				OnHitEffect(npc, player, hit);
			}
		}
	}
	private void OnHitEffect(NPC npc, Player player, NPC.HitInfo hit) {
		npc.Center.LookForHostileNPC(out List<NPC> npclist, 175);
		foreach (NPC target in npclist) {
			if (npc.whoAmI == target.whoAmI) {
				continue;
			}
			player.StrikeNPCDirect(target, hit);
		}
		for (int i = 0; i < 150; i++) {
			int smokedust = Dust.NewDust(npc.Center, 0, 0, DustID.Smoke);
			Main.dust[smokedust].noGravity = true;
			Main.dust[smokedust].velocity = Main.rand.NextVector2Circular(14, 14);
			Main.dust[smokedust].scale = Main.rand.NextFloat(.75f, 2f);
			int dust = Dust.NewDust(npc.Center, 0, 0, DustID.Torch);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Main.rand.NextVector2Circular(14, 14);
			Main.dust[dust].scale = Main.rand.NextFloat(.75f, 2f);
		}
	}
}
internal class FuryOfTheSun : ModBuff {
	public override string Texture => ModTexture.EMPTYDEBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		npc.lifeRegen -= 35;
		if (npc.HasBuff(BuffID.OnFire) || npc.HasBuff(BuffID.OnFire)) {
			npc.lifeRegen -= 15;
		}
		if (npc.buffTime[buffIndex] <= 0) {
			npc.GetGlobalNPC<Roguelike_FuryOfTheSun_GlobalNPC>().FuryOfTheSun = 0;
		}
	}
}
