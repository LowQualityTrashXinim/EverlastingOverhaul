using Terraria;
using System.IO;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using EverlastingOverhaul.Common.Utils;
using EverlastingOverhaul.Common.General;

namespace EverlastingOverhaul.Common.Global
{
	/// <summary>
	/// This class hold random general information, not recommend to look into this class
	/// </summary>
	class ModdedPlayer : ModPlayer {
		public int EnchantingEnable = 0;
		public int SkillEnable = 0;

		public int HowManyBossIsAlive = 0;
		public bool ItemIsUsedDuringBossFight = false;

		public bool Hold_Shift = false;
		public bool Press_Shift = false;
		public bool Pressed_Shift = false;
		public bool Shift_Option() {
			if (ModContent.GetInstance<RogueLikeConfig>().HoldShift) {
				return Hold_Shift;
			}
			else {
				return Press_Shift;
			}
		}
		private Item starterItem = null;
		public bool UseOnly1ItemSinceTheStartOfTheGame(int type = 0) {
			if (starterItem == null) {
				return false;
			}
			if (starterItem != Player.HeldItem) {
				return false;
			}
			if (type != 0) {
				if (starterItem.type == type) {
					return true;
				}
			}
			return true;
		}
		public override void ProcessTriggers(TriggersSet triggersSet) {
			if (Main.playerInventory) {
				Hold_Shift = triggersSet.SmartSelect;
				if (triggersSet.SmartSelect) {
					if (!Pressed_Shift) {
						Press_Shift = !Press_Shift;
					}
					Pressed_Shift = true;
				}
				else {
					Pressed_Shift = false;
				}
			}
		}
		public override void OnEnterWorld() {
			Mod.Reflesh_GlobalItem(Player);
			Player.itemAnimation = 0;
			if (Player.HeldItem != null && Player.HeldItem.IsAWeapon()) {
				Player.itemAnimationMax = Player.HeldItem.useAnimation;
			}
            Main.NewText("Welcome to Everlasting overhaul !");
            Main.NewText("This mod will only focus on overhauling vanilla weapon to Everlasting balancing");
            Main.NewText("Expect many weapon to be terribly broken or unbalanced");
            Main.NewText("We are looking for playtester for this mod so if you think you are up to the task, join our discord server !");
        }
		public override void PreUpdate() {
			if (starterItem == null) {
				var item = Player.HeldItem;
				if (item.IsAWeapon()) {
					starterItem = item;
				}
			}
		}
		public int amountOfTimeGotHit = 0;
		public bool AllowForAchievement = true;
		public override void OnHurt(Player.HurtInfo info) {
			if (ModUtils.IsAnyVanillaBossAlive())
				amountOfTimeGotHit++;
		}
		public override void Initialize() {
			EnchantingEnable = 0;
			SkillEnable = 0;
			AllowForAchievement = true;
		}
		public override void SaveData(TagCompound tag) {
			tag["EnchantingEnable"] = EnchantingEnable;
			tag["SkillEnable"] = SkillEnable;
			tag["AllowForAchievement"] = AllowForAchievement;
		}
		public override void LoadData(TagCompound tag) {
			if (tag.TryGet("EnchantingEnable", out int value1)) {
				EnchantingEnable = value1;
			}
			if (tag.TryGet("SkillEnable", out int value2)) {
				SkillEnable = value2;
			}
			if (tag.TryGet("AllowForAchievement", out bool all)) {
				AllowForAchievement = all;
			}
		}
		public void ReceivePlayerSync(BinaryReader reader) {
			EnchantingEnable = reader.ReadInt32();
			SkillEnable = reader.ReadInt32();
			AllowForAchievement = reader.ReadBoolean();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
			var clone = (ModdedPlayer)targetCopy;
			clone.EnchantingEnable = EnchantingEnable;
			clone.SkillEnable = SkillEnable;
			clone.AllowForAchievement = AllowForAchievement;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
			var clone = (ModdedPlayer)clientPlayer;
			if (EnchantingEnable != clone.EnchantingEnable
				|| SkillEnable != clone.SkillEnable
				|| AllowForAchievement != clone.AllowForAchievement) SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}
	}
}