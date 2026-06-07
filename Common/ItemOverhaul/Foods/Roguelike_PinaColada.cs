using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_PinaColada : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.PinaColada;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(6.5f);
	public override int EnergyAmount() => 50;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_PinaColada_ModBuff>(), ModUtils.ToMinute(8));
	}
	public override string OverrideBasicTooltip() => ModUtils.LocalizationText("RoguelikeRework", "PinaColada");
}
public class Roguelike_PinaColada_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.PinaColada;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().UpdateFullHPDamage += .5f;
		player.ModPlayerStats().UpdateThorn += .11f;
		player.endurance += .09f;
	}
}
