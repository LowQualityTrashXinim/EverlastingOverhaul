using Terraria;
using Terraria.ID;

namespace EverlastingOverhaul.Contents.Items.Weapon.ItemVariant;
public class SlimeStaff_Var1 : ModVariant {
	public override void SetStaticDefaults() {
		ItemType = ItemID.SlimeStaff;
	}
	public override void SetDefault(Item item) {
		item.damage = 22;
		item.mana = 0;
	}
}
