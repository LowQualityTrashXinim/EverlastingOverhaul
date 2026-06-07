using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Bacon : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Bacon;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(3.5f);
	public override int LifeAmount() => 250;
	public override int EnergyAmount() => 550;
	public override byte Tier() => 2;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(1.75f);
		SetBuff(item, ModContent.BuffType<Roguelike_Bacon_ModBuff>(), ModUtils.ToMinute(39));
	}
}
public class Roguelike_Bacon_ModBuff : FoodItemTier3 {
	public override int TypeID => ItemID.Bacon;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_Bacon_ModPlayer>().Bacon = true;
	}
}
public class Roguelike_Bacon_ModPlayer : ModPlayer {
	public bool Bacon = false;
	public float Stack = 0;
	public override void ResetEffects() {
		Bacon = false;
	}
	public override void UpdateEquips() {
		if (Bacon) {
			Stack += Player.velocity.SafeNormalize(Vector2.Zero).Length();
			Stack = Math.Clamp(Stack, 0, 100);
			if (Stack >= 100) {
				PlayerStatsHandle handler = Player.ModPlayerStats();
				handler.AddStatsToPlayer(PlayerStats.PureDamage, Additive: 3);
				handler.DodgeChance += .15f;
				handler.UpdateThorn.Base += Player.velocity.Length() * .25f;
			}
			if (Player.itemAnimation == 1) {
				Stack = 0;
			}
		}
	}
}
