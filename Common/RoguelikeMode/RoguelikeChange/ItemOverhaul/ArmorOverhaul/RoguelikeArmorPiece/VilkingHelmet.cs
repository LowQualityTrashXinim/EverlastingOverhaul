using EverlastingOverhaul.Common.Global;
using Roguelike.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorPiece;
public class VikingHelmet : ModArmorPiece {
	public override void SetDefault() {
		ArmorName = NoArmorSet;
		TypeEquipment = "VikingHelmet";
		PieceID = ItemID.VikingHelmet;
		AddTooltip = true;
		Add_Defense = 9;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.GetDamage<MeleeDamageClass>() += .15f;
		PlayerStatsHandle handle = player.ModPlayerStats();
		handle.Melee_CritDamage += .3f;
		handle.UpdateMovement += .2f;
		handle.UpdateJumpBoost += .3f;
	}
}
public class Goggles : ModArmorPiece {
	public override void SetDefault() {
		ArmorName = NoArmorSet;
		TypeEquipment = "Goggles";
		PieceID = ItemID.Goggles;
		Add_Defense = 1;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle handle = player.ModPlayerStats();
		handle.UpdateCritDamage += .5f;
		player.GetCritChance<GenericDamageClass>() += 5;
	}
}
