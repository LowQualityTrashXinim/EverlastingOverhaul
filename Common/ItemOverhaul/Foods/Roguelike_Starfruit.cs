using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Starfruit : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Starfruit;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(9);
	public override int ManaAmount() => 200;
	public override byte Tier() => 1;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3);
		SetBuff(item, ModContent.BuffType<Roguelike_Starfruit_ModBuff>(), ModUtils.ToMinute(23));
	}
}
public class Roguelike_Starfruit_ModBuff : FoodItemTier2 {
	public override int TypeID => ItemID.Starfruit;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.Iframe.Base += ModUtils.ToSecond(.33f);
		handler.UpdateManaRegen += .5f;
		player.GetDamage(DamageClass.Magic) += .15f;
		player.manaCost -= .2f;
	}
}
