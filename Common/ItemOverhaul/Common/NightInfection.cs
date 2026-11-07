using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Common;
internal class NightInfection_Weapon : GlobalItem {
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.NightInfection].Contains(item.type)) {
			target.AddBuff<NightInfection>(ModUtils.ToSecond(5));
		}
	}
}
public class NightInfection_Projectile : GlobalProjectile {
	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (OutroEffectSystem.Get_Arr_WeaponTag[(int)WeaponTag.NightInfection].Contains(projectile.GetGlobalProjectile<EverlastingOverhaulGlobalProjectile>().Source_ItemType)) {
			target.AddBuff<NightInfection>(ModUtils.ToSecond(5));
		}
	}
}
public class NightInfection_GlobalNPC : GlobalNPC {
	public override bool InstancePerEntity => true;
	public int Infection = 0;
	public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<NightInfection>()) {
			if (npc.HasBuff(BuffID.ShadowFlame)) {
				modifiers.SourceDamage *= 1.25f;
			}
			modifiers.SourceDamage += .05f * Infection;
		}
	}
	public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers) {
		if (npc.HasBuff<NightInfection>()) {
			if (npc.HasBuff(BuffID.ShadowFlame)) {
				modifiers.SourceDamage *= 1.25f;
			}
			modifiers.SourceDamage += .05f * Infection;
		}
	}
	public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone) {
		if (npc.HasBuff<NightInfection>()) {
			Infection = Math.Clamp(Infection + 1, 0, 20);
			if (!Main.rand.NextBool(5)) {
				return;
			}
			var vec = Vector2.UnitX;
			for (int i = 0; i < 3; i++) {
				var vec2 = vec.Vector2DistributeEvenly(3, 360, i) * 3;
				Projectile.NewProjectile(player.GetSource_ItemUse(item), npc.Center, vec2, ProjectileID.ShadowFlame, item.damage, 3f, player.whoAmI, Main.rand.NextFloat(-.05f, .05f), Main.rand.NextFloat(-.1f, .1f));
			}
		}
	}
	public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone) {
		if (npc.HasBuff<NightInfection>()) {
			Infection = Math.Clamp(Infection + 1, 0, 20);
			if (!Main.rand.NextBool(5) ||
				projectile.type == ProjectileID.ShadowFlame 
				|| projectile.Check_ItemTypeSource(ItemID.ShadowFlameHexDoll)) {
				return;
			}
			var vec = Vector2.UnitX;
			for (int i = 0; i < 3; i++) {
				var vec2 = vec.Vector2DistributeEvenly(3, 360, i) * 3;
				Projectile.NewProjectile(projectile.GetSource_FromAI(), npc.Center, vec2, ProjectileID.ShadowFlame, projectile.damage, 3f, projectile.owner, Main.rand.NextFloat(-.05f, .05f), Main.rand.NextFloat(-.1f, .1f));
			}
		}
	}
}
public class NightInfection_ModPlayer : ModPlayer {
}
public class NightInfection : ModBuff {
	public override string Texture => ModTexture.MissingTexture_Default;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultDeBuff();
	}
	public override void Update(NPC npc, ref int buffIndex) {
		var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.DemonTorch);
		dust.noGravity = true;
		dust.scale = Main.rand.NextFloat(1, 1.25f);
		dust.velocity = -Vector2.UnitY * Main.rand.NextFloat(0.5f, 1.5f);
		npc.GetGlobalNPC<EverlastingOverhaulGlobalNPC>().StatDefense.Base -= 20;
		npc.lifeRegen -= 10 + npc.GetGlobalNPC<NightInfection_GlobalNPC>().Infection * 2;
	}
}
