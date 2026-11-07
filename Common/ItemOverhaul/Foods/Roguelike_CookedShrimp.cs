using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_CookedShrimp : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.CookedShrimp;
	public override int CoolDownBetweenUse() => 240;
	public override int LifeAmount() => 60;
	public override int ManaAmount() => 150;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.25f);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_CookedShrimp_Buff>(), ModUtils.ToMinute(18));
	}
}
public class EverlastingOverhaul_CookedShrimp_Buff : FoodItemTier2 {
	public override int TypeID => ItemID.CookedShrimp;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<EverlastingOverhaul_CookedShrimp_ModPlayer>().CookedShrimp = true;
	}
}
public class EverlastingOverhaul_CookedShrimp_ModPlayer : ModPlayer {
	public bool CookedShrimp = false;
	public int Cooldown = 0;
	public override void ResetEffects() {
		CookedShrimp = false;
		Cooldown = ModUtils.CountDown(Cooldown);
	}
	public override void PostUpdate() {
		if (CookedShrimp && Cooldown <= 0) {
			int type = ModContent.NPCType<EverlastingOverhaul_CookedShrimp_NPC>();
			if (NPC.CountNPCS(type) > 10) {
				return;
			}
			Cooldown = ModUtils.ToSecond(5);
			Vector2 position = Player.Center + Main.rand.NextVector2CircularEdge(500, 500) * Main.rand.NextFloat(.85f, 1.5f);
			Point pos = position.ToPoint();
			NPC.NewNPC(NPC.GetSource_NaturalSpawn(), pos.X, pos.Y, type);
		}
	}
}
public class EverlastingOverhaul_CookedShrimp_NPC : ModNPC {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Shrimp);
	public override void SetDefaults() {
		NPC.width = NPC.height = 32;
		NPC.lifeMax = 500;
		NPC.damage = 0;
		NPC.defense = 0;
		NPC.timeLeft = 120;
		NPC.chaseable = false;
		NPC.behindTiles = false;
		NPC.knockBackResist = 0f;
		NPC.noTileCollide = true;
		NPC.GravityIgnoresType = true;
	}
	public override void AI() {
		NPC.GravityMultiplier *= 0;
		NPC.ai[0]++;
		if (NPC.ai[0] >= 90) {
			NPC.ai[0] = 0;
			NPC.ai[1]++;
		}
		if (NPC.ai[1] == 0) {
			NPC.velocity.Y = .25f;
		}
		if (NPC.ai[1] % 2 == 0) {
			NPC.velocity.Y = .5f * Math.Clamp(MathHelper.Lerp(0, 1, NPC.ai[0] / 30f), 0, 1f);
		}
		else {
			NPC.velocity.Y = -.5f * Math.Clamp(MathHelper.Lerp(0, 1, NPC.ai[0] / 30f), 0, 1f);
		}
	}
	public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
		return false;
	}
	public override void OnKill() {
		for (int i = 0; i < 15; i++) {
			Dust dust = Dust.NewDustDirect(NPC.Center, 0, 0, DustID.Smoke);
			dust.velocity = Main.rand.NextVector2Circular(5, 5);
			dust.noGravity = true;
			dust.scale += Main.rand.NextFloat(.25f, .5f);
		}
		if (NPC.lastInteraction < 0 || NPC.lastInteraction >= 255) {
			return;
		}
		Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<EverlastingOverhaul_CookedShrimp_Projectile>(), 0, 0, NPC.lastInteraction);
	}
}
public class EverlastingOverhaul_CookedShrimp_Projectile : ModProjectile {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Shrimp);
	public override void SetDefaults() {
		Projectile.width = Projectile.height = 32;
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 720;
	}
	public override void OnSpawn(IEntitySource source) {
		Projectile.ai[0] = 60;
		if (Projectile.owner < 0 || Projectile.owner >= 255) {
			return;
		}
		Projectile.rotation = Main.rand.NextFloat();
		Player player = Main.player[Projectile.owner];
		Projectile.damage = (int)(player.GetWeaponDamage(player.HeldItem) * .2f) + 50;
	}
	public override void AI() {
		if (--Projectile.ai[0] >= 0) {
			return;
		}
		Projectile.rotation += MathHelper.ToRadians(30);
		if (Projectile.Center.LookForHostileNPC(out NPC npc, 900)) {
			Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10;
		}
	}
	public override void OnKill(int timeLeft) {
		Player player = Main.player[Projectile.owner];
		for (int i = 0; i < 25; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Smoke);
			dust.velocity = Main.rand.NextVector2Circular(5, 5);
			dust.noGravity = true;
			dust.scale += Main.rand.NextFloat(.25f, .5f);

			Dust torch = Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.Torch);
			torch.velocity = Main.rand.NextVector2Circular(5, 5);
			torch.noGravity = true;
			torch.scale += Main.rand.NextFloat(.25f, .5f);
		}
		Projectile.Center.LookForHostileNPC(out List<NPC> npclist, 250);
		foreach (NPC npc in npclist) {
			player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Projectile.damage, -1));
		}
	}
}
