using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using EverlastingOverhaul.Common.Utils;

namespace EverlastingOverhaul.Common.ItemOverhaul.Foods;
internal class Roguelike_Apple : GlobalFoodItem {
	public override int AppliesToFoodType() => ItemID.Apple;
	public override int CoolDownBetweenUse() => ModUtils.ToSecond(2);
	public override int LifeAmount() => 35;
	public override void SetFoodDefaults(Item item) {
		item.useTime = item.useAnimation = ModUtils.ToSecond(.5f);
		SetBuff(item, ModContent.BuffType<Roguelike_Apple_Buff>(), ModUtils.ToMinute(3));
	}
}
public class Roguelike_Apple_Buff : FoodItemTier1 {
	public override int TypeID => ItemID.Apple;
	public override void Update(Player player, ref int buffIndex) {
		player.GetModPlayer<Roguelike_Apple_ModPlayer>().AppleEffect = true;
		if (player.statLife >= player.statLifeMax2 * .75f) {
			player.GetDamage(DamageClass.Generic) += .2f;
		}
	}
}
public class Roguelike_Apple_ModPlayer : ModPlayer {
	public bool AppleEffect = false;
	public override void ResetEffects() {
		AppleEffect = false;
	}
	public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price) {
		if (AppleEffect) {
			price = (int)Math.Clamp(price * .65f, 0, int.MaxValue);
		}
	}
}
public class Roguelike_Apple_GlobalNPC : GlobalNPC {
	public override void ModifyActiveShop(NPC npc, string shopName, Item[] items) {
		bool apple = Main.LocalPlayer.GetModPlayer<Roguelike_Apple_ModPlayer>().AppleEffect;
		if (npc.type == NPCID.WitchDoctor && apple) {
			foreach (var item in items) {
				if (item == null) {
					continue;
				}
				if (item.IsAir) {
					continue;
				}
				if (item.isAShopItem) {
					item.value = (int)(item.value * .65f);
				}
			}
		}
	}
}
