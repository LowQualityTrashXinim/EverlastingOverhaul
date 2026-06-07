using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_RoastedBird : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.RoastedBird;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(6.25f);
	public override int LifeAmount() => 40;
	public override int ManaAmount() => 90;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(3);
		SetBuff(item, ModContent.BuffType<Roguelike_RoastedBird_ModBuff>(), ModUtils.ToMinute(7));
	}
}

public class Roguelike_RoastedBird_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.RoastedBird;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.WingUpTime += .35f;
		if (player.wingTime != player.wingTimeMax && player.wingTime != 0) {
			handler.UpdateFullHPDamage *= 1.23f;
			handler.DodgeChance += .05f;
		}
	}
}
