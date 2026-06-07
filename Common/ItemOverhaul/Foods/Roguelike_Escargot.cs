using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Escargot : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Escargot;
	public override int LifeAmount() => 140;
	public override byte Tier() => 1;
	public override int CoolDownBetweenUse() => 420;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = 180;
		SetBuff(item, ModContent.BuffType<Roguelike_Escargot_Buff>(), ModUtils.ToMinute(21));
	}
}
public class Roguelike_Escargot_Buff : FoodItemTier2 {
	public override int TypeID => ItemID.Escargot;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.UpdateDefenseBase.Base += 20;
		handler.UpdateMovement -= .4f;
		handler.UpdateJumpBoost -= .9f;
		handler.AttackSpeed -= .6f;
		player.endurance += .2f;
	}
}
