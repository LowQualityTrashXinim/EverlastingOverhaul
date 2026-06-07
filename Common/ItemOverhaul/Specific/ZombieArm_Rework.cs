using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Specific;
internal class ZombieArm_Rework : GlobalItem {
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.ZombieArm;
	}
	public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
		ModUtils.AddTooltip(ref tooltips, new(Mod, "", ModUtils.LocalizationText("RoguelikeRework", item.Name)));
	}
	public override void SetDefaults(Item entity) {
		entity.damage = 33;
	}
	public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone) {
		player.Heal(Main.rand.Next(1, 4));
		target.AddBuff<Rotting>(ModUtils.ToSecond(3));
	}
}
public class ZombieArm_Rework_ModPlayer : ModPlayer {
	public override void ResetEffects() {
		PlayerStatsHandle.SetSecondLifeCondition(Player, "ZombieArm", Player.HeldItem.type == ItemID.ZombieArm);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (Player.HeldItem.type == ItemID.ZombieArm) {
			Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(Player.GetWeaponDamage(Player.HeldItem), 1));
		}
	}
	public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
		if(PlayerStatsHandle.GetSecondLife(Player, "ZombieArm")) {
			Player.Heal(Player.statLifeMax2);
			Player.AddImmuneTime(-1, 120);
			Player.immune = true;
			if(Player.HeldItem.type == ItemID.ZombieArm) {
				Player.HeldItem.TurnToAir();
			}
			return false;
		}
		return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
	}
}
class Rotting : ModBuff
{
    public override string Texture => ModTexture.EMPTYBUFF;
    public override void SetStaticDefaults()
    {
        this.BossRushSetDefaultDeBuff();
    }
    public override void Update(NPC npc, ref int buffIndex)
    {
        npc.lifeRegen -= 20;
        npc.GetGlobalNPC<RoguelikeGlobalNPC>().StatDefense.Base -= 5;
    }
}
