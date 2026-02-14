using Terraria;
using Terraria.ID;

namespace EverlastingOverhaul.Contents.Items.Weapon.ItemVariant;
internal class VampireKnive_Var1 : ModVariant {
	public override void SetStaticDefaults() {
		ItemType = ItemID.VampireKnives;
	}
	public override void SetDefault(Item item) {
		item.damage = 9;
		item.useTime = item.useAnimation = 40;
		item.shootSpeed = 6;
	}
}
