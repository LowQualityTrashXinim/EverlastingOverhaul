using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_BloodyMoscato : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.BloodyMoscato;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3.5f);
	public override int LifeAmount() => 27;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_BloodyMoscato_Buff>(), ModUtils.ToMinute(5));
	}
}
public class Roguelike_BloodyMoscato_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.BloodyMoscato;
	public override void Update(Player player, ref int buffIndex) {
		PlayerStatsHandle handler = player.ModPlayerStats();
		handler.LifeSteal.Base += 2;
		handler.HealEffectiveness += .3f;
		handler.UpdateDefenseBase.Base += 2;
		handler.UpdateThorn.Base += 3;
	}
}
public class Roguelike_BloodyMoscato_ModPlayer : ModPlayer {
	public bool BloodyMoscato = false;
	public override void ResetEffects() {
		BloodyMoscato = false;
	}
	public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!BloodyMoscato) {
			return;
		}
		target.AddBuff(BuffID.Ichor, ModUtils.ToSecond(Main.rand.Next(1, 4)));
	}
}
