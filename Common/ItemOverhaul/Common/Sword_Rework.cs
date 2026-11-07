using EverlastingOverhaul.Common ;
using EverlastingOverhaul.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common .ItemOverhaul.Common;
internal class Sword_Rework : GlobalItem {
	public override void SetDefaults(Item entity) {
		SwordWeaponOverhaul(entity);
	}
	public void SwordWeaponOverhaul(Item item) {
		if (item.noMelee || item.noUseGraphic) {
			return;
		}
		var global = item.GetGlobalItem<MeleeWeaponOverhaul>();
		switch (item.type) {
			//Sword that have even end
			//WoodSword
			case ItemID.PearlwoodSword:
			case ItemID.BorealWoodSword:
			case ItemID.PalmWoodSword:
			case ItemID.ShadewoodSword:
			case ItemID.EbonwoodSword:
			case ItemID.RichMahoganySword:
			case ItemID.WoodenSword:
			case ItemID.AshWoodSword:
			case ItemID.CactusSword:
			//OrebroadSword
			case ItemID.BeeKeeper:
			case ItemID.CopperBroadsword:
			case ItemID.TinBroadsword:
			case ItemID.IronBroadsword:
			case ItemID.LeadBroadsword:
			case ItemID.SilverBroadsword:
			case ItemID.TungstenBroadsword:
			case ItemID.GoldBroadsword:
			case ItemID.Flymeal:
			case ItemID.PlatinumBroadsword:
			//Misc PreHM sword
			case ItemID.PurpleClubberfish:
			case ItemID.BladeofGrass:
			case ItemID.FieryGreatsword:
			case ItemID.LightsBane:
			//HardmodeSword
			case ItemID.MythrilSword:
			case ItemID.AdamantiteSword:
			case ItemID.OrichalcumSword:
			case ItemID.TitaniumSword:
			case ItemID.Excalibur:
			case ItemID.TheHorsemansBlade:
			case ItemID.Bladetongue:
			case ItemID.DD2SquireDemonSword:
			//Sword That shoot projectile
			case ItemID.BeamSword:
			case ItemID.EnchantedSword:
			case ItemID.Starfury:
			case ItemID.InfluxWaver:
			case ItemID.ChlorophyteClaymore:
			case ItemID.ChlorophyteSaber:
			case ItemID.ChristmasTreeSword:
				global.SwingType = BossRushUseStyle.Swipe;
				item.useTurn = false;
				item.Set_ItemCriticalDamage(1f);
				break;
			//Poke Sword
			//Pre HM Sword
			case ItemID.DyeTradersScimitar:
			case ItemID.CandyCaneSword:
			case ItemID.Muramasa:
			case ItemID.BloodButcherer:
			case ItemID.Katana:
			case ItemID.FalconBlade:
			case ItemID.BoneSword:
			//HM sword
			case ItemID.CobaltSword:
			case ItemID.PalladiumSword:
			case ItemID.IceBlade:
			case ItemID.BreakerBlade:
			case ItemID.Frostbrand:
			case ItemID.Cutlass:
			case ItemID.Seedler:
			case ItemID.Keybrand:
			case ItemID.AntlionClaw:
			case ItemID.StarWrath:
			case ItemID.Meowmere:
				global.SwingType = BossRushUseStyle.Swipe;
				global.UseSwipeTwo = true;
				item.useTurn = false;
				item.Set_ItemCriticalDamage(1f);
				break;
			case ItemID.ZombieArm:
			case ItemID.BatBat:
			case ItemID.TentacleSpike:
			case ItemID.SlapHand:
			case ItemID.HamBat:
			case ItemID.PsychoKnife:
				global.SwingType = BossRushUseStyle.SwipeDown;
				item.useTurn = false;
				item.Set_ItemCriticalDamage(1f);
				break;
			case ItemID.DD2SquireBetsySword:
				global.OffSetFromHand = 20;
				global.SwingType = BossRushUseStyle.SwipeDown;
				item.useTurn = false;
				item.Set_ItemCriticalDamage(1f);
				break;
			case ItemID.PurplePhaseblade:
			case ItemID.BluePhaseblade:
			case ItemID.GreenPhaseblade:
			case ItemID.YellowPhaseblade:
			case ItemID.OrangePhaseblade:
			case ItemID.RedPhaseblade:
			case ItemID.WhitePhaseblade:
			case ItemID.PurplePhasesaber:
			case ItemID.BluePhasesaber:
			case ItemID.GreenPhasesaber:
			case ItemID.YellowPhasesaber:
			case ItemID.OrangePhasesaber:
			case ItemID.RedPhasesaber:
			case ItemID.WhitePhasesaber:
				global.SwingType = BossRushUseStyle.Swipe;
				item.useTurn = false;
				item.Set_ItemCriticalDamage(1f);
				global.IframeDivision = 3;
				global.ShaderOffSetLength = 5;
				break;
			default:
				break;
		}
	}
}
