using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_CookedShrimp : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.CookedShrimp;
	public override int CoolDownBetweenUse() => 240;
	public override int LifeAmount() => 60;
	public override int ManaAmount() => 150;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(2.25f);
		SetBuff(item, ModContent.BuffType<Roguelike_CookedShrimp_Buff>(), ModUtils.ToMinute(18));
	}
}
public class Roguelike_CookedShrimp_Buff : FoodItemTier2 {
	public override int TypeID => ItemID.CookedShrimp;
	public override void Update(Player player, ref int buffIndex) {
		base.Update(player, ref buffIndex);
	}
}
public class Roguelike_CookedShrimp_ModPlayer : ModPlayer {

}
public class Roguelike_CookedShrimp_NPC : ModNPC {
	public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.Shrimp);

}
