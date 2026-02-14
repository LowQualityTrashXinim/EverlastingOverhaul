using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Contents.Items.NoneSynergy
{
	class NatureCrystal : ModItem {
		public override void SetDefaults() {
			Item.DefaultToAccessory(28, 30);
			Item.value = 1000000;
		}
		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.statLifeMax2 += 40;
			player.statManaMax2 += 40;
		}
	}
}
