using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Items.Weapon;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public partial class EverlastingOverhaul : Mod
    {

    }
}
public class ModItemLib : ModSystem
{
    public static List<int> FireDeBuff;
    public static bool[] IsPoisonBuff;
    public static bool[] CanBeAffectByLastingVile;
    public static bool[] AdvancedRPGItem;
    public static List<int> Shield = new() {
                    ItemID.SquireShield,
            ItemID.EoCShield,
            ItemID.CobaltShield,
            ItemID.ObsidianShield,
            ItemID.PaladinsShield,
            ItemID.AnkhShield,
            ItemID.FrozenShield,
            ItemID.HeroShield};
    public static HashSet<Item> List_Weapon { get; private set; }
    public static HashSet<int> MinionPetMountBuff { get; private set; }
    public static List<Item> SynergyItem { get; private set; }
    public override void OnModLoad()
    {
        List_Weapon = new();
        MinionPetMountBuff = new();
        FireDeBuff = new();
        SynergyItem = new();
    }
    public override void OnModUnload()
    {
        FireDeBuff = null;
        IsPoisonBuff = null;
        List_Weapon = null;
        MinionPetMountBuff = null;
        SynergyItem = null;
    }
    public override void PostSetupContent()
    {
        FireDeBuff.AddRange([BuffID.OnFire, BuffID.OnFire3, BuffID.ShadowFlame, BuffID.Frostburn, BuffID.Frostburn2, BuffID.CursedInferno]);
        IsPoisonBuff = BuffID.Sets.Factory.CreateBoolSet(BuffID.Poisoned, BuffID.Venom);
        AdvancedRPGItem = ItemID.Sets.Factory.CreateBoolSet();
        List<Item> cacheitemList = ContentSamples.ItemsByType.Values.ToList();
        for (int i = 0; i < cacheitemList.Count; i++)
        {
            Item item = cacheitemList[i];
            if (item.IsAWeapon())
            {
                if (item.buffType != -1 && item.shoot != ProjectileID.None)
                {
                    if (ContentSamples.ProjectilesByType[item.shoot].minion)
                    {
                        MinionPetMountBuff.Add(item.buffType);
                    }
                }
                List_Weapon.Add(item);
            }
            if (item.ModItem is SynergyModItem)
            {
                SynergyItem.Add(item);
            }
        }
    }
}
