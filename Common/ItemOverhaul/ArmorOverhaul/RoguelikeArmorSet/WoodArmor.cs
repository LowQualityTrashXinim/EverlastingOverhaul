using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.Global;
using System.Linq;
using EverlastingOverhaul.Common.ItemOverhaul.ArmorOverhaul;
using EverlastingOverhaul.Contents.Projectiles;

namespace EverlastingOverhaul.Common.ItemOverhaul.ArmorOverhaul.RoguelikeArmorSet;
internal class WoodArmor : ModArmorSet
{
    public override void SetDefault()
    {
        headID = ItemID.WoodHelmet;
        bodyID = ItemID.WoodBreastplate;
        legID = ItemID.WoodGreaves;
    }
}
class WoodHelmet : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.WoodHelmet;
        Add_Defense = 3;
        AddTooltip = true;
        ArmorName = "WoodArmor";
        TypeEquipment = Type_Head;
    }
    public override void UpdateEquip(Player player, Item item)
    {
        player.GetModPlayer<WoodArmorPublicPlayer>().IncreasesWoodWeaponDamage = true;
        player.GetModPlayer<WoodArmorPublicPlayer>().AcornSpawnChance += .02f;
        player.GetCritChance<GenericDamageClass>() += 5;
    }
}
class WoodBreastplate : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.WoodBreastplate;
        Add_Defense = 4;
        AddTooltip = true;
        ArmorName = "WoodArmor";
        TypeEquipment = Type_Body;
    }
    public override void UpdateEquip(Player player, Item item)
    {
        player.GetModPlayer<WoodArmorPublicPlayer>().AcornSpawnChance += .03f;
        PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.PureDamage, Multiplicative: 1.1f);
        player.GetModPlayer<WoodArmorPublicPlayer>().IncreasesWoodWeaponCritChance = true;
    }
}
class WoodGreaves : ModArmorPiece
{
    public override void SetDefault()
    {
        PieceID = ItemID.WoodGreaves;
        Add_Defense = 3;
        AddTooltip = true;
        ArmorName = "WoodArmor";
        TypeEquipment = Type_Leg;
    }
    public override void UpdateEquip(Player player, Item item)
    {
        player.GetModPlayer<WoodArmorPublicPlayer>().AcornSpawnChance += .01f;
        PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.MovementSpeed, 1.15f);
        PlayerStatsHandle.AddStatsToPlayer(player, PlayerStats.JumpBoost, 1.15f);
    }
}
public class WoodArmorPublicPlayer : ModPlayer
{
    public bool IncreasesWoodWeaponDamage = false;
    public bool IncreasesWoodWeaponCritChance = false;
    public float AcornSpawnChance = 0;
    public override void ResetEffects()
    {
        IncreasesWoodWeaponDamage = false;
        IncreasesWoodWeaponCritChance = false;
        AcornSpawnChance = 0;
    }
    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        if (WoodWeaponCheck(item.type) && IncreasesWoodWeaponDamage)
        {
            damage += .2f;
        }
    }
    public override void ModifyWeaponCrit(Item item, ref float crit)
    {
        if (WoodWeaponCheck(item.type) && IncreasesWoodWeaponCritChance)
        {
            crit += 15;
        }
    }
    private bool WoodWeaponCheck(int type) => TerrariaArrayID.AllWoodBowPHM.Contains(type) || TerrariaArrayID.AllWoodSword.Contains(type);
    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.NextFloat() <= AcornSpawnChance)
        {
            SpawnAcorn(target);
        }
    }
    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.NextFloat() <= AcornSpawnChance && (proj.ModProjectile == null || proj.ModProjectile is not AcornProjectile))
        {
            SpawnAcorn(target);
        }
    }
    private void SpawnAcorn(NPC target)
    {
        int damage = Player.GetWeaponDamage(Player.HeldItem);

        int proj = Projectile.NewProjectile(Player.GetSource_FromThis(),
                target.Center - new Vector2(0, 400),
                Vector2.UnitY * 1.35f,
                ModContent.ProjectileType<AcornProjectile>(), 10 + damage / 5, 1f, Player.whoAmI);

        var projectile = Main.projectile[proj];
        projectile.damage += (int)(damage * .2f);
        projectile.CritChance += 15;
    }
}
class WoodArmorPlayer : PlayerArmorHandle
{
    public override void SetStaticDefaults()
    {
        ArmorLoader.SetModPlayer("WoodArmor", this);
    }
    public override void Armor_UpdateEquipsSet()
    {
        Player.statDefense += 5;
        Player.moveSpeed += .25f;
        Player.GetModPlayer<WoodArmorPublicPlayer>().AcornSpawnChance += .1f;
    }
    public override void Armor_OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        int typeProjectile = ModContent.ProjectileType<SunflowerProjectile>();
        if (Main.rand.NextBool(20) && Player.ownedProjectileCounts[typeProjectile] < 1)
        {
            Projectile.NewProjectile(Player.GetSource_FromAI(), Player.Center, Vector2.Zero, typeProjectile, 0, 0, Player.whoAmI, Main.rand.Next(0, 270));
        }
    }
}
