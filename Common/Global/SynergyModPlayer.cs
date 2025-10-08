using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Global;

    public class SynergyModPlayer : ModPlayer
{
    public int ItemTypeCurrent = 0;
    public Item itemOld = null;
    public int ItemTypeOld = 0;
    public bool acc_SynergyEnergy = false;
    public override void ResetEffects()
    {
        acc_SynergyEnergy = false;
        Item item = Player.HeldItem;
        if (item.type == ItemID.None)
        {
            return;
        }
        if (ItemTypeCurrent != item.type)
        {
            ItemTypeCurrent = item.type;
            itemOld = item;
        }
        if (Player.itemAnimation == 1)
        {
            ItemTypeOld = ItemTypeCurrent;
        }
    }

    public bool CompareOldvsNewItemType => ItemTypeCurrent != ItemTypeOld;
    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        damage = damage.CombineWith(Player.GetModPlayer<PlayerStatsHandle>().SynergyDamage);
    }
}