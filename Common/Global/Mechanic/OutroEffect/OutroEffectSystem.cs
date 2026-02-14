using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Items.NoneSynergy;
using EverlastingOverhaul.Contents.Items.NoneSynergy.EnhancedKatana;
using EverlastingOverhaul.Contents.Items.NoneSynergy.FrozenEnchantedSword;
using EverlastingOverhaul.Contents.Items.NoneSynergy.FrozenShark;
using EverlastingOverhaul.Contents.Items.NoneSynergy.GenericBlackSword;
using EverlastingOverhaul.Contents.Items.NoneSynergy.Gunmerang;
using EverlastingOverhaul.Contents.Items.NoneSynergy.HuntingRifle;
using EverlastingOverhaul.Contents.Items.NoneSynergy.LongerMusket;
using EverlastingOverhaul.Contents.Items.NoneSynergy.ManaStarFury;
using EverlastingOverhaul.Contents.Items.NoneSynergy.OvergrownMinishark;
using EverlastingOverhaul.Contents.Items.NoneSynergy.RectangleShotgun;
using EverlastingOverhaul.Contents.Items.NoneSynergy.RifleShotgun;
using EverlastingOverhaul.Contents.Items.NoneSynergy.SharpBoomerang;
using EverlastingOverhaul.Contents.Items.NoneSynergy.SingleBarrelMinishark;
using EverlastingOverhaul.Contents.Items.NoneSynergy.SnowballRifle;
using EverlastingOverhaul.Contents.Items.NoneSynergy.TrueEnchantedSword;
using EverlastingOverhaul.Contents.Items.Weapon.ArcaneRange.MoonStarBow;
using EverlastingOverhaul.Contents.Items.Weapon.ArcaneRange.TheBurningSky;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.AmberBoneSpear;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.LaserSniper;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.MagicBow;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.MagicHandCannon;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.SinisterBook;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.StarLightDistributer;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.Swotaff;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.WyvernWrath;
using EverlastingOverhaul.Contents.Items.Weapon.MagicSynergyWeapon.ZapSnapper;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.AkaiHanbunNoHasami;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.BloodyStella;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.BurningPassion;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.DarkCactus;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.DrainingVeilBlade;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.EnchantedOreSword;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.EnchantedStarFury;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.EnergyBlade;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.EverlastingCold;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.FlamingWoodSword;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.FrostSwordFish;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.MasterSword;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.MythrilBeamSword;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.RelentlessAbomination;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.SakuraKatana;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.ShadowTrick;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.ShatteredSky;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.SuperShortSword;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.TheOrbit;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.TwilightNight;
using EverlastingOverhaul.Contents.Items.Weapon.MeleeSynergyWeapon.YinYang;
using EverlastingOverhaul.Contents.Items.Weapon.PureSynergyWeapon.Resolve;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Annihiliation;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.BloodyShot;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Bowmarang;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.ChaosMiniShark;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.CorruptedRebirth;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Deagle;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.DeathBySpark;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.ForceOfEarth;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.HandmadeLauncher;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.HeartPistol;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.HeavenSmg;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.HorusEye;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.IceStorm;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.KnifeRevolver;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Merciless;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Mixmaster;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.NatureSelection;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.PaintRifle;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.ParadoxPistol;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.PulseRifle;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.QuadDemonBlaster;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.SkullRevolver;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.SnowballCannonMK2;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.SuperFlareGun;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.TheTwoEvil;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.TundraBow;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Underdog;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Unforgiving;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.WinterFlame;
using EverlastingOverhaul.Contents.Items.Weapon.SummonerSynergyWeapon.MothWeapon;
using EverlastingOverhaul.Contents.Items.Weapon.SummonerSynergyWeapon.StarWhip;
using EverlastingOverhaul.Contents.Items.Weapon.SummonerSynergyWeapon.StickySlime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.WorldBuilding;

namespace EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
internal class OutroEffectSystem : ModSystem {
	public static List<WeaponEffect> list_effect { get; private set; } = new();
	public static WeaponEffect GetWeaponEffect(int type) => type >= list_effect.Count || type < 0 ? null : list_effect[type];
	private static HashSet<int>[] Arr_WeaponTag = [];
	/// <summary>
	/// It is highly recommanded to use this if you already know what tag you want to check to see if it have the weapon
	/// </summary>
	public static HashSet<int>[] Get_Arr_WeaponTag => Arr_WeaponTag;
	private static Dictionary<int, HashSet<WeaponTag>> WeaponType_WeaponTag = new();
	public static Dictionary<int, HashSet<WeaponTag>> Get_WeaponType_WeaponTag => WeaponType_WeaponTag;
	public static short Register(WeaponEffect effect) {
		ModTypeLookup<WeaponEffect>.Register(effect);
		effect.SetStaticDefaults();
		list_effect.Add(effect);
		return (short)(list_effect.Count - 1);
	}
	public override void PostSetupContent() {
		var watch = new Stopwatch();
		watch.Start();
		int len = (int)Enum.GetValues(typeof(WeaponTag)).Cast<WeaponTag>().Last() + 1;
		Array.Resize(ref Arr_WeaponTag, len);

		for (int i = 0; i < len; i++) {
			Arr_WeaponTag[i] = new();
		}

		Add_SwordTag();
		Add_ShortSwordTag();
		Add_GreatSwordTag();
		Add_BluntTag();
		Add_SickleTag();
		Add_SpearTag();
		Add_FlailTag();
		Add_YoyoTag();
		Add_BoomerangTag();
		Add_BowTag();
		Add_RepeaterTag();
		Add_GunTag();
		Add_PistolTag();
		Add_RifleTag();
		Add_ShotgunTag();
		Add_LauncherTag();
		Add_MagicStaffTag();
		Add_MagicWandTag();
		Add_MagicBookTag();
		Add_MagicGunTag();
		Add_SummonStaffTag();
		Add_SummonMiscTag();
		Add_WhipTag();
		Add_OtherTag();
		Add_ReaperMarkTag();
		Add_HallowedGazeTag();
		Add_WrathOfBlueMoonTag();
		Add_FuryOfTheSunTag();
		Add_EletricConductor();
		Add_Avarice();

		watch.Stop();
		Mod.Logger.Info("Time taken to initialize tag: " + watch.ToString());
	}
	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight) {
		WeaponType_WeaponTag.Clear();
	}
	/// <summary>
	/// Check whenever a weapon tag is even present at all for the weapon<br/>
	/// This is for optimizing so don't bother using this at all<br/>
	/// Instead use <see cref="Get_Arr_WeaponTag"/>
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static bool Has_WeaponTag(int type) => WeaponType_WeaponTag.ContainsKey(type);
	/// <summary>
	/// Do a reverse lookup for the item and then cached the item tag in a hash
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public string GetWeaponTag(int type) {
		string tag = "";
		if (WeaponType_WeaponTag.ContainsKey(type)) {
			foreach (var weaponTag in WeaponType_WeaponTag[type]) {
				tag += $"[{weaponTag.ToString()}] ";
			}
			return tag;
		}
		for (int i = 0; i < Arr_WeaponTag.Length; i++) {
			if (Arr_WeaponTag[i].Contains(type)) {
				tag += $"[{(WeaponTag)i}] ";
				if (WeaponType_WeaponTag.ContainsKey(type)) {
					WeaponType_WeaponTag[type].Add((WeaponTag)i);
				}
				else {
					WeaponType_WeaponTag.Add(type, new() { (WeaponTag)i });
				}
			}
		}
		return tag;
	}
	//All of this mustn't be confused with the actual Add_tag function since these below are more like initialize function
	private void Add_SwordTag() {
		int tag = (int)WeaponTag.Sword;
		Arr_WeaponTag[tag].Add(ItemID.WoodenSword);
		Arr_WeaponTag[tag].Add(ItemID.BorealWoodSword);
		Arr_WeaponTag[tag].Add(ItemID.RichMahoganySword);
		Arr_WeaponTag[tag].Add(ItemID.EbonwoodSword);
		Arr_WeaponTag[tag].Add(ItemID.ShadewoodSword);
		Arr_WeaponTag[tag].Add(ItemID.PearlwoodSword);
		Arr_WeaponTag[tag].Add(ItemID.CactusSword);

		Arr_WeaponTag[tag].Add(ItemID.CopperBroadsword);
		Arr_WeaponTag[tag].Add(ItemID.LeadBroadsword);
		Arr_WeaponTag[tag].Add(ItemID.TinBroadsword);
		Arr_WeaponTag[tag].Add(ItemID.IronBroadsword);
		Arr_WeaponTag[tag].Add(ItemID.TungstenBroadsword);
		Arr_WeaponTag[tag].Add(ItemID.SilverBroadsword);
		Arr_WeaponTag[tag].Add(ItemID.GoldBroadsword);
		Arr_WeaponTag[tag].Add(ItemID.PlatinumBroadsword);

		Arr_WeaponTag[tag].Add(ItemID.Katana);
		Arr_WeaponTag[tag].Add(ItemID.LightsBane);
		Arr_WeaponTag[tag].Add(ItemID.Muramasa);
		Arr_WeaponTag[tag].Add(ItemID.BoneSword);
		Arr_WeaponTag[tag].Add(ItemID.DyeTradersScimitar);
		Arr_WeaponTag[tag].Add(ItemID.Flymeal);
		Arr_WeaponTag[tag].Add(ItemID.BloodButcherer);
		Arr_WeaponTag[tag].Add(ItemID.IceBlade);
		Arr_WeaponTag[tag].Add(ItemID.EnchantedSword);
		Arr_WeaponTag[tag].Add(ItemID.NightsEdge);
		Arr_WeaponTag[tag].Add(ItemID.Starfury);
		Arr_WeaponTag[tag].Add(ItemID.FalconBlade);
		Arr_WeaponTag[tag].Add(ItemID.BeeKeeper);

		Arr_WeaponTag[tag].Add(ItemID.BluePhaseblade);
		Arr_WeaponTag[tag].Add(ItemID.RedPhaseblade);
		Arr_WeaponTag[tag].Add(ItemID.PurplePhaseblade);
		Arr_WeaponTag[tag].Add(ItemID.YellowPhaseblade);
		Arr_WeaponTag[tag].Add(ItemID.OrangePhaseblade);
		Arr_WeaponTag[tag].Add(ItemID.GreenPhaseblade);
		Arr_WeaponTag[tag].Add(ItemID.WhitePhaseblade);

		Arr_WeaponTag[tag].Add(ItemID.BluePhasesaber);
		Arr_WeaponTag[tag].Add(ItemID.RedPhasesaber);
		Arr_WeaponTag[tag].Add(ItemID.PurplePhasesaber);
		Arr_WeaponTag[tag].Add(ItemID.YellowPhasesaber);
		Arr_WeaponTag[tag].Add(ItemID.OrangePhasesaber);
		Arr_WeaponTag[tag].Add(ItemID.GreenPhasesaber);
		Arr_WeaponTag[tag].Add(ItemID.WhitePhasesaber);

		Arr_WeaponTag[tag].Add(ItemID.CobaltSword);
		Arr_WeaponTag[tag].Add(ItemID.PalladiumSword);
		Arr_WeaponTag[tag].Add(ItemID.MythrilSword);
		Arr_WeaponTag[tag].Add(ItemID.OrichalcumSword);
		Arr_WeaponTag[tag].Add(ItemID.Bladetongue);
		Arr_WeaponTag[tag].Add(ItemID.Cutlass);
		Arr_WeaponTag[tag].Add(ItemID.Frostbrand);
		Arr_WeaponTag[tag].Add(ItemID.Excalibur);
		Arr_WeaponTag[tag].Add(ItemID.ChlorophyteSaber);
		Arr_WeaponTag[tag].Add(ItemID.Seedler);
		Arr_WeaponTag[tag].Add(ItemID.Keybrand);
		Arr_WeaponTag[tag].Add(ItemID.BeamSword);
		Arr_WeaponTag[tag].Add(ItemID.TrueExcalibur);
		Arr_WeaponTag[tag].Add(ItemID.TrueNightsEdge);
		Arr_WeaponTag[tag].Add(ItemID.TerraBlade);
		Arr_WeaponTag[tag].Add(ItemID.InfluxWaver);

		Arr_WeaponTag[tag].Add(ItemID.Arkhalis);
		Arr_WeaponTag[tag].Add(ItemID.Terragrim);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<EnchantedCopperSword>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<ManaStarFury>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<GenericBlackSword>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<FrozenEnchantedSword>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Katahanced>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<OldFlamingWoodSword>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<AmethystSwotaff>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<TopazSwotaff>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SapphireSwotaff>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<EmeraldSwotaff>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<RubySwotaff>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<DiamondSwotaff>());

		Arr_WeaponTag[tag].Add(ModContent.ItemType<BloodyStella>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<EnchantedOreSword>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<EnchantedStarfury>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<FlamingWoodSword>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<MasterSword>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<MythrilBeamSword>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SakuraKatana>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<EnergyBlade>());
	}
	private void Add_ShortSwordTag() {
		int tag = (int)WeaponTag.Thrustsword;
		Arr_WeaponTag[tag].Add(ItemID.CopperShortsword);
		Arr_WeaponTag[tag].Add(ItemID.TinShortsword);
		Arr_WeaponTag[tag].Add(ItemID.IronShortsword);
		Arr_WeaponTag[tag].Add(ItemID.LeadShortsword);
		Arr_WeaponTag[tag].Add(ItemID.SilverShortsword);
		Arr_WeaponTag[tag].Add(ItemID.TungstenShortsword);
		Arr_WeaponTag[tag].Add(ItemID.GoldShortsword);
		Arr_WeaponTag[tag].Add(ItemID.PlatinumShortsword);
		Arr_WeaponTag[tag].Add(ItemID.Gladius);
		Arr_WeaponTag[tag].Add(ItemID.Ruler);
		Arr_WeaponTag[tag].Add(ItemID.PiercingStarlight);

		//Modded weapon
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SuperShortSword>());
	}
	private void Add_GreatSwordTag() {
		int tag = (int)WeaponTag.Greatsword;

		Arr_WeaponTag[tag].Add(ItemID.AshWoodSword);
		Arr_WeaponTag[tag].Add(ItemID.CandyCaneSword);
		Arr_WeaponTag[tag].Add(ItemID.BladeofGrass);
		Arr_WeaponTag[tag].Add(ItemID.FieryGreatsword);
		Arr_WeaponTag[tag].Add(ItemID.BreakerBlade);
		Arr_WeaponTag[tag].Add(ItemID.AdamantiteSword);
		Arr_WeaponTag[tag].Add(ItemID.TitaniumSword);
		Arr_WeaponTag[tag].Add(ItemID.DD2SquireDemonSword);
		Arr_WeaponTag[tag].Add(ItemID.ChlorophyteClaymore);
		Arr_WeaponTag[tag].Add(ItemID.TheHorsemansBlade);
		Arr_WeaponTag[tag].Add(ItemID.ChristmasTreeSword);
		Arr_WeaponTag[tag].Add(ItemID.DD2SquireBetsySword);
		Arr_WeaponTag[tag].Add(ItemID.StarWrath);
		Arr_WeaponTag[tag].Add(ItemID.Meowmere);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<TrueEnchantedSword>());

		Arr_WeaponTag[tag].Add(ModContent.ItemType<ShatteredSky>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<AkaiHanbunNoHasami>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<EverlastingCold>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<FrostSwordFish>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<DrainingVeilBlade>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<RelentlessAbomination>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<TwilightNight>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<DarkCactus>());
	}
	private void Add_BluntTag() {
		int tag = (int)WeaponTag.Blunt;

		Arr_WeaponTag[tag].Add(ItemID.BatBat);
		Arr_WeaponTag[tag].Add(ItemID.TentacleSpike);
		Arr_WeaponTag[tag].Add(ItemID.ZombieArm);
		Arr_WeaponTag[tag].Add(ItemID.PurpleClubberfish);
		Arr_WeaponTag[tag].Add(ItemID.SlapHand);
		Arr_WeaponTag[tag].Add(ItemID.TaxCollectorsStickOfDoom);
		Arr_WeaponTag[tag].Add(ItemID.WaffleIron);
		Arr_WeaponTag[tag].Add(ItemID.HamBat);
		Arr_WeaponTag[(int)WeaponTag.Blunt].Add(ItemID.AntlionClaw);
		Arr_WeaponTag[(int)WeaponTag.Blunt].Add(ItemID.KOCannon);
		Arr_WeaponTag[tag].Add(ItemID.GolemFist);
	}
	private void Add_SickleTag() {
		int tag = (int)WeaponTag.Sickle;

		Arr_WeaponTag[tag].Add(ItemID.Sickle);
		Arr_WeaponTag[tag].Add(ItemID.IceSickle);
		Arr_WeaponTag[tag].Add(ItemID.DeathSickle);
		Arr_WeaponTag[tag].Add(ItemID.ScytheWhip);
	}
	private void Add_SpearTag() {
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.Spear);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.Trident);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.ThunderSpear);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.TheRottedFork);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.Swordfish);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.DarkLance);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.PalladiumPike);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.CobaltNaginata);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.MythrilHalberd);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.OrichalcumHalberd);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.AdamantiteGlaive);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.TitaniumTrident);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.MonkStaffT2);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.Gungnir);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.ChlorophytePartisan);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.MushroomSpear);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.ObsidianSwordfish);
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ItemID.NorthPole);

		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ModContent.ItemType<BurningPassion>());
		Arr_WeaponTag[(int)WeaponTag.Spear].Add(ModContent.ItemType<AmberBoneSpear>());
	}
	private void Add_FlailTag() {
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.Mace);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.FlamingMace);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.BallOHurt);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.TheMeatball);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.BlueMoon);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.Sunfury);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.ChainKnife);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.DripplerFlail);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.DaoofPow);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.FlowerPow);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.Anchor);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.ChainGuillotines);
		Arr_WeaponTag[(int)WeaponTag.Flail].Add(ItemID.Flairon);
	}
	private void Add_YoyoTag() {
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.WoodYoyo);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Rally);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.CorruptYoyo);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.CrimsonYoyo);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.JungleYoyo);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Code1);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.HiveFive);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Valor);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Cascade);

		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Code2);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.FormatC);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Gradient);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Chik);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.HelFire);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Amarok);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Yelets);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.RedsYoyo);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.ValkyrieYoyo);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Kraken);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.TheEyeOfCthulhu);
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ItemID.Terrarian);

		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ModContent.ItemType<ShadowTrick>());
		Arr_WeaponTag[(int)WeaponTag.Yoyo].Add(ModContent.ItemType<YinYang>());
	}
	private void Add_BoomerangTag() {
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.WoodenBoomerang);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.EnchantedBoomerang);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.FruitcakeChakram);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.BloodyMachete);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.Shroomerang);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.IceBoomerang);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.ThornChakram);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.Trimarang);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.CombatWrench);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.Flamarang);

		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.FlyingKnife);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.BouncingShield);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.LightDisc);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.Bananarang);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.PaladinsHammer);
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ItemID.PossessedHatchet);

		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ModContent.ItemType<Bowmarang>());
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ModContent.ItemType<SharpBoomerang>());
		Arr_WeaponTag[(int)WeaponTag.Boomerang].Add(ModContent.ItemType<TheOrbit>());
	}
	private void Add_BowTag() {
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.WoodenBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.BorealWoodBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.RichMahoganyBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.EbonwoodBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.ShadewoodBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.PearlwoodBow);

		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.CopperBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.TinBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.LeadBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.IronBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.SilverBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.TungstenBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.GoldBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.PlatinumBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.DemonBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.TendonBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.BloodRainBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.BeesKnees);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.HellwingBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.MoltenFury);

		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.Marrow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.IceBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.ShadowFlameBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.DaedalusStormbow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.DD2PhoenixBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.PulseBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.DD2BetsyBow);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.Tsunami);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.FairyQueenRangedItem);
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ItemID.Phantasm);

		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<MoonStarBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<TheBurningSky>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<Bowmarang>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<CorruptedRebirth>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<ForceOfEarth>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<NatureSelection>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<IceStorm>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<TheTwoEvil>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<TundraBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<Resolve>());

		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<AmethystBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<TopazBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<SapphireBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<EmeraldBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<RubyBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<DiamondBow>());

		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<MasterWoodBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<GoldComplexBow>());
		Arr_WeaponTag[(int)WeaponTag.Bow].Add(ModContent.ItemType<PlatinumComplexBow>());
	}
	private void Add_RepeaterTag() {
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.CobaltRepeater);
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.PalladiumRepeater);
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.MythrilRepeater);
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.OrichalcumRepeater);
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.AdamantiteRepeater);
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.TitaniumRepeater);
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.HallowedRepeater);
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.ChlorophyteShotbow);
		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ItemID.StakeLauncher);

		Arr_WeaponTag[(int)WeaponTag.Repeater].Add(ModContent.ItemType<Unforgiving>());
	}
	private void Add_GunTag() {
		int tag = (int)WeaponTag.Gun;
		Arr_WeaponTag[tag].Add(ItemID.FlintlockPistol);
		Arr_WeaponTag[tag].Add(ItemID.TheUndertaker);
		Arr_WeaponTag[tag].Add(ItemID.Handgun);
		Arr_WeaponTag[tag].Add(ItemID.Revolver);
		Arr_WeaponTag[tag].Add(ItemID.PhoenixBlaster);
		Arr_WeaponTag[tag].Add(ItemID.PewMaticHorn);
		Arr_WeaponTag[tag].Add(ItemID.VenusMagnum);
		Arr_WeaponTag[tag].Add(ItemID.Xenopopper);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<BloodyShot>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SkullRevolver>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<OldSkullRevolver>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<KnifeRevolver>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<HeartPistol>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Deagle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<QuadDemonBlaster>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<UltimatePistol>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<MagicHandCannon>());

		Arr_WeaponTag[tag].Add(ModContent.ItemType<Gunmerang>());

		Arr_WeaponTag[tag].Add(ItemID.SpaceGun);

		Arr_WeaponTag[tag].Add(ItemID.Musket);
		Arr_WeaponTag[tag].Add(ItemID.Minishark);
		Arr_WeaponTag[tag].Add(ItemID.ClockworkAssaultRifle);
		Arr_WeaponTag[tag].Add(ItemID.Megashark);
		Arr_WeaponTag[tag].Add(ItemID.Gatligator);
		Arr_WeaponTag[tag].Add(ItemID.Uzi);
		Arr_WeaponTag[tag].Add(ItemID.CandyCornRifle);
		Arr_WeaponTag[tag].Add(ItemID.ChainGun);
		Arr_WeaponTag[tag].Add(ItemID.VortexBeater);
		Arr_WeaponTag[tag].Add(ItemID.SniperRifle);
		Arr_WeaponTag[tag].Add(ItemID.SDMG);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<ChaosMiniShark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Annihiliation>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<PulseRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<AngelicSmg>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Mixmaster>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<TheUnderdog>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Unforgiving>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<PaintRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<LaserSniper>());

		Arr_WeaponTag[tag].Add(ModContent.ItemType<BlueMinishark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<FrozenShark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<RifleShotgun>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<OvergrownMinishark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SingleBarrelMinishark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SnowballRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<HuntingRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<LongerMusket>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SniperRifle>());

		Arr_WeaponTag[tag].Add(ItemID.LaserRifle);

		Arr_WeaponTag[tag].Add(ItemID.TheUndertaker);
		Arr_WeaponTag[tag].Add(ItemID.Boomstick);
		Arr_WeaponTag[tag].Add(ItemID.QuadBarrelShotgun);
		Arr_WeaponTag[tag].Add(ItemID.Shotgun);
		Arr_WeaponTag[tag].Add(ItemID.OnyxBlaster);
		Arr_WeaponTag[tag].Add(ItemID.TacticalShotgun);
		Arr_WeaponTag[tag].Add(ItemID.Xenopopper);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<Merciless>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<PaintRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<HorusEye>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<UltimatePistol>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<QuadDemonBlaster>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<TundraBow>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<TheUnderdog>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Unforgiving>());

		Arr_WeaponTag[tag].Add(ModContent.ItemType<HuntingRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<RifleShotgun>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<RectangleShotgun>());

		Arr_WeaponTag[tag].Add(ItemID.ZapinatorGray);
		Arr_WeaponTag[tag].Add(ItemID.BeeGun);

		Arr_WeaponTag[tag].Add(ItemID.LaserRifle);
		Arr_WeaponTag[tag].Add(ItemID.ZapinatorOrange);
		Arr_WeaponTag[tag].Add(ItemID.WaspGun);
		Arr_WeaponTag[tag].Add(ItemID.LeafBlower);
		Arr_WeaponTag[tag].Add(ItemID.RainbowGun);
		Arr_WeaponTag[tag].Add(ItemID.HeatRay);
		Arr_WeaponTag[tag].Add(ItemID.LaserMachinegun);
		Arr_WeaponTag[tag].Add(ItemID.ChargedBlasterCannon);
		Arr_WeaponTag[tag].Add(ItemID.BubbleGun);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<MagicHandCannon>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<StarLightDistributer>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<ZapSnapper>());
	}
	private void Add_PistolTag() {
		int tag = (int)WeaponTag.Pistol;
		Arr_WeaponTag[tag].Add(ItemID.FlintlockPistol);
		Arr_WeaponTag[tag].Add(ItemID.TheUndertaker);
		Arr_WeaponTag[tag].Add(ItemID.Handgun);
		Arr_WeaponTag[tag].Add(ItemID.Revolver);
		Arr_WeaponTag[tag].Add(ItemID.PhoenixBlaster);
		Arr_WeaponTag[tag].Add(ItemID.PewMaticHorn);
		Arr_WeaponTag[tag].Add(ItemID.VenusMagnum);
		Arr_WeaponTag[tag].Add(ItemID.Xenopopper);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<BloodyShot>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SkullRevolver>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<OldSkullRevolver>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<KnifeRevolver>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<HeartPistol>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Deagle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<QuadDemonBlaster>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<UltimatePistol>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<MagicHandCannon>());

		Arr_WeaponTag[tag].Add(ModContent.ItemType<Gunmerang>());

		Arr_WeaponTag[tag].Add(ItemID.SpaceGun);
	}
	private void Add_RifleTag() {
		int tag = (int)WeaponTag.Rifle;

		Arr_WeaponTag[tag].Add(ItemID.Musket);
		Arr_WeaponTag[tag].Add(ItemID.Minishark);
		Arr_WeaponTag[tag].Add(ItemID.ClockworkAssaultRifle);
		Arr_WeaponTag[tag].Add(ItemID.Megashark);
		Arr_WeaponTag[tag].Add(ItemID.Gatligator);
		Arr_WeaponTag[tag].Add(ItemID.Uzi);
		Arr_WeaponTag[tag].Add(ItemID.CandyCornRifle);
		Arr_WeaponTag[tag].Add(ItemID.ChainGun);
		Arr_WeaponTag[tag].Add(ItemID.VortexBeater);
		Arr_WeaponTag[tag].Add(ItemID.SniperRifle);
		Arr_WeaponTag[tag].Add(ItemID.SDMG);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<ChaosMiniShark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Annihiliation>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<PulseRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<AngelicSmg>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Mixmaster>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<TheUnderdog>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Unforgiving>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<PaintRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<LaserSniper>());

		Arr_WeaponTag[tag].Add(ModContent.ItemType<BlueMinishark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<FrozenShark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<RifleShotgun>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<OvergrownMinishark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SingleBarrelMinishark>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SnowballRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<HuntingRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<LongerMusket>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<SniperRifle>());

		Arr_WeaponTag[tag].Add(ItemID.LaserRifle);
	}
	private void Add_ShotgunTag() {
		int tag = (int)WeaponTag.Shotgun;
		Arr_WeaponTag[tag].Add(ItemID.TheUndertaker);
		Arr_WeaponTag[tag].Add(ItemID.Boomstick);
		Arr_WeaponTag[tag].Add(ItemID.QuadBarrelShotgun);
		Arr_WeaponTag[tag].Add(ItemID.Shotgun);
		Arr_WeaponTag[tag].Add(ItemID.OnyxBlaster);
		Arr_WeaponTag[tag].Add(ItemID.TacticalShotgun);
		Arr_WeaponTag[tag].Add(ItemID.Xenopopper);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<Merciless>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<PaintRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<HorusEye>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<UltimatePistol>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<QuadDemonBlaster>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<TundraBow>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<TheUnderdog>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<Unforgiving>());

		Arr_WeaponTag[tag].Add(ModContent.ItemType<HuntingRifle>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<RifleShotgun>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<RectangleShotgun>());
	}
	private void Add_LauncherTag() {
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.FlareGun);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.Sandgun);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.SnowballCannon);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.Harpoon);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.SuperStarCannon);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.StarCannon);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.StakeLauncher);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.GrenadeLauncher);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.ProximityMineLauncher);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.RocketLauncher);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.NailGun);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.Stynger);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.JackOLanternLauncher);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.SnowmanCannon);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.FireworksLauncher);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.ElectrosphereLauncher);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.Celeb2);

		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.Flamethrower);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.ElfMelter);
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ItemID.PiranhaGun);

		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ModContent.ItemType<WinterFlame>());
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ModContent.ItemType<HandmadeLauncher>());
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ModContent.ItemType<SnowballCannonMK2>());
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ModContent.ItemType<SuperFlareGun>());
		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ModContent.ItemType<DeathBySpark>());

		Arr_WeaponTag[(int)WeaponTag.Launcher].Add(ModContent.ItemType<MagicHandCannon>());
	}
	private void Add_MagicStaffTag() {
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.AmethystStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.TopazStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.SapphireStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.EmeraldStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.RubyStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.DiamondStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.AmberStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.ThunderStaff);

		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.FrostStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.MeteorStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.BookStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.UnholyTrident);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.InfernoFork);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.PoisonStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.VenomStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.BlizzardStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.ShadowbeamStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.SpectreStaff);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.StaffofEarth);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.NettleBurst);
		Arr_WeaponTag[(int)WeaponTag.MagicStaff].Add(ItemID.ApprenticeStaffT3);

		Arr_WeaponTag[(int)WeaponTag.Sword].Add(ModContent.ItemType<AmethystSwotaff>());
		Arr_WeaponTag[(int)WeaponTag.Sword].Add(ModContent.ItemType<TopazSwotaff>());
		Arr_WeaponTag[(int)WeaponTag.Sword].Add(ModContent.ItemType<SapphireSwotaff>());
		Arr_WeaponTag[(int)WeaponTag.Sword].Add(ModContent.ItemType<EmeraldSwotaff>());
		Arr_WeaponTag[(int)WeaponTag.Sword].Add(ModContent.ItemType<RubySwotaff>());
		Arr_WeaponTag[(int)WeaponTag.Sword].Add(ModContent.ItemType<DiamondSwotaff>());
		Arr_WeaponTag[(int)WeaponTag.Sword].Add(ModContent.ItemType<AmberBoneSpear>());

		Arr_WeaponTag[(int)WeaponTag.Sword].Add(ModContent.ItemType<WyvernWrath>());
	}
	private void Add_MagicWandTag() {
		int tag = (int)WeaponTag.MagicWand;

		Arr_WeaponTag[tag].Add(ItemID.WandofFrosting);
		Arr_WeaponTag[tag].Add(ItemID.WandofSparking);
		Arr_WeaponTag[tag].Add(ItemID.CrimsonRod);
		Arr_WeaponTag[tag].Add(ItemID.Vilethorn);
		Arr_WeaponTag[tag].Add(ItemID.MagicMissile);
		Arr_WeaponTag[tag].Add(ItemID.Flamelash);
		Arr_WeaponTag[tag].Add(ItemID.AquaScepter);
		Arr_WeaponTag[tag].Add(ItemID.WeatherPain);
		Arr_WeaponTag[tag].Add(ItemID.FlowerofFire);

		Arr_WeaponTag[tag].Add(ItemID.FlowerofFrost);
		Arr_WeaponTag[tag].Add(ItemID.CrystalVileShard);
		Arr_WeaponTag[tag].Add(ItemID.SoulDrain);
		Arr_WeaponTag[tag].Add(ItemID.RainbowRod);
		Arr_WeaponTag[tag].Add(ItemID.BatScepter);
		Arr_WeaponTag[tag].Add(ItemID.PrincessWeapon);
		Arr_WeaponTag[tag].Add(ItemID.Razorpine);

		Arr_WeaponTag[tag].Add(ItemID.ClingerStaff);
		Arr_WeaponTag[tag].Add(ItemID.NimbusRod);
		Arr_WeaponTag[tag].Add(ItemID.SharpTears);
	}
	private void Add_MagicBookTag() {
		int tag = (int)WeaponTag.MagicBook;

		Arr_WeaponTag[tag].Add(ItemID.WaterBolt);
		Arr_WeaponTag[tag].Add(ItemID.BookofSkulls);
		Arr_WeaponTag[tag].Add(ItemID.DemonScythe);
		Arr_WeaponTag[tag].Add(ItemID.CursedFlames);
		Arr_WeaponTag[tag].Add(ItemID.GoldenShower);
		Arr_WeaponTag[tag].Add(ItemID.CrystalStorm);
		Arr_WeaponTag[tag].Add(ItemID.MagnetSphere);
		Arr_WeaponTag[tag].Add(ItemID.RazorbladeTyphoon);
		Arr_WeaponTag[tag].Add(ItemID.LunarFlareBook);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<SinisterBook>());
	}
	private void Add_MagicGunTag() {
		int tag = (int)WeaponTag.MagicGun;

		Arr_WeaponTag[tag].Add(ItemID.SpaceGun);
		Arr_WeaponTag[tag].Add(ItemID.ZapinatorGray);
		Arr_WeaponTag[tag].Add(ItemID.BeeGun);

		Arr_WeaponTag[tag].Add(ItemID.LaserRifle);
		Arr_WeaponTag[tag].Add(ItemID.ZapinatorOrange);
		Arr_WeaponTag[tag].Add(ItemID.WaspGun);
		Arr_WeaponTag[tag].Add(ItemID.LeafBlower);
		Arr_WeaponTag[tag].Add(ItemID.RainbowGun);
		Arr_WeaponTag[tag].Add(ItemID.HeatRay);
		Arr_WeaponTag[tag].Add(ItemID.LaserMachinegun);
		Arr_WeaponTag[tag].Add(ItemID.ChargedBlasterCannon);
		Arr_WeaponTag[tag].Add(ItemID.BubbleGun);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<LaserSniper>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<MagicHandCannon>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<StarLightDistributer>());
		Arr_WeaponTag[tag].Add(ModContent.ItemType<ZapSnapper>());
	}
	private void Add_SummonStaffTag() {
		int tag = (int)WeaponTag.SummonStaff;

		Arr_WeaponTag[tag].Add(ItemID.BabyBirdStaff);
		Arr_WeaponTag[tag].Add(ItemID.FlinxStaff);
		Arr_WeaponTag[tag].Add(ItemID.SlimeStaff);
		Arr_WeaponTag[tag].Add(ItemID.HornetStaff);
		Arr_WeaponTag[tag].Add(ItemID.VampireFrogStaff);
		Arr_WeaponTag[tag].Add(ItemID.ImpStaff);
		Arr_WeaponTag[tag].Add(ItemID.Smolstar);
		Arr_WeaponTag[tag].Add(ItemID.SpiderStaff);
		Arr_WeaponTag[tag].Add(ItemID.PirateStaff);
		Arr_WeaponTag[tag].Add(ItemID.SanguineStaff);

		Arr_WeaponTag[tag].Add(ItemID.OpticStaff);
		Arr_WeaponTag[tag].Add(ItemID.DeadlySphereStaff);
		Arr_WeaponTag[tag].Add(ItemID.PygmyStaff);
		Arr_WeaponTag[tag].Add(ItemID.RavenStaff);
		Arr_WeaponTag[tag].Add(ItemID.StormTigerStaff);
		Arr_WeaponTag[tag].Add(ItemID.TempestStaff);
		Arr_WeaponTag[tag].Add(ItemID.XenoStaff);
		Arr_WeaponTag[tag].Add(ItemID.StardustCellStaff);
		Arr_WeaponTag[tag].Add(ItemID.StardustDragonStaff);

		Arr_WeaponTag[tag].Add(ItemID.QueenSpiderStaff);
		Arr_WeaponTag[tag].Add(ItemID.StaffoftheFrostHydra);
		Arr_WeaponTag[tag].Add(ItemID.MoonlordTurretStaff);
		Arr_WeaponTag[tag].Add(ItemID.RainbowCrystalStaff);

		Arr_WeaponTag[tag].Add(ItemID.HoundiusShootius);
		Arr_WeaponTag[tag].Add(ItemID.DD2LightningAuraT1Popper);
		Arr_WeaponTag[tag].Add(ItemID.DD2LightningAuraT2Popper);
		Arr_WeaponTag[tag].Add(ItemID.DD2LightningAuraT3Popper);

		Arr_WeaponTag[tag].Add(ItemID.DD2ExplosiveTrapT1Popper);
		Arr_WeaponTag[tag].Add(ItemID.DD2ExplosiveTrapT2Popper);
		Arr_WeaponTag[tag].Add(ItemID.DD2ExplosiveTrapT3Popper);

		Arr_WeaponTag[tag].Add(ItemID.DD2FlameburstTowerT1Popper);
		Arr_WeaponTag[tag].Add(ItemID.DD2FlameburstTowerT2Popper);
		Arr_WeaponTag[tag].Add(ItemID.DD2FlameburstTowerT3Popper);

		Arr_WeaponTag[tag].Add(ItemID.DD2BallistraTowerT1Popper);
		Arr_WeaponTag[tag].Add(ItemID.DD2BallistraTowerT2Popper);
		Arr_WeaponTag[tag].Add(ItemID.DD2BallistraTowerT3Popper);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<StreetLamp>());
	}
	private void Add_SummonMiscTag() {
		int tag = (int)WeaponTag.SummonMisc;

		Arr_WeaponTag[tag].Add(ItemID.AbigailsFlower);
		Arr_WeaponTag[tag].Add(ItemID.EmpressBlade);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<StickyFlower>());
	}
	private void Add_WhipTag() {
		int tag = (int)WeaponTag.Whip;

		Arr_WeaponTag[tag].Add(ItemID.BlandWhip);
		Arr_WeaponTag[tag].Add(ItemID.IvyWhip);
		Arr_WeaponTag[tag].Add(ItemID.BoneWhip);
		Arr_WeaponTag[tag].Add(ItemID.FireWhip);
		Arr_WeaponTag[tag].Add(ItemID.CoolWhip);
		Arr_WeaponTag[tag].Add(ItemID.ThornWhip);
		Arr_WeaponTag[tag].Add(ItemID.SwordWhip);
		Arr_WeaponTag[tag].Add(ItemID.ScytheWhip);
		Arr_WeaponTag[tag].Add(ItemID.MaceWhip);
		Arr_WeaponTag[tag].Add(ItemID.RainbowWhip);

		Arr_WeaponTag[tag].Add(ModContent.ItemType<StarWhip>());
	}
	private void Add_OtherTag() {
		int tag = (int)WeaponTag.Other;

		Arr_WeaponTag[tag].Add(ItemID.Blowpipe);
		Arr_WeaponTag[tag].Add(ItemID.Blowgun);
		Arr_WeaponTag[tag].Add(ItemID.DartPistol);
		Arr_WeaponTag[tag].Add(ItemID.DartRifle);
		Arr_WeaponTag[tag].Add(ItemID.Toxikarp);
		Arr_WeaponTag[tag].Add(ItemID.BreathingReed);
		Arr_WeaponTag[tag].Add(ItemID.BladedGlove);
		Arr_WeaponTag[tag].Add(ItemID.StylistKilLaKillScissorsIWish);
		Arr_WeaponTag[tag].Add(ItemID.FetidBaghnakhs);
		Arr_WeaponTag[tag].Add(ItemID.PsychoKnife);

		Arr_WeaponTag[tag].Add(ItemID.JoustingLance);
		Arr_WeaponTag[tag].Add(ItemID.HallowJoustingLance);
		Arr_WeaponTag[tag].Add(ItemID.ShadowJoustingLance);

		Arr_WeaponTag[tag].Add(ItemID.ShadowFlameKnife);
		Arr_WeaponTag[tag].Add(ItemID.MonkStaffT1);
		Arr_WeaponTag[tag].Add(ItemID.ScourgeoftheCorruptor);
		Arr_WeaponTag[tag].Add(ItemID.VampireKnives);
		Arr_WeaponTag[tag].Add(ItemID.MonkStaffT3);
		Arr_WeaponTag[tag].Add(ItemID.DayBreak);
		Arr_WeaponTag[tag].Add(ItemID.SolarEruption);

		Arr_WeaponTag[tag].Add(ItemID.PainterPaintballGun);
		Arr_WeaponTag[tag].Add(ItemID.AleThrowingGlove);
		Arr_WeaponTag[tag].Add(ItemID.CoinGun);

		Arr_WeaponTag[tag].Add(ItemID.MedusaHead);
		Arr_WeaponTag[tag].Add(ItemID.SpiritFlame);
		Arr_WeaponTag[tag].Add(ItemID.ShadowFlameHexDoll);
		Arr_WeaponTag[tag].Add(ItemID.MagicalHarp);
		Arr_WeaponTag[tag].Add(ItemID.ToxicFlask);
		Arr_WeaponTag[tag].Add(ItemID.MagicDagger);

		Arr_WeaponTag[tag].Add(ItemID.FairyQueenMagicItem);
		Arr_WeaponTag[tag].Add(ItemID.SparkleGuitar);
		Arr_WeaponTag[tag].Add(ItemID.NebulaArcanum);
		Arr_WeaponTag[tag].Add(ItemID.NebulaBlaze);

		Arr_WeaponTag[tag].Add(ItemID.LastPrism);
	}
	private void Add_ReaperMarkTag() {
		int tag = (int)WeaponTag.ReaperMark;

		Arr_WeaponTag[tag].Add(ItemID.DeathSickle);
		Arr_WeaponTag[tag].Add(ItemID.BoneSword);
		Arr_WeaponTag[tag].Add(ItemID.ZombieArm);
		Arr_WeaponTag[tag].Add(ItemID.SpectreStaff);
		Arr_WeaponTag[tag].Add(ItemID.RavenStaff);
		Arr_WeaponTag[tag].Add(ItemID.ScytheWhip);
	}
	private void Add_HallowedGazeTag() {
		int tag = (int)WeaponTag.HallowedGaze;

		Arr_WeaponTag[tag].Add(ItemID.Excalibur);
		Arr_WeaponTag[tag].Add(ItemID.HallowedRepeater);
		Arr_WeaponTag[tag].Add(ItemID.LightDisc);
		Arr_WeaponTag[tag].Add(ItemID.Gungnir);
		Arr_WeaponTag[tag].Add(ItemID.TrueExcalibur);
		Arr_WeaponTag[tag].Add(ItemID.HallowJoustingLance);
		Arr_WeaponTag[tag].Add(ItemID.SwordWhip);
	}
	private void Add_WrathOfBlueMoonTag() {
		int tag = (int)WeaponTag.WrathOfBlueMoon;

		Arr_WeaponTag[tag].Add(ItemID.Muramasa);
		Arr_WeaponTag[tag].Add(ItemID.BlueMoon);
		Arr_WeaponTag[tag].Add(ItemID.AquaScepter);
		Arr_WeaponTag[tag].Add(ItemID.MagicMissile);
		Arr_WeaponTag[tag].Add(ItemID.WaterBolt);
	}
	private void Add_FuryOfTheSunTag() {
		int tag = (int)WeaponTag.FuryOfTheSun;

		Arr_WeaponTag[tag].Add(ItemID.Sunfury);
		Arr_WeaponTag[tag].Add(ItemID.FieryGreatsword);
		Arr_WeaponTag[tag].Add(ItemID.Flamelash);
		Arr_WeaponTag[tag].Add(ItemID.HellwingBow);
		Arr_WeaponTag[tag].Add(ItemID.PhoenixBlaster);
	}
	private void Add_EletricConductor() {
		int tag = (int)WeaponTag.ElectricConductor;

		Arr_WeaponTag[tag].Add(ItemID.CopperBroadsword);
		Arr_WeaponTag[tag].Add(ItemID.CopperShortsword);
		Arr_WeaponTag[tag].Add(ItemID.CopperBow);
		Arr_WeaponTag[tag].Add(ItemID.CopperAxe);
		Arr_WeaponTag[tag].Add(ItemID.CopperHammer);
	}
	private void Add_Avarice() {
		int tag = (int)WeaponTag.Avarice;

		Arr_WeaponTag[tag].Add(ItemID.PlatinumAxe);
		Arr_WeaponTag[tag].Add(ItemID.PlatinumBow);
		Arr_WeaponTag[tag].Add(ItemID.PlatinumHammer);
		Arr_WeaponTag[tag].Add(ItemID.PlatinumShortsword);
		Arr_WeaponTag[tag].Add(ItemID.PlatinumBroadsword);
	}
}
public class WeaponEffect_ModPlayer : ModPlayer {
	//This is not really clean but I really don't want to create a Modplayer class just to store a single field
	public int OutroEffect_RejuvinatingGlow_Counter = 0;
	public int[] Arr_WeaponEffect = [];
	public List<int> Easy_WeaponEffectFollow = new();
	public int IntroEffect_Duration = 0;
	public int IntroEffect_ItemType = -1;
	public static bool Check_ValidForIntroEffect(Player player) => player.GetModPlayer<WeaponEffect_ModPlayer>().IntroEffect_ItemType == player.HeldItem.type;
	/// <summary>
	/// Check whenever or not if the intro effect is valid, this is different from <see cref="Check_ValidForIntroEffect(Player)"/>
	/// </summary>
	/// <param name="player"></param>
	/// <param name="itemType"></param>
	/// <returns></returns>
	public static bool Check_IntroEffect(Player player, int itemType) {
		if (player.TryGetModPlayer(out WeaponEffect_ModPlayer modplayer)) {
			return modplayer.IntroEffect_Duration > 0 && modplayer.IntroEffect_ItemType == itemType;
		}
		return false;
	}
	public static int Get_CurrentIntroEffect(Player player) => player.GetModPlayer<WeaponEffect_ModPlayer>().IntroEffect_ItemType;
	public static void Set_IntroEffect(Player player, int itemType, int duration) {
		var modplayer = player.GetModPlayer<WeaponEffect_ModPlayer>();
		modplayer.IntroEffect_ItemType = itemType;
		modplayer.IntroEffect_Duration = duration;
	}
	public override void Initialize() {
		Array.Resize(ref Arr_WeaponEffect, OutroEffectSystem.list_effect.Count);
		IntroEffect_ItemType = -1;
	}
	public void Add_WeaponEffect(int type) {
		var ef = OutroEffectSystem.GetWeaponEffect(type);
		if (ef == null) {
			return;
		}
		Add_WeaponEffect(ef);
	}
	public void Add_WeaponEffect(WeaponEffect ef) {
		Arr_WeaponEffect[ef.Type] = ef.Duration;
		if (!Easy_WeaponEffectFollow.Contains(ef.Type)) {
			Easy_WeaponEffectFollow.Add(ef.Type);
			ef.OnAdd(Player);
		}
	}
	public override void ResetEffects() {
		if (!Player.active) {
			return;
		}
		OutroEffect_RejuvinatingGlow_Counter = ModUtils.CountDown(OutroEffect_RejuvinatingGlow_Counter);
		if (--IntroEffect_Duration <= 0) {
			IntroEffect_Duration = 0;
			IntroEffect_ItemType = Player.HeldItem.type;
		}
	}
	public override void UpdateEquips() {
		for (int i = 0; i < Arr_WeaponEffect.Length; i++) {
			if (--Arr_WeaponEffect[i] <= 0) {
				var ef2 = OutroEffectSystem.GetWeaponEffect(i);
				if (ef2 != null) {
					ef2.OnRemove(Player);
				}
				Arr_WeaponEffect[i] = 0;
				Easy_WeaponEffectFollow.Remove(i);
				continue;
			}
			var ef = OutroEffectSystem.GetWeaponEffect(i);
			if (ef == null) {
				continue;
			}
			ef.Update(Player);
		}
	}
	public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
		for (int i = 0; i < Arr_WeaponEffect.Length; i++) {
			if (Arr_WeaponEffect[i] <= 0) {
				continue;
			}
			var ef = OutroEffectSystem.GetWeaponEffect(i);
			if (ef == null) {
				continue;
			}
			ef.WeaponDamage(Player, item, ref damage);
		}
	}
	public override void ModifyWeaponCrit(Item item, ref float crit) {
		for (int i = 0; i < Arr_WeaponEffect.Length; i++) {
			if (Arr_WeaponEffect[i] <= 0) {
				continue;
			}
			var ef = OutroEffectSystem.GetWeaponEffect(i);
			if (ef == null) {
				continue;
			}
			ef.WeaponCrit(Player, item, ref crit);
		}
	}
	public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback) {
		for (int i = 0; i < Arr_WeaponEffect.Length; i++) {
			if (Arr_WeaponEffect[i] <= 0) {
				continue;
			}
			var ef = OutroEffectSystem.GetWeaponEffect(i);
			if (ef == null) {
				continue;
			}
			ef.WeaponKnockBack(Player, item, ref knockback);
		}
	}
	public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers) {
		for (int i = 0; i < Arr_WeaponEffect.Length; i++) {
			if (Arr_WeaponEffect[i] <= 0) {
				continue;
			}
			var ef = OutroEffectSystem.GetWeaponEffect(i);
			if (ef == null) {
				continue;
			}
			ef.ModifyHitItem(Player, target, ref modifiers);
		}
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers) {
		if (!proj.Check_ItemTypeSource(Player.HeldItem.type)) {
			return;
		}
		for (int i = 0; i < Arr_WeaponEffect.Length; i++) {
			if (Arr_WeaponEffect[i] <= 0) {
				continue;
			}
			var ef = OutroEffectSystem.GetWeaponEffect(i);
			if (ef == null) {
				continue;
			}
			ef.ModifyHitProj(Player, proj, target, ref modifiers);
		}
	}
}
public class WeaponEffect_GlobalItem : GlobalItem {
	public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
		if (WeaponEffect_ModPlayer.Check_IntroEffect(Main.LocalPlayer, item.type)) {
			Main.instance.LoadItem(item.type);
			var texture = TextureAssets.Item[item.type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(1.5f, 1.5f), frame, Color.White with { A = 0 }, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(1.5f, -1.5f), frame, Color.White with { A = 0 }, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, 1.5f), frame, Color.White with { A = 0 }, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, -1.5f), frame, Color.White with { A = 0 }, 0, origin, scale, SpriteEffects.None, 0);
			}
		}
		return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
	}
}
public abstract class WeaponEffect : ModType {
	public short Type = -1;
	public int Duration = 0;
	public string DisplayName => ModUtils.LocalizationText("Outro", $"{Name}.DisplayName");
	public string Description => ModUtils.LocalizationText("Outro", $"{Name}.Description");
	protected string Tooltip => ModUtils.LocalizationText("Outro", $"{Name}.Tooltip");
	public virtual string ModifyTooltip() => string.Format(Tooltip, Duration / 60);
	public static int GetWeaponEffectType<T>() where T : WeaponEffect => ModContent.GetInstance<T>().Type;
	protected sealed override void Register() {
		Type = OutroEffectSystem.Register(this);
		SetStaticDefaults();
	}
	public virtual void OnAdd(Player player) { }
	public virtual void OnRemove(Player player) { }
	public virtual void Update(Player player) { }
	public virtual void WeaponDamage(Player player, Item item, ref StatModifier damage) { }
	/// <summary>
	/// The crit number take whole number
	/// </summary>
	/// <param name="player"></param>
	/// <param name="item"></param>
	/// <param name="crit"></param>
	public virtual void WeaponCrit(Player player, Item item, ref float crit) { }
	public virtual void WeaponKnockBack(Player player, Item item, ref StatModifier knockback) { }
	public virtual void ModifyHitItem(Player player, NPC npc, ref NPC.HitModifiers mod) { }
	public virtual void ModifyHitProj(Player player, Projectile proj, NPC npc, ref NPC.HitModifiers mod) { }
}
public enum WeaponTag : byte {
	None,
	//Weapon type
	Sword,
	Thrustsword,
	Greatsword,
	Blunt,
	Sickle,

	Spear,
	Flail,
	Yoyo,
	Boomerang,

	Bow,
	Repeater,
	Gun,
	Pistol,
	Rifle,
	Shotgun,
	Launcher,

	MagicStaff,
	MagicWand,
	MagicGun,
	MagicBook,

	SummonStaff,
	SummonMisc,
	Whip,

	Other,

	HallowedGaze,
	WrathOfBlueMoon,
	FuryOfTheSun,
	ElectricConductor,
	Avarice,
	Wooden,
	Ore,
	/// <summary>
	/// Not to be confused with actual magic weapon<br/>
	/// This is for weapon that shoot magical projectile like ice blade and Blood Rain Bow
	/// </summary>
	Magical,
	/// <summary>
	/// Not to be confused with <see cref="Magical"/><br/>
	/// This is for weapon that are like Candy Cane Sword and Tentacle Spike 
	/// </summary>
	Fantasy,
	/// <summary>
	/// For weapon that is a actual living creature like toxickarp
	/// </summary>
	Living,
	/// <summary>
	/// For weapon that shoot out elemental like fire, or water like ice bow, waterbolt, etc
	/// </summary>
	Elemental,
	/// <summary>
	/// For weapon that is consider must have like Night Edge, Terra Blade, True Night Edge, Last Prism, etc<br/>
	/// in more simple term, it is for weapon that consider to be lengendary
	/// </summary>
	Mythical,
	/// <summary>
	/// Lunar and moon lord weapon
	/// </summary>
	Celestial,
	/// <summary>
	/// For weapon that is a musical instrument
	/// </summary>
	Musical,
	ReaperMark,
}
public class UIImage_WeaponEffectShower : Roguelike_UIImage {
	public UIImage_WeaponEffectShower() : base(TextureAssets.InventoryBack7) {
	}
	int Counter = 0;
	public override void UpdateImage(GameTime gameTime) {
		base.UpdateImage(gameTime);
		var player = Main.LocalPlayer;
		var modplayer = player.GetModPlayer<WeaponEffect_ModPlayer>();
		if (modplayer.Easy_WeaponEffectFollow.Count <= 0) {
			Counter = ModUtils.CountDown(Counter);
		}
		else {
			if (++Counter >= 100) {
				Counter = 100;
			}
		}
		Color = OriginalColor * (Counter / 100f);
	}
	public override void DrawImage(SpriteBatch spriteBatch) {
		if (Counter > 0 && IsMouseHovering) {
			string textEf = "";
			var player = Main.LocalPlayer;
			var modplayer = player.GetModPlayer<WeaponEffect_ModPlayer>();
			for (int i = 0; i < modplayer.Easy_WeaponEffectFollow.Count; i++) {
				var eff = OutroEffectSystem.GetWeaponEffect(modplayer.Easy_WeaponEffectFollow[i]);
				if (i == modplayer.Easy_WeaponEffectFollow.Count - 1) {
					textEf += $"[{eff.DisplayName}]: \n{eff.Description}";
					continue;
				}
				textEf += $"[{eff.DisplayName}]: \n{eff.Description} \n";
			}
			UICommon.TooltipMouseText(textEf);
		}
	}
}
