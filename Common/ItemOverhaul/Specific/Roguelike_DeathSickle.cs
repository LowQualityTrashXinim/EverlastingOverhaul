using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents.SubBranch;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Contents.Items.Weapon;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class EverlastingOverhaul_DeathSickle : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.DeathSickle;
	public override void SetDefaults(Item entity) {
		entity.damage = 75;
		entity.GetGlobalItem<GlobalItemHandle>().OutroEffect_type = OutroEffect.GetOutroEffectType<OutroEffect_DeathSickle>();
	}
	public override void HoldItem(Item item, Player player) {
		player.ModPlayerStats().Chance_ToInstantKill += .01f;
	}
}
public class EverlastingOverhaul_DeathSickle_ModBuff : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		this.BossRushSetDefaultBuff();
	}
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateHPRegen.Base += 10;
		player.GetDamage(DamageClass.Generic) += .25f;
		player.GetCritChance(DamageClass.Generic) += 10;
	}
}
public class EverlastingOverhaul_DeathSickle_GlobalNPC : GlobalNPC {
	public override void OnKill(NPC npc) {
		if (npc.lastInteraction < 0 || npc.lastInteraction >= 255) {
			return;
		}
		Player player = Main.player[npc.lastInteraction];
		if (player.HeldItem.type == ItemID.DeathSickle) {
			Projectile.NewProjectile(npc.GetSource_FromAI(),
				npc.Center,
				Vector2.Zero,
				ModContent.ProjectileType<EverlastingOverhaul_DeathSickle_SoulProjectile>(),
				0, 0, player.whoAmI
				);
		}
	}
}
internal class EverlastingOverhaul_DeathSickle_SoulProjectile : ModProjectile {
	public override string Texture => ModTexture.SMALLWHITEBALL;
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 15;
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 60;
	}
	public override void AI() {
		Player player = Main.player[Projectile.owner];
		if (Projectile.Center.IsCloseToPosition(player.Center, 30)) {
			player.AddBuff<EverlastingOverhaul_DeathSickle_ModBuff>(300);
			Projectile.Kill();
		}
	}
	public override Color? GetAlpha(Color lightColor) {
		return Color.White;
	}
	public override bool PreDraw(ref Color lightColor) {
		Projectile.DrawTrail(lightColor);
		return base.PreDraw(ref lightColor);
	}
}
