using EverlastingOverhaul.Common.BuffAndDebuff;
using EverlastingOverhaul.Common.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class CopperArmor : ModArmorSet
{
    public override void SetDefault()
    {
        headID = ItemID.CopperHelmet;
        bodyID = ItemID.CopperChainmail;
        legID = ItemID.CopperGreaves;
    }
}
public class CopperHelmet : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.CopperHelmet;
        Add_Defense = 2;
        TypeEquipment = Type_Head;
        ArmorName = "CopperArmor";
        AddTooltip = true;
    }
    public override void UpdateEquip(Player player, Item item)
    {
        player.GetModPlayer<CopperArmorModPlayer>().CritDmgAgainstElec = true;
        player.GetModPlayer<CopperArmorModPlayer>().ElectricityChance += .02f;
    }
}
public class CopperChainmail : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.CopperChainmail;
        Add_Defense = 3;
        TypeEquipment = Type_Body;
        ArmorName = "CopperArmor";
        AddTooltip = true;
    }
    public override void UpdateEquip(Player player, Item item)
    {
        player.GetModPlayer<CopperArmorModPlayer>().ONHitEffect = true;
        player.GetModPlayer<CopperArmorModPlayer>().ElectricityChance += .03f;
    }
}
public class CopperGreaves : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.CopperGreaves;
        Add_Defense = 2;
        TypeEquipment = Type_Leg;
        ArmorName = "CopperArmor";
        AddTooltip = true;
    }
    public override void UpdateEquip(Player player, Item item)
    {
        player.GetModPlayer<CopperArmorModPlayer>().DmgAgainstElec = true;
        player.GetModPlayer<CopperArmorModPlayer>().ElectricityChance += .01f;
    }
}
public class CopperArmorModPlayer : ModPlayer
{
    public bool CritDmgAgainstElec = false;
    public bool DmgAgainstElec = false;
    public bool ONHitEffect = false;
    public float ElectricityChance = 0;
    public override void ResetEffects()
    {
        CritDmgAgainstElec = false;
        DmgAgainstElec = false;
        ONHitEffect = false;
        ElectricityChance = 0;
    }
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (!target.HasBuff(BuffID.Electrified))
        {
            return;
        }
        if (CritDmgAgainstElec)
        {
            modifiers.CritDamage += .25f;
        }
        if (DmgAgainstElec)
        {
            modifiers.SourceDamage += .12f;
        }
    }
    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        if (ONHitEffect)
        {
            npc.AddBuff(BuffID.Electrified, ModUtils.ToSecond(3));
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.NextFloat() <= ElectricityChance)
        {
            target.AddBuff(BuffID.Electrified, ModUtils.ToSecond(Main.rand.Next(4, 7)));
        }
    }
}
public class CopperArmorPlayer : PlayerArmorHandle
{
    int CopperArmorChargeCounter = 0;
    public override void SetStaticDefaults()
    {
        ArmorLoader.SetModPlayer("CopperArmor", this);
    }
    public override void Armor_UpdateEquipsSet()
    {
        Player.moveSpeed += 0.25f;
    }
    public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        OnHitNPC_CopperArmor();
    }
    private void OnHitNPC_CopperArmor()
    {
        if (Player.ZoneRain)
            CopperArmorChargeCounter++;
        if (++CopperArmorChargeCounter >= 50)
        {
            Player.AddBuff(ModContent.BuffType<OverCharged>(), 300);
            CopperArmorChargeCounter = 0;
        }
    }
}
