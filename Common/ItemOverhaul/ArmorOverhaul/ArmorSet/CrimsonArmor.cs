using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.ItemOverhaul.ArmorOverhaul;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;

namespace EverlastingOverhaul.Common.ItemOverhaul.ArmorOverhaul.ArmorSet;
internal class CrimsonArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.CrimsonHelmet;
		bodyID = ItemID.CrimsonScalemail;
		legID = ItemID.CrimsonGreaves;
	}
}
public class CrimsonHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.CrimsonHelmet;
		Add_Defense = 4;
		TypeEquipment = Type_Head;
		ArmorName = "CrimsonArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.ModPlayerStats().UpdateHPRegen.Base += 1;
	}
}
public class CrimsonScalemail : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.CrimsonScalemail;
		Add_Defense = 5;
		TypeEquipment = Type_Body;
		ArmorName = "CrimsonArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.ModPlayerStats().UpdateHPRegen.Base += 1;
	}
}
public class CrimsonGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.CrimsonGreaves;
		Add_Defense = 3;
		TypeEquipment = Type_Leg;
		ArmorName = "CrimsonArmor";
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.ModPlayerStats().UpdateHPRegen.Base += 1;
	}
}
public class CrimsonArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("CrimsonArmor", this);
	}
	public override void Armor_UpdateEquipsSet() {
		PlayerStatsHandle handler = Player.ModPlayerStats();
		handler.UpdateMovement += .25f;
		handler.UpdateJumpBoost += .3f;
		handler.UpdateHPRegen += .25f;
		handler.Rapid_LifeRegen += .1f;
	}
}
