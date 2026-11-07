using EverlastingOverhaul.Common.ItemOverhaul.ArmorOverhaul;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;

namespace EverlastingOverhaul.Common.ItemOverhaul.ArmorOverhaul.ArmorSet;
internal class BorealwoodArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.BorealWoodHelmet;
		bodyID = ItemID.BorealWoodBreastplate;
		legID = ItemID.BorealWoodGreaves;
	}
}
class BorealWoodHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodHelmet;
		Add_Defense = 13;
		ArmorName = "BorealwoodArmor";
		AddTooltip = true;
		TypeEquipment = Type_Head;
	}
	public override void UpdateEquip(Player player, Item item) {
		var EverlastingOverhaul = player.GetModPlayer<EverlastingOverhaulArmorPlayer>();
		EverlastingOverhaul.FrostBurnChance += .02f;
		EverlastingOverhaul.SnowBallDamage = true;
		EverlastingOverhaul.SnowSpawnChance += .03f;
	}
}
class BorealWoodBreastplate : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodBreastplate;
		Add_Defense = 14;
		ArmorName = "BorealwoodArmor";
		AddTooltip = true;
		TypeEquipment = Type_Body;
	}
	public override void UpdateEquip(Player player, Item item) {
		var EverlastingOverhaul = player.GetModPlayer<EverlastingOverhaulArmorPlayer>();
		EverlastingOverhaul.FrostBurnChance += .03f;
		EverlastingOverhaul.ReplaceSnowBallWithSnow = true;
		EverlastingOverhaul.SnowSpawnChance += .04f;
	}
}
class BorealWoodGreaves : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.BorealWoodGreaves;
		Add_Defense = 13;
		ArmorName = "BorealwoodArmor";
		AddTooltip = true;
		TypeEquipment = Type_Leg;
	}
	public override void UpdateEquip(Player player, Item item) {
		var EverlastingOverhaul = player.GetModPlayer<EverlastingOverhaulArmorPlayer>();
		EverlastingOverhaul.FrostBurnChance += .01f;
		EverlastingOverhaul.RunningCauseSnowToShoot = true;
		EverlastingOverhaul.SnowSpawnChance += .02f;
	}
}
class BorealWoodArmorPlayer : PlayerArmorHandle {
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("BorealwoodArmor", this);
	}
	int CD_Heal = 0;
	public override void Armor_UpdateEquipsSet() {
		Player.statDefense += 5;
		Player.moveSpeed += .20f;
		Player.buffImmune[BuffID.Chilled] = true;
		Player.buffImmune[BuffID.Slow] = true;
		var rougelike = Player.GetModPlayer<EverlastingOverhaulArmorPlayer>();
		rougelike.FrostBurnChance += .07f;
		rougelike.SnowSpawnChance += .1f;
		CD_Heal = ModUtils.CountDown(CD_Heal);
	}
	public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (target.HasBuff(BuffID.Frostburn) || target.HasBuff(BuffID.Frostburn2)) {
			if(CD_Heal <= 0) {
				Player.Heal(1);
				CD_Heal = 60;
			}
		}
	}
}
