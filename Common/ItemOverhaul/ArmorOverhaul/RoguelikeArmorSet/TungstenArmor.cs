using EverlastingOverhaul.Common.Global;
using Terraria;
using Terraria.ID;
namespace EverlastingOverhaul.Common.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class TungstenArmor : ModArmorSet
{
    public override void SetDefault()
    {
        headID = ItemID.TungstenHelmet;
        bodyID = ItemID.TungstenChainmail;
        legID = ItemID.TungstenGreaves;
    }
}
public class TungstenHelmet : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.TungstenHelmet;
        Add_Defense = 9;
        AddTooltip = true;
        TypeEquipment = Type_Head;
        ArmorName = "TungstenArmor";
    }
    public override void UpdateEquip(Player player, Item item)
    {
        var modplayer = player.GetModPlayer<PlayerStatsHandle>();
        modplayer.UpdateMovement -= .05f;
        modplayer.UpdateJumpBoost -= .1f;
    }
}
public class TungstenChainmail : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.TungstenChainmail;
        Add_Defense = 10;
        AddTooltip = true;
        TypeEquipment = Type_Body;
        ArmorName = "TungstenArmor";
    }
    public override void UpdateEquip(Player player, Item item)
    {
        var modplayer = player.GetModPlayer<PlayerStatsHandle>();
        modplayer.UpdateMovement -= .2f;
        modplayer.UpdateJumpBoost -= .3f;
    }
}

public class TungstenGreaves : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.TungstenGreaves;
        Add_Defense = 8;
        AddTooltip = true;
        TypeEquipment = Type_Leg;
        ArmorName = "TungstenArmor";
    }
    public override void UpdateEquip(Player player, Item item)
    {
        var modplayer = player.GetModPlayer<PlayerStatsHandle>();
        modplayer.UpdateMovement -= .15f;
        modplayer.UpdateJumpBoost -= .2f;
    }
}
public class TungstenArmorPlayer : PlayerArmorHandle
{
    public override void SetStaticDefaults()
    {
        ArmorLoader.SetModPlayer("TungstenArmor", this);
    }
    public override void Armor_UpdateEquipsSet()
    {
        Player.noKnockback = true;
        Player.maxFallSpeed += 20;
    }
    public override void Armor_ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        modifiers.SetMaxDamage(100);
    }
    public override void Armor_ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
    {
        modifiers.SetMaxDamage(100);
    }
}
