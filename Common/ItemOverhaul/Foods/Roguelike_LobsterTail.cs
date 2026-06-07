using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_LobsterTail : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.LobsterTail;
	public override int LifeAmount() => 120;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(8);
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(5.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_LobsterTail_ModBuff>(), ModUtils.ToMinute(23));
	}
}
public class Roguelike_LobsterTail_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.LobsterTail;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_LobsterTail_ModPlayer>().LobsterTail = true;
	}
}
public class Roguelike_LobsterTail_ModPlayer : ModPlayer {
	public bool LobsterTail = false;
	public int CoolDown = 0;
	public override void ResetEffects() {
		LobsterTail = false;
		CoolDown = ModUtils.CountDown(CoolDown);
	}
	public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
		if (LobsterTail) {
			if (CoolDown > 0) {
				return;
			}
			int damage;
			if (Player.wet || Player.HasBuff(BuffID.Wet)) {
				damage = hurtInfo.Damage * 15;
			}
			else {
				damage = hurtInfo.Damage * 5;
			}
			Player.StrikeNPCDirect(npc, npc.CalculateHitInfo(damage, hurtInfo.HitDirection * -1));
			CoolDown = ModUtils.ToSecond(2);
		}
	}
}
