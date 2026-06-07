using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using System.Collections.Generic;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Lemon : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Lemon;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4);
	public override int EnergyAmount() => 92;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3);
		SetBuff(item, ModContent.BuffType<Roguelike_Lemon_Buff>(), ModUtils.ToMinute(4));
	}
}
public class Roguelike_Lemon_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Lemon;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().DebuffTime += .25f;
		player.GetKnockback(DamageClass.Generic).Base += .5f;
	}
}
public class Roguelike_Lemon_ModPlayer : ModPlayer {
	public bool Lemon = false;
	public int CooldownExplosion = 0;
	public override void ResetEffects() {
		Lemon = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (Lemon) {
			CooldownExplosion = 60;
			target.Center.LookForHostileNPC(out List<NPC> npclist, 150);
			foreach (NPC npc in npclist) {
				if (npc.whoAmI == target.whoAmI) {
					continue;
				}
				Player.StrikeNPCDirect(npc, hit with { Damage = (int)(hit.Damage * .45f) });
			}
		}
	}
}
