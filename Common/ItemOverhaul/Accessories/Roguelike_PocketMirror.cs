using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Accessories;
internal class EverlastingOverhaul_PocketMirror : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.type == ItemID.PocketMirror;
	public override void UpdateEquip(Item item, Player player) {
		player.GetModPlayer<EverlastingOverhaul_PocketMirror_ModPlayer>().PocketMirror = true;
	}
}
public class EverlastingOverhaul_PocketMirror_ModPlayer : ModPlayer {
	public bool PocketMirror = false;
	int Cooldown = 0;
	int ChargeTime = 0;
	public override void ResetEffects() {
		PocketMirror = false;
		cooldownProj = ModUtils.CountDown(cooldownProj);
		if (Cooldown <= 0) {
			if (++ChargeTime > 300) {
				ChargeTime = 300;
			}
		}
		else {
			Cooldown = ModUtils.CountDown(Cooldown);
			ChargeTime = 0;
		}
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Cooldown <= 0) {
			int damage = Player.GetWeaponDamage(Player.HeldItem) * 3 + 1;
			if (ChargeTime >= 300) {
				damage *= 10;
			}
			Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, ModUtils.DirectionFromEntityAToEntityB(Player.Center.X, npc.Center.X)));
			Cooldown = 180;
		}
	}
	int cooldownProj = 0;
	public override void UpdateEquips() {
		if (PocketMirror && cooldownProj <= 0) {
			foreach (var proj in Main.ActiveProjectiles) {
				if (proj.hostile && proj.Center.IsCloseToPosition(Player.Center, 100)) {
					cooldownProj = 60;
					if (Main.rand.NextBool(10)) {
						proj.velocity = -proj.velocity;
					}
				}
			}
		}
	}
}
