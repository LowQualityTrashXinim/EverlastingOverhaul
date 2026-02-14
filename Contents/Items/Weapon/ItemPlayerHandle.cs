using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using EverlastingOverhaul.Common.Global;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Contents.Items.Weapon.RangeSynergyWeapon.Annihiliation;
using EverlastingOverhaul.Texture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;
using EverlastingOverhaul.Common.Global.Mechanic.OutroEffect;
using EverlastingOverhaul.Common.Global.Prefixes;

namespace EverlastingOverhaul.Contents.Items.Weapon {
	public struct SynergyBonus {
		public int ItemID;
		public bool Active;
		public string Tooltip = "";

		public SynergyBonus(int id) {
			ItemID = id;
		}
		public SynergyBonus(int id, string tooltip) {
			ItemID = id;
			Tooltip = tooltip;
		}
	}
	/// <summary>
	/// This is synergy bonus system, this system will automatically handle most of the bonus action for you<br/>
	/// No need to manual set, nor anything, you only need to check whenever or not if a bonus is active or not
	/// </summary>
	public class SynergyBonus_System : ModSystem {
		public static Dictionary<int, List<SynergyBonus>> Dictionary_SynergyBonus = new();
		public override void Load() {
			Dictionary_SynergyBonus = new();
		}
		public override void Unload() {
			Dictionary_SynergyBonus = null;
		}
		public static void Add_SynergyBonus(int SynergyItemID, int ItemID, string tooltip = "") {
			if (Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				if (Dictionary_SynergyBonus[SynergyItemID].Select(b => b.ItemID).ToArray().Contains(ItemID)) {
					return;
				}
				Dictionary_SynergyBonus[SynergyItemID].Add(new(ItemID, tooltip));
				return;
			}
			Dictionary_SynergyBonus.Add(SynergyItemID, new() { { new(ItemID, tooltip) } });
		}
		/// <summary>
		/// Check if the synergy bonus is active or not<br/>
		/// <b>Note :</b> If you are checking a item group, check the key item instead
		/// </summary>
		/// <param name="SynergyItemID"></param>
		/// <param name="ItemID"></param>
		/// <returns></returns>
		public static bool Check_SynergyBonus(int SynergyItemID, int ItemID) {
			if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				return false;
			}
			for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
				SynergyBonus bonus = Dictionary_SynergyBonus[SynergyItemID][i];
				if (bonus.ItemID == ItemID) {
					return bonus.Active;
				}
			}
			return false;
		}
		public static string Get_SynergyBonusTooltip(int SynergyItemID, int itemID) {
			if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				return "Synergy item not found !";
			}
			for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
				SynergyBonus bonus = Dictionary_SynergyBonus[SynergyItemID][i];
				if (bonus.ItemID == itemID) {
					return bonus.Tooltip;
				}
			}
			return "Synergy bonus item not found !";
		}
		public static void Write_SynergyTooltip(ref List<TooltipLine> lines, SynergyModItem moditem, int itemID) {
			int SynergyItemID = moditem.Type;
			if (Main.LocalPlayer.HeldItem.type != moditem.Type) {
				return;
			}
			if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				return;
			}
			SynergyBonus bonus = new();
			for (int i = 0; i < Dictionary_SynergyBonus[SynergyItemID].Count; i++) {
				if (Dictionary_SynergyBonus[SynergyItemID][i].ItemID == itemID) {
					bonus = Dictionary_SynergyBonus[SynergyItemID][i];
				}
			}
			if (bonus.Active)
				lines.Add(new(moditem.Mod, moditem.Set_TooltipName(itemID), bonus.Tooltip));
		}
		/// <summary>
		/// return item id of synergy item
		/// </summary>
		/// <param name="SynergyItemID"></param>
		/// <returns></returns>
		public static int[] Get_SynergyBonus(int SynergyItemID) {
			if (!Dictionary_SynergyBonus.ContainsKey(SynergyItemID)) {
				return null;
			}
			else {
				return Dictionary_SynergyBonus[SynergyItemID].Select(sy => sy.ItemID).ToArray();
			}
		}

		public bool GodAreEnraged = false;
		public int CooldownCheck = 999;
		private void SynergyEnergyCheckPlayer(Player player) {
			int synergyCounter = 0;
			synergyCounter += player.CountItem(ModContent.ItemType<SynergyEnergy>(), 2);
			synergyCounter += player.inventory.Where(itemInv => itemInv.ModItem is SynergyModItem).Count();
			int maxCount = NPC.GetActivePlayerCount() + 1;
			if (synergyCounter >= maxCount) {
				GodAreEnraged = true;
			}
		}
		public override void PostUpdateWorld() {
		}
	}
	/// <summary>
	///This mod player should hold all the logic require for the item, if the item is shooting out the projectile, it should be doing that itself !<br/>
	///Same with projectile unless it is a vanilla projectile then we can refer to global projectile<br/>
	///This should only hold custom bool or data that we think should be hold/use/transfer<br/>
	/// </summary>
	public class PlayerSynergyItemHandle : ModPlayer {
		public bool SynergyBonusBlock = false;
		public int SynergyBonus = 0;

		public int Annihiliation_Counter = 0;

		public override void ResetEffects() {
			SynergyBonus = 0;
			SynergyBonusBlock = false;

			if (Player.HeldItem.type != ModContent.ItemType<Annihiliation>()) {
				Annihiliation_Counter = 0;
			}
			if (!ModItemLib.SynergyItem.Select(i => i.type).Contains(Player.HeldItem.type)) {
				return;
			}
			int synergyItem = Player.HeldItem.type;
			if (!SynergyBonus_System.Dictionary_SynergyBonus.ContainsKey(synergyItem)) {
				return;
			}
			int SynergyBonusLength = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem].Count;
			for (int l = 0; l < SynergyBonusLength; l++) {
				int itemIDBonus = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l].ItemID;
				bool HasItem = Player.HasItem(itemIDBonus);
				if (HasItem) {
					SynergyBonus++;
				}
				SynergyBonus bonus = SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l];
				bonus.Active = HasItem;
				SynergyBonus_System.Dictionary_SynergyBonus[synergyItem][l] = bonus;
			}
		}
	}
	public class WorldVaultSystem : ModSystem {
		public static short Set_Variant = -1;
		private static Dictionary<int, List<ModVariant>> variantlist = new();
		public static short Register(ModVariant variant) {
			ModTypeLookup<ModVariant>.Register(variant);
			if (variantlist.ContainsKey(variant.ItemType)) {
				variantlist[variant.ItemType].Add(variant);
			}
			else {
				variantlist.Add(variant.ItemType, new() { variant });
			}
			return (short)(variantlist[variant.ItemType].Count - 1);
		}
		public static ModVariant GetVariant(int ItemType, short variant) {
			if (variantlist.ContainsKey(ItemType)) {
				foreach (var item in variantlist[ItemType]) {
					if (item.Variant == variant) {
						return item;
					}
				}
				return null;
			}
			else {
				return null;
			}
		}
	}
	public abstract class ModVariant : ModType {
		public short Variant = 0;
		public int ItemType = 0;
		public static short GetVariantType<T>() where T : ModVariant => ModContent.GetInstance<T>().Variant;
		protected sealed override void Register() {
			SetStaticDefaults();
			Variant = WorldVaultSystem.Register(this);
		}
		public virtual void SetDefault(Item item) { }
		public virtual void Shoot(Item item, Player player, IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) { }
		public virtual void UpdateInv(Item item, Player player) { }
	}
	/// <summary>
	/// This class hold mainly tooltip information<br/>
	/// However this doesn't handle overhaul information
	/// </summary>
	public class GlobalItemHandle : GlobalItem {
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
		public short VariantType = -1;
		public int ItemLevel = 0;
		public bool IsASword = false;
		public int OutroEffect_type = -1;
		public override void SetDefaults(Item entity) {
			if (WorldVaultSystem.Set_Variant != -1) {
				VariantType = WorldVaultSystem.Set_Variant;
				var variant = WorldVaultSystem.GetVariant(entity.type, VariantType);
				if (variant != null) {
					variant.SetDefault(entity);
				}
				WorldVaultSystem.Set_Variant = 0;
			}
		}
		public override bool CanUseItem(Item item, Player player) {
			return base.CanUseItem(item, player);
		}
		public override void HoldItem(Item item, Player player) {
			UpdateCriticalDamage = 0;
			if (VariantType != -1) {
				var variant = WorldVaultSystem.GetVariant(item.type, VariantType);
				if (variant != null) {
					variant.UpdateInv(item, player);
				}
			}
		}
		public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if (VariantType != -1) {
				var variant = WorldVaultSystem.GetVariant(item.type, VariantType);
				if (variant != null) {
					variant.Shoot(item, player, source, position, velocity, type, damage, knockback);
				}
			}
			return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		public float CriticalDamage = 0;
		public float UpdateCriticalDamage;
		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage) {
			if (ItemLevel <= 0) {
				return;
			}
			damage += .02f * ItemLevel;
			damage.Base += (int)(ItemLevel * .5f);
		}
		public override void ModifyWeaponCrit(Item item, Player player, ref float crit) {
			if (ItemLevel <= 0) {
				return;
			}
			crit += ItemLevel / 3;
			UpdateCriticalDamage += .05f * (ItemLevel / 4);
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			//tooltips.Add(new(Mod, "Debug", $"Item width : {item.width} | height {item.height}"));
			if (item.IsAWeapon(true)) {
				for (int i = 0; i < tooltips.Count; i++) {
					TooltipLine line = tooltips[i];
					if (tooltips[i].Name == "ItemName") {
						if (ItemLevel > 0) {
							tooltips[i].Text = $"+{ItemLevel} {tooltips[i].Text}";
						}
					}
					if (line.Name == "CritChance") {
						tooltips.Insert(i + 1, new(Mod, "CritDamage", $"{Math.Round(CriticalDamage, 2) * 100}% bonus critical damage"));
						tooltips.Insert(i + 2, new(Mod, "ArmorPenetration", $"{item.ArmorPenetration} Armor penetration"));
					}
					else if (line.Name == "Damage") {
						line.Text = line.Text + $" | Base : {item.OriginalDamage}";
					}
					else if (line.Name == "Knockback") {
						line.Text = line.Text + $" | Base : {Math.Round(ContentSamples.ItemsByType[item.type].knockBack, 2)} | Modified : {Math.Round(Main.LocalPlayer.GetWeaponKnockback(item), 2)}";
					}
				}
			}
			ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
			if (item.ModItem != null) {
				if (ExtraInfo) {
					if (!moddedplayer.Shift_Option()) {
						tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for more infomation]") { OverrideColor = Color.Gray });
					}
				}
			}
			else {
				if (item.IsAWeapon()) {
					if (!OutroEffectSystem.Has_WeaponTag(item.type)) {
						ModContent.GetInstance<OutroEffectSystem>().GetWeaponTag(item.type);
					}
					else {
						if (Main.SmartCursorIsUsed && !moddedplayer.Shift_Option()) {
							tooltips.Add(new TooltipLine(Mod, "Shift_Info", "[Press shift for more infomation]") { OverrideColor = Color.Gray });
						}
					}
				}
			}
			if (item.ModItem == null) {
				return;
			}
			if (item.ModItem.Mod != Mod) {
				return;
			}
			TooltipLine NameLine = tooltips.Where(t => t.Name == "ItemName").FirstOrDefault();
			if (DebugItem && NameLine != null) {
				NameLine.Text += " [Debug]";
				NameLine.OverrideColor = Color.MediumPurple;
				return;
			}
			if (AdvancedBuffItem && NameLine != null) {
				NameLine.Text += " [Advanced]";
			}
		}
		public override void UpdateInventory(Item item, Player player) {
			if (player.GetModPlayer<SynergyModPlayer>().JustSwitched) {
				WeaponEffect eff = OutroEffectSystem.GetWeaponEffect(OutroEffect_type);
				if (eff != null) {
					player.GetModPlayer<WeaponEffect_ModPlayer>().Add_WeaponEffect(eff);
				}
			}
			if (++Counter >= int.MaxValue / 10) {
				Counter = 0;
			}
			if (item.prefix == ModContent.PrefixType<Chaotic>() && Counter % 100 == 0) {
				Prefix_ChaoticEffect(item);
			}
			else if (item.prefix == ModContent.PrefixType<Unstable>() && Counter % 600 == 0) {
				Prefix_UnstableEffect(item);
			}
		}
		public void Prefix_ChaoticEffect(Item item) {
			if (item.damage < item.OriginalDamage / 2) {
				item.damage += Main.rand.Next(0, 2);
			}
			else {
				item.damage += Main.rand.Next(-1, 2);
			}
			if (item.crit <= 1) {
				item.crit += Main.rand.Next(0, 2);
			}
			else {
				item.crit += Main.rand.Next(-1, 2);
			}
			if (CriticalDamage <= -.5f) {
				CriticalDamage += Main.rand.NextFloat(0, .2f);
			}
			else {
				CriticalDamage += Main.rand.NextFloat(-.2f, .2f);
			}
			item.knockBack += Main.rand.NextFloat(-1, 1);
		}
		public void Prefix_UnstableEffect(Item item) {
			item.SetDefaults(Main.rand.NextFromHashSet(ModItemLib.List_Weapon).type);
			if (!Main.rand.NextBool(1000)) {
				item.prefix = ModContent.PrefixType<Unstable>();
			}
		}
		public override bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y) {
			//Prevent possible conflict, basically hardcoding to make it so that it only work for item belong to this mod
			string value = null;
			if (!Main.SmartCursorIsUsed) {
				if (item.ModItem != null) {
					if (item.ModItem.Mod.Name != Mod.Name) {
						return true;
					}
					if (ExtraInfo) {
						value = ModUtils.LocalizationText("Items", $"{item.ModItem.Name}.ExtraInfo");
					}
				}
			}
			else {
				value = "This weapon is classified as following: \n";
				value += ModContent.GetInstance<OutroEffectSystem>().GetWeaponTag(item.type);
				if (OutroEffect_type != -1) {
					WeaponEffect ef = OutroEffectSystem.GetWeaponEffect(OutroEffect_type);
					if (ef != null) {
						value += $"\n{ef.DisplayName}\n- {ef.ModifyTooltip()}";
					}
				}
			}
			if (value == null) {
				return base.PreDrawTooltip(item, lines, ref x, ref y); ;
			}
			ModdedPlayer moddedplayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
			if (moddedplayer.Shift_Option()) {
				float width;
				float height = -16;
				Vector2 pos;
				DynamicSpriteFont font = FontAssets.MouseText.Value;
				if (Main.MouseScreen.X < Main.screenWidth / 2) {
					string widest = lines.OrderBy(n => ChatManager.GetStringSize(font, n.Text, Vector2.One).X).Last().Text;
					width = ChatManager.GetStringSize(font, widest, Vector2.One).X;
					pos = new Vector2(x, y) + new Vector2(width + 30, 0);
				}
				else {
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
		public override bool? UseItem(Item item, Player player) {
			//if (AdvancedBuffItem && !UniversalSystem.CanAccessContent(player, UniversalSystem.BOSSRUSH_MODE)) {
			//	player.AddBuff(ModContent.BuffType<Drawback>(), ModUtils.ToMinute(6));
			//}
			return base.UseItem(item, player);
		}
		public override void SaveData(Item item, TagCompound tag) {
			tag["VariantType"] = VariantType;
		}
		public override void LoadData(Item item, TagCompound tag) {
			VariantType = tag.Get<short>("VariantType");
		}
	}
	public abstract class SynergyModItem : ModItem {
		public virtual void OutroAttack(Player player) {

		}
		public string Set_TooltipName(int ItemID) => $"{Name}_{ContentSamples.ItemsByType[ItemID].Name}";
		public sealed override void SetStaticDefaults() {
			ItemID.Sets.ShimmerTransformToItem[Item.type] = ModContent.ItemType<SynergyEnergy>();
			CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(50, 100, 100) });
			Synergy_SetStaticDefaults();
		}
		public virtual void Synergy_SetStaticDefaults() { }
		public ColorInfo CustomColor = new ColorInfo(new List<Color> { new Color(100, 255, 255), new Color(100, 150, 150) });
		public override sealed void ModifyTooltips(List<TooltipLine> tooltips) {
			ModifySynergyToolTips(ref tooltips, Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>());
			if (CustomColor != null) {
				tooltips.Where(t => t.Name == "ItemName").FirstOrDefault().OverrideColor = CustomColor.MultiColor(5);
			}
		}
		public override sealed void ModifyWeaponCrit(Player player, ref float crit) {
			Synergy_ModifyWeaponCrit(player, ref crit);
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			crit += 4 * modplayer.SynergyBonus;
		}
		public virtual void Synergy_ModifyWeaponCrit(Player player, ref float crit) { }
		public override sealed void ModifyWeaponDamage(Player player, ref StatModifier damage) {
			Synergy_ModifyWeaponDamage(player, ref damage);
			float damageIncreasement = 0;
			float damageMultiplier = 0;
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (modplayer.SynergyBonus > 0) {
				damageMultiplier += 0.025f * modplayer.SynergyBonus;
			}
			else {
				damageMultiplier += 0.01f;
			}
			for (int i = 0; player.inventory.Length > 0; i++) {
				if (i > 50) {
					break;
				}
				Item item = player.inventory[i];
				if (!item.IsAWeapon() || item == Item || item.ModItem is SynergyModItem) {
					continue;
				}
				damageIncreasement += player.inventory[i].damage * damageMultiplier;
			}
			damage += damageIncreasement;
		}
		public virtual void Synergy_ModifyWeaponDamage(Player player, ref StatModifier damage) { }
		public virtual void ModifySynergyToolTips(ref List<TooltipLine> tooltips, PlayerSynergyItemHandle modplayer) { }
		public override sealed void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			ModifySynergyShootStats(player, player.GetModPlayer<PlayerSynergyItemHandle>(), ref position, ref velocity, ref type, ref damage, ref knockback);
		}
		public override sealed void UpdateInventory(Player player) {
			base.UpdateInventory(player);
			//Very funny that hold item happen after ModifyWeaponDamage
			//This probably will tank our mod performance, but well, it is what it is
			PlayerSynergyItemHandle modplayer = player.GetModPlayer<PlayerSynergyItemHandle>();
			if (player.HeldItem == Item) {
				HoldSynergyItem(player, modplayer);
			}
			SynergyUpdateInventory(player, modplayer);
		}
		public virtual void SynergyUpdateInventory(Player player, PlayerSynergyItemHandle modplayer) {

		}
		public virtual void ModifySynergyShootStats(Player player, PlayerSynergyItemHandle modplayer, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {

		}
		/// <summary>
		/// You should use this to set condition, the condition must be pre set in <see cref="PlayerSynergyItemHandle"/> and then check condition in here
		/// </summary>
		/// <param name="player"></param>
		/// <param name="modplayer"></param>
		public virtual void HoldSynergyItem(Player player, PlayerSynergyItemHandle modplayer) { }
		public override sealed bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			SynergyShoot(player, player.GetModPlayer<PlayerSynergyItemHandle>(), source, position, velocity, type, damage, knockback, out bool CanShootItem);
			return CanShootItem;
		}
		public virtual void SynergyShoot(Player player, PlayerSynergyItemHandle modplayer, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, out bool CanShootItem) { CanShootItem = true; }
		public override sealed void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			base.OnHitNPC(player, target, hit, damageDone);
			OnHitNPCSynergy(player, player.GetModPlayer<PlayerSynergyItemHandle>(), target, hit, damageDone);
		}
		public virtual void OnHitNPCSynergy(Player player, PlayerSynergyItemHandle modplayer, NPC target, NPC.HitInfo hit, int damageDone) { }

		private int countX = 0;
		private float positionRotateX = 0;
		private int rotate = 0;
		private void PositionHandle() {
			if (positionRotateX < 3.5f && countX == 1) {
				positionRotateX += .2f;
			}
			else {
				countX = -1;
			}
			if (positionRotateX > 0 && countX == -1) {
				positionRotateX -= .2f;
			}
			else {
				countX = 1;
			}
		}
		Color auraColor;
		private void ColorHandle() {
			switch (Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus) {
				case 1:
					auraColor = new Color(255, 50, 0, 30);
					break;
				case 2:
					auraColor = new Color(255, 255, 0, 30);
					break;
				case 3:
					auraColor = new Color(0, 255, 255, 30);
					break;
				default:
					auraColor = new Color(255, 255, 255, 30);
					break;
			}
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
			PositionHandle();
			ColorHandle();
			rotate = ModUtils.Safe_SwitchValue(rotate, (int)(MathHelper.Pi * 1000));
			if (ItemID.Sets.AnimatesAsSoul[Type] || Main.LocalPlayer.GetModPlayer<PlayerSynergyItemHandle>().SynergyBonus < 1 || Main.LocalPlayer.HeldItem.type != Type) {
				return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
			}
			int[] synegyB = SynergyBonus_System.Get_SynergyBonus(Type);
			if (synegyB != null) {
				int len = synegyB.Length;
				for (int i = 0; i < len; i++) {
					if (!SynergyBonus_System.Dictionary_SynergyBonus[Type][i].Active) {
						continue;
					}
					int type = synegyB[i];
					Main.instance.LoadItem(type);
					Texture2D synergyBonus = TextureAssets.Item[type].Value;
					spriteBatch.Draw(synergyBonus, position + Vector2.One.RotatedBy(MathHelper.ToRadians(rotate) + MathHelper.Pi * MathHelper.Lerp(0, 1, i / (len - 1f))) * 10, synergyBonus.Frame(), Color.White, 0, synergyBonus.Size() * .5f, .55f, SpriteEffects.None, 0);
				}
			}
			Main.instance.LoadItem(Type);
			Texture2D texture = TextureAssets.Item[Type].Value;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, position + new Vector2(1.5f, 1.5f), frame, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(1.5f, -1.5f), frame, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, 1.5f), frame, auraColor, 0, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, position + new Vector2(-1.5f, -1.5f), frame, auraColor, 0, origin, scale, SpriteEffects.None, 0);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}
	public abstract class SynergyBuff : ModBuff {
		public override string Texture => ModTexture.MissingTexture_Default;
		public override sealed void SetStaticDefaults() {
			base.SetStaticDefaults();
			SynergySetStaticDefaults();
		}
		public virtual void SynergySetStaticDefaults() {

		}
		public override sealed void Update(Player player, ref int buffIndex) {
			base.Update(player, ref buffIndex);
			UpdatePlayer(player, ref buffIndex);
		}
		public virtual void UpdatePlayer(Player player, ref int buffIndex) {

		}
		public override sealed void Update(NPC npc, ref int buffIndex) {
			base.Update(npc, ref buffIndex);
			UpdateNPC(npc, ref buffIndex);
		}
		public virtual void UpdateNPC(NPC npc, ref int buffIndex) {

		}
	}
}
