using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_ChickenNugget : GlobalFoodItem{
	public override int AppliesToFoodType() => ItemID.ChickenNugget;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(5);
	public override int LifeAmount() => 80;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_ChickenNugget_ModBuff>(), ModUtils.ToMinute(23));
	}
}
public class Roguelike_ChickenNugget_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.ChickenNugget;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		//handler.EnergyRecharge *= .35f;
		//handler.SkillDuration *= .45f;
		//player.GetModPlayer<SkillHandlePlayer>().skilldamage *= 1.25f;
	}
}
