using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.RoguelikeMode.RoguelikeChange.Prefixes;

namespace EverlastingOverhaul.Common.Global;

public class WorldVaultSystem : ModSystem
{
    public static short Set_Variant = -1;
    private static Dictionary<int, List<ModVariant>> variantlist = new();
    public static short Register(ModVariant variant)
    {
        ModTypeLookup<ModVariant>.Register(variant);
        if (variantlist.ContainsKey(variant.ItemType))
        {
            variantlist[variant.ItemType].Add(variant);
        }
        else
        {
            variantlist.Add(variant.ItemType, new() { variant });
        }
        return (short)(variantlist[variant.ItemType].Count - 1);
    }
    public static ModVariant GetVariant(int ItemType, short variant)
    {
        if (variantlist.ContainsKey(ItemType))
        {
            foreach (var item in variantlist[ItemType])
            {
                if (item.Variant == variant)
                {
                    return item;
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }
}
public abstract class ModVariant : ModType
{
    public short Variant = 0;
    public int ItemType = 0;
    public static short GetVariantType<T>() where T : ModVariant => ModContent.GetInstance<T>().Variant;
    protected sealed override void Register()
    {
        SetStaticDefaults();
        Variant = WorldVaultSystem.Register(this);
    }
    public virtual void SetDefault(Item item) { }
}
/// <summary>
/// This class hold mainly tooltip information<br/>
/// However this doesn't handle overhaul information
/// </summary>
public class GlobalItemHandle : GlobalItem
{
    /// <summary>
    /// Use this to set variant before using Player.QuickSpawnItem or Item.NewItemDirect<br/>
    /// This is a hacky way of setting up custom stats for item
    /// </summary>
    public const byte None = 0;
    public override bool InstancePerEntity => true;
    public bool DebugItem = false;
    public bool ExtraInfo = false;
    public bool RequiredWeaponGuide = false;
    public bool AdvancedBuffItem = false;
    public bool OverrideVanillaEffect = false;
    public int Counter = 0;
    public short VariantType = -1;
    public override void SetDefaults(Item entity)
    {
        if (WorldVaultSystem.Set_Variant != -1)
        {
            VariantType = WorldVaultSystem.Set_Variant;
            var variant = WorldVaultSystem.GetVariant(entity.type, VariantType);
            if (variant != null)
            {
                variant.SetDefault(entity);
            }
            WorldVaultSystem.Set_Variant = 0;
        }
    }
    public float CriticalDamage;
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {

        //tooltips.Add(new(Mod, "Debug", $"Item width : {item.width} | height {item.height}"));
        if (item.IsAWeapon(true))
        {
            for (int i = 0; i < tooltips.Count; i++)
            {
                TooltipLine line = tooltips[i];
                if (line.Name == "CritChance")
                {
                    tooltips.Insert(i + 1, new(Mod, "CritDamage", $"{Math.Round(CriticalDamage, 2) * 100}% bonus critical damage"));
                    tooltips.Insert(i + 2, new(Mod, "ArmorPenetration", $"{item.ArmorPenetration} Armor penetration"));
                }
                else if (line.Name == "Damage")
                {
                    line.Text = line.Text + $" | Base : {item.OriginalDamage}";
                }
                else if (line.Name == "Knockback")
                {
                    line.Text = line.Text + $" | Base : {Math.Round(ContentSamples.ItemsByType[item.type].knockBack, 2)} | Modified : {Math.Round(Main.LocalPlayer.GetWeaponKnockback(item), 2)}";
                }
            }
        }
        if (item.ModItem == null)
        {
            return;
        }
        if (item.ModItem.Mod != Mod)
        {
            return;
        }
        TooltipLine NameLine = tooltips.Where(t => t.Name == "ItemName").FirstOrDefault();
        if (DebugItem && NameLine != null)
        {
            NameLine.Text += " [Debug]";
            NameLine.OverrideColor = Color.MediumPurple;
            return;
        }
        ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
        if (ExtraInfo && item.ModItem != null)
        {
            if (!moddedplayer.Shift_Option())
            {
                tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for more infomation]") { OverrideColor = Color.Gray });
            }
        }
        if (RequiredWeaponGuide && item.ModItem != null)
        {
            if (!moddedplayer.Shift_Option())
            {
                tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for weapon guide]") { OverrideColor = Color.Gray });
            }
        }
        if (AdvancedBuffItem && NameLine != null)
        {
            NameLine.Text += " [Advanced]";
        }
    }
    public override void UpdateInventory(Item item, Player player)
    {
        if (++Counter >= int.MaxValue / 10)
        {
            Counter = 0;
        }
        if (item.prefix == ModContent.PrefixType<Chaotic>() && Counter % 100 == 0)
        {
            Prefix_ChaoticEffect(item);
        }
        else if (item.prefix == ModContent.PrefixType<Unstable>() && Counter % 600 == 0)
        {
            Prefix_UnstableEffect(item);
        }
    }
    public void Prefix_ChaoticEffect(Item item)
    {
        if (item.damage < item.OriginalDamage / 2)
        {
            item.damage += Main.rand.Next(0, 2);
        }
        else
        {
            item.damage += Main.rand.Next(-1, 2);
        }
        if (item.crit <= 1)
        {
            item.crit += Main.rand.Next(0, 2);
        }
        else
        {
            item.crit += Main.rand.Next(-1, 2);
        }
        if (CriticalDamage <= -.5f)
        {
            CriticalDamage += Main.rand.NextFloat(0, .2f);
        }
        else
        {
            CriticalDamage += Main.rand.NextFloat(-.2f, .2f);
        }
        item.knockBack += Main.rand.NextFloat(-1, 1);
    }
    public void Prefix_UnstableEffect(Item item)
    {
        item.SetDefaults(Main.rand.NextFromHashSet(ModItemLib.List_Weapon).type);
        if (!Main.rand.NextBool(1000))
        {
            item.prefix = ModContent.PrefixType<Unstable>();
        }
    }
    public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
    {
        if (item.ModItem == null)
        {
            return true;
        }
        //Prevent possible conflict, basically hardcoding to make it so that it only work for item belong to this mod
        if (item.ModItem.Mod.Name != Mod.Name)
        {
            return true;
        }
        if (item.ModItem != null)
        {
            string value = null;
            if (ExtraInfo)
            {
                value = ModUtils.LocalizationText("Items", $"{item.ModItem.Name}.ExtraInfo");
            }
            if (RequiredWeaponGuide)
            {
                value = ModUtils.LocalizationText("Items", $"{item.ModItem.Name}.Guide");
            }
            if (value == null)
            {
                return base.PreDrawTooltip(item, lines, ref x, ref y); ;
            }
            ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
            if (moddedplayer.Shift_Option())
            {
                float width;
                float height = -16;
                Vector2 pos;
                DynamicSpriteFont font = FontAssets.MouseText.Value;
                if (Main.MouseScreen.X < Main.screenWidth / 2)
                {
                    string widest = lines.OrderBy(n => ChatManager.GetStringSize(font, n.Text, Vector2.One).X).Last().Text;
                    width = ChatManager.GetStringSize(font, widest, Vector2.One).X;
                    pos = new Vector2(x, y) + new Vector2(width + 30, 0);
                }
                else
                {
                    width = ChatManager.GetStringSize(font, value, Vector2.One).X + 20;
                    pos = new Vector2(x, y) - new Vector2(width + 30, 0);
                }
                width = ChatManager.GetStringSize(font, value, Vector2.One).X + 20;
                height += ChatManager.GetStringSize(font, value, Vector2.One).Y + 16;
                Terraria.Utils.DrawInvBG(Main.spriteBatch, new Rectangle((int)pos.X - 10, (int)pos.Y - 10, (int)width + 20, (int)height + 20), new Color(25, 100, 55) * 0.85f);
                Terraria.Utils.DrawBorderString(Main.spriteBatch, value, pos, Color.White);
                pos.Y += ChatManager.GetStringSize(font, value, Vector2.One).Y + 16;
            }
        }
        return base.PreDrawTooltip(item, lines, ref x, ref y);
    }
    public override void SaveData(Item item, TagCompound tag)
    {
        tag["VariantType"] = VariantType;
    }
    public override void LoadData(Item item, TagCompound tag)
    {
        VariantType = tag.Get<short>("VariantType");
    }
}

