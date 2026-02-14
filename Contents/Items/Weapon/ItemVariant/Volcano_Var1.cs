using Terraria;
using Terraria.ID;

namespace EverlastingOverhaul.Contents.Items.Weapon.ItemVariant;
internal class Volcano_Var1 : ModVariant {
	public override void SetStaticDefaults() {
		ItemType = ItemID.FieryGreatsword;
	}
	public override void SetDefault(Item item) {
		item.damage = 40;
		item.useTime = item.useAnimation = 44;
		item.knockBack = 10;
	}
}
