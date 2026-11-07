using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Systems.ReviveSystem;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Foods;
internal class EverlastingOverhaul_BowlofSoup : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BowlofSoup;
	public override int LifeAmount() => 240;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(10);
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3.75f);
		SetBuff(item, ModContent.BuffType<EverlastingOverhaul_BowlofSoup_ModBuff>(), ModUtils.ToMinute(15));
	}
}
public class EverlastingOverhaul_BowlofSoup_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.BowlofSoup;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateMovement += .4f;
		handler.UpdateDefenseBase.Base += 4;
		handler.MeleeAtkSpeed += .1f;
		player.GetDamage(DamageClass.Generic) += .1f;
		player.GetCritChance(DamageClass.Generic) += 10;
		player.pickSpeed += .15f;
	}
}
public class EverlastingOverhaul_BowlofSoup_Revive : Revive {
	public override bool IsActive(Player player) {
		return player.HasBuff(ModContent.BuffType<EverlastingOverhaul_BowlofSoup_ModBuff>());
	}
	public override void OnRevive(Player player) {
		player.ClearBuff(ModContent.BuffType<EverlastingOverhaul_BowlofSoup_ModBuff>());
		player.Heal(player.statLifeMax2);
		player.immune = true;
		player.AddImmuneTime(-1, 90);
	}
}
