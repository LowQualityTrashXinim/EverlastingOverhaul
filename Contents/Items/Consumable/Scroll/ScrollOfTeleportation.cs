using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.Consumable.Scroll;
internal class ScrollOfTeleportation : ModItem {
	public override void SetDefaults() {
		Item.Item_DefaultToConsume(32, 32);
		Item.maxStack = 99;
	}
	public override bool? UseItem(Player player) {
		if (player.ItemAnimationJustStarted) {
			player.Teleport(Main.MouseWorld, TeleportationStyleID.TeleportationPotion);
		}
		return true;
	}
}
