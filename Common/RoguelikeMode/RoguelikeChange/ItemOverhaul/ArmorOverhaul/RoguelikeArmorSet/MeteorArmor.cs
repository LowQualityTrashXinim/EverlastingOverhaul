using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class MeteorArmor : ModArmorSet {
	public override void SetDefault() {
		headID = ItemID.MeteorHelmet;
		bodyID = ItemID.MeteorSuit;
		legID = ItemID.MeteorLeggings;
	}
}
public class MeteorHelmet : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.MeteorHelmet;
		Add_Defense = 2;
		ArmorName = "MeteorArmor";
		TypeEquipment = Type_Head;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		modplayer.Magic_CritDamage += .15f;
		player.GetCritChance<MagicDamageClass>() += 5;
	}
}
public class MeteorSuit : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.MeteorSuit;
		Add_Defense = 3;
		ArmorName = "MeteorArmor";
		TypeEquipment = Type_Body;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		player.endurance += .15f;
	}
}
public class MeteorLeggings : ModArmorPiece {
	public override void SetDefault() {
		PieceID = ItemID.MeteorLeggings;
		Add_Defense = 2;
		ArmorName = "MeteorArmor";
		TypeEquipment = Type_Leg;
		AddTooltip = true;
	}
	public override void UpdateEquip(Player player, Item item) {
		PlayerStatsHandle modplayer = player.ModPlayerStats();
		player.rocketBoots = 1;
		player.vanityRocketBoots = 1;
		modplayer.UpdateJumpBoost += .5f;
	}
}
public class MeteorArmorPlayer : PlayerArmorHandle {
	public const int DefenseHealth = 200;
	public int MagicDefense = DefenseHealth;
	public int MagicRegenCounter = 0;
	public override void SetStaticDefaults() {
		ArmorLoader.SetModPlayer("MeteorArmor", this);
	}
	public override void Armor_ResetEffects() {
		if (++MagicRegenCounter >= 12) {
			MagicDefense = Math.Clamp(MagicDefense + 1, 0, DefenseHealth);
			MagicRegenCounter = 0;
		}
	}
	public override void Armor_UpdateEquipsSet() {
		Player.AddBuff<SpaceSuitDefenses>(2);
	}
	public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
		if (item.type == ItemID.LaserRifle) {
			mult *= 0;
		}
	}
	public override void Armor_ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers) {
		int damage = modifiers.ToHurtInfo(npc.damage, 0, 0, 0, false).Damage;
		MagicDefense -= damage;
		if (MagicDefense < 0) {
			MagicDefense = 0;
			MagicRegenCounter = -300;
			return;
		}
		modifiers.SourceDamage -= .8f;
	}
	public override void Armor_ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers) {
		int damage = modifiers.ToHurtInfo(proj.damage, 0, 0, 0, false).Damage;
		MagicDefense -= damage;
		if (MagicDefense < 0) {
			MagicDefense = 0;
			MagicRegenCounter = -300;
			return;
		}
		modifiers.SourceDamage -= .8f;
	}
}
public class SpaceSuitDefenses : ModBuff {
	public override string Texture => ModTexture.EMPTYBUFF;
	public override void SetStaticDefaults() {
		Main.debuff[Type] = false;
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}
	public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare) {
		string hexColor;
		Player player = Main.LocalPlayer;
		MeteorArmorPlayer shieldplayer = player.GetModPlayer<MeteorArmorPlayer>();
		int healthPoint = shieldplayer.MagicDefense;
		if (healthPoint >= MeteorArmorPlayer.DefenseHealth * .55f) {
			hexColor = Color.LawnGreen.Hex3();
		}
		else if (healthPoint >= MeteorArmorPlayer.DefenseHealth * .2f) {
			hexColor = Color.Yellow.Hex3();
		}
		else {
			hexColor = Color.Red.Hex3();
		}
		tip = string.Format(tip, [hexColor, healthPoint.ToString()]);
	}
}
