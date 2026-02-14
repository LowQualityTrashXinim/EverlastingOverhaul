using Terraria;

namespace EverlastingOverhaul.Contents.Items.Weapon.ItemVariant;
internal class BreakerBlade_Var1 : ModVariant {
	public override void SetDefault(Item item) {
		item.damage = 40;
		item.knockBack = 10;
		item.useTime = item.useAnimation = 49;
	}
}
