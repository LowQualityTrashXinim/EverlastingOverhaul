using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect.Contents;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;

namespace EverlastingOverhaul.Contents.Items.Weapon {
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
        public bool AdvancedBuffItem = false;
        public bool OverrideVanillaEffect = false;
        public int Counter = 0;
        public bool IsASword = false;
        public int OutroEffect_type = -1;
        public int InventoryWhoAmI = -1;
        public override GlobalItem NewInstance(Item target)
        {
            if (target.TryGetGlobalItem(out GlobalItemHandle handler))
            {
                handler.OutroEffect_type = -1;
                handler.InventoryWhoAmI = -1;
                handler.Counter = 0;
            }
            return base.NewInstance(target);
        }
        public override void SetDefaults(Item entity)
        {
            if (OutroEffect_type == -1)
            {
                OutroEffect_type = OutroEffect.GetOutroEffectType<OutroEffect_None>();
            }
            entity.prefix = 0;
        }
        public override bool CanUseItem(Item item, Player player)
        {
            return base.CanUseItem(item, player);
        }
        public override void HoldItem(Item item, Player player)
        {
            UpdateCriticalDamage = 0;
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
        public float CriticalDamage = 0;
        public float UpdateCriticalDamage;
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
            Player player = Main.LocalPlayer;
            ModdedPlayer moddedplayer = player.GetModPlayer<ModdedPlayer>();
            if (item.ModItem != null)
            {
                if (ExtraInfo)
                {
                    if (!moddedplayer.Shift_Option())
                    {
                        tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for more infomation]") { OverrideColor = Color.Gray });
                    }
                }
            }
            else
            {
                if (item.IsAWeapon())
                {
                    if (!OutroEffectSystem.Has_WeaponTag(item.type))
                    {
                        ModContent.GetInstance<OutroEffectSystem>().GetWeaponTag(item.type);
                    }
                    else
                    {
                        if (!moddedplayer.Shift_Option())
                        {
                            tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for weapon tag information]") { OverrideColor = Color.Gray });
                        }
                        else
                        {
                            string value = "This weapon is classified as following: \n";
                            value += ModContent.GetInstance<OutroEffectSystem>().GetWeaponTag(item.type);
                            if (OutroEffect_type != -1)
                            {
                                OutroEffect ef = OutroEffectSystem.GetOutroEffect(OutroEffect_type);
                                if (ef != null && ef.Type != OutroEffect.GetOutroEffectType<OutroEffect_None>())
                                {
                                    value += $"\nOutro effect: \n{ef.DisplayName}\n- {ef.ModifyTooltip()}";
                                }
                            }
                            tooltips.Add(new TooltipLine(Mod, "Shift_Info", value) { OverrideColor = new Color(255, 255, 0, 0) });
                        }
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
            if (AdvancedBuffItem && NameLine != null)
            {
                NameLine.Text += " [Advanced]";
            }
        }
        public override void PostUpdate(Item item)
        {
        }
        public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            //Prevent possible conflict, basically hardcoding to make it so that it only work for item belong to this mod
            string value = null;
            if (item.ModItem != null)
            {
                if (item.ModItem.Mod.Name != Mod.Name)
                {
                    return true;
                }
                if (ExtraInfo)
                {
                    value = ModUtils.LocalizationText("Items", $"{item.ModItem.Name}.ExtraInfo");
                }
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
                Utils.DrawInvBG(Main.spriteBatch, new Rectangle((int)pos.X - 10, (int)pos.Y - 10, (int)width + 20, (int)height + 20), new Color(25, 100, 55) * 0.85f);
                Utils.DrawBorderString(Main.spriteBatch, value, pos, Color.White);
                pos.Y += ChatManager.GetStringSize(font, value, Vector2.One).Y + 16;
            }
            return base.PreDrawTooltip(item, lines, ref x, ref y);
        }
        public override bool? UseItem(Item item, Player player)
        {
            //if (AdvancedBuffItem && !UniversalSystem.CanAccessContent(player, UniversalSystem.BOSSRUSH_MODE)) {
            //	player.AddBuff(ModContent.BuffType<Drawback>(), ModUtils.ToMinute(6));
            //}
            return base.UseItem(item, player);
        }
    }
}
