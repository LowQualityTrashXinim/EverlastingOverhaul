using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_FrozenBananaDaiquiri : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BananaDaiquiri;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(4);
	public override int EnergyAmount() => 90;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = 60;
		SetBuff(item, ModContent.BuffType<Roguelike_FrozenBananaDaiquiri_ModBuff>(), ModUtils.ToMinute(4));
	}
}
public class Roguelike_FrozenBananaDaiquiri_ModBuff : FoodItemTier1 {
	public override int TypeID => ItemID.BananaDaiquiri;
	public override void Update(Player player, ref int buffIndex) {
		player.ModPlayerStats().UpdateCritDamage += .1f;
		player.GetCritChance(DamageClass.Generic) += 3;
		player.GetDamage(DamageClass.Generic) += .07f;
		player.buffImmune[BuffID.OnFire] = true;
		player.buffImmune[BuffID.OnFire3] = true;
		player.GetModPlayer<Roguelike_FrozenBananaDaiquiri_ModPlayer>().BananaDaiquiri = true;
	}
}
public class Roguelike_FrozenBananaDaiquiri_ModPlayer : ModPlayer {
	public bool BananaDaiquiri = false;
	public override void ResetEffects() {
		BananaDaiquiri = false;
	}
	public override bool CanConsumeAmmo(Item weapon, Item ammo) {
		if (BananaDaiquiri) {
			return Main.rand.NextFloat() <= .07f;
		}
		return base.CanConsumeAmmo(weapon, ammo);
	}
}
