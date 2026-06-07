using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_BowlofSoup : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BowlofSoup;
	public override int LifeAmount() => 240;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(10);
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_BowlofSoup_ModBuff>(), ModUtils.ToMinute(15));
	}
}
public class Roguelike_BowlofSoup_ModBuff : FoodItemTier2 {
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
public class Roguelike_BowlofSoup_ModPlayer : ModPlayer {
	public override void ResetEffects() {
		PlayerStatsHandle.SetSecondLifeCondition(Player, "RW_BoS", Player.HasBuff(ModContent.BuffType<Roguelike_BowlofSoup_ModBuff>()));
	}
	public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource) {
		if (PlayerStatsHandle.GetSecondLife(Player, "RW_BoS")) {
			Player.ClearBuff(ModContent.BuffType<Roguelike_BowlofSoup_ModBuff>());
			Player.Heal(Player.statLifeMax2);
			Player.immune = true;
			Player.AddImmuneTime(-1, 90);
			return false;
		}
		return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
	}
}
