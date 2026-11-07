using Microsoft.Xna.Framework;
using EverlastingOverhaul.Common.Systems.ObjectSystem;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using EverlastingOverhaul.Common.Systems.ObjectSystem.DataStructures;

namespace EverlastingOverhaul.Common.Systems.ReviveSystem;

public class RevivePlayer : ModPlayer {

	/// <summary>
	/// A dictionary that holds all the revive data of this player.
	/// </summary>
	public Dictionary<Revive, ReviveData> ReviveData = [];

	/// <summary>
	/// Tracks whether the chances need to be sent on the network.
	/// Set to true during Revive actions.
	/// </summary>
	public bool NetUpdate;

	/// <summary>
	/// List holding all the revive consumables that the player
	/// has. This list is cleared during <see cref="ModPlayer.ResetEffects"/>.
	/// Add item references during <see cref="ModPlayer.UpdateEquips"/> or
	/// during item updates. Used during <see cref="ModPlayer.PreKill"/>.
	/// </summary>
	public List<Item> ReviveConsumables = [];

	/// <summary>
	/// Called when a player object is cloned.
	/// </summary>
	public override ModPlayer Clone(Player newEntity) {
		var inst = base.Clone(newEntity);

		if (inst is not RevivePlayer revivePlayer) {
			return inst;
		}

		// Copy all the item references into a new list
		revivePlayer.ReviveConsumables = new List<Item>(ReviveConsumables);

		// Copy the data objects into a new dictionary
		revivePlayer.ReviveData = [];
		foreach (var revive in ReviveData) {
			revivePlayer.ReviveData.Add(revive.Key, revive.Value.Clone());
		}

		return inst;
	}

	public override void ResetEffects() {
		// Clear all the revive consumable references
		ReviveConsumables?.Clear();
	}

	public override void OnEnterWorld() {
		// Recalculate all the chances
		foreach (var revive in ReviveSystem.Revives) {
			if (revive.ReviveType == ReviveType.Chance) {
				revive.RecalculateChance(Player);
			}
		}
	}

	public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource) {
		bool CanDie = true;
		// 1) evaluate all the chance based revives
		foreach (var revive in ReviveSystem.Revives) {
			if (!revive.IsActive(Player)) {
				continue;
			}
			if (revive.ReviveType == ReviveType.Chance) {
				if (revive.GetChanceResult(Player)) {
					revive.OnRevive(Player);
					if (Player.whoAmI == Main.myPlayer) {
						revive.RecalculateChance(Player);
					}
					CanDie = false;
					break;
				}
				else {
					if (Player.whoAmI == Main.myPlayer) {
						revive.RecalculateChance(Player);
					}
				}
			}

		}

		// 2) use all the conditional revives
		foreach (var revive in ReviveSystem.Revives) {

			if (revive.ReviveType != ReviveType.Conditional
				|| !revive.IsActive(Player)) {
				continue;
			}

			revive.OnRevive(Player);

			return false;
		}

		// 3) use all the consumable revives
		foreach (var revive in ReviveSystem.Revives) {
			if (revive.ReviveType != ReviveType.Uses
				|| !revive.IsActive(Player)
				|| revive.GetUseResult(Player)) {
				continue;
			}

			revive.OnRevive(Player);

			revive.Used(Player);

			CanDie = false;
			break;
		}

		// 4) use revive item
		if (ReviveConsumables is { Count: > 0 }) {
			var consumable = ReviveConsumables[0];

			ModObject.NewModObject(
				new EntitySource_AccessoryVisual(consumable.type, Player),
				Player.Center,
				Vector2.Zero,
				ModObject.GetModObjectType<AccessoryVisualModObject>());

			consumable.TurnToAir();
			consumable.NetStateChanged();

			ReviveConsumables.RemoveAt(0);

			Player.Heal(Player.statLifeMax2 / 2);

			CanDie = false;
		}

		// 5) no revive triggered
		return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
		SendRevivePacket(toWho, fromWho, true);
		NetUpdate = false;
	}

	public override void SendClientChanges(ModPlayer clientPlayer) {
		if (!NetUpdate) {
			return;
		}

		SendRevivePacket(-1, -1);
		NetUpdate = false;
	}
	public override void CopyClientState(ModPlayer targetCopy) {
		base.CopyClientState(targetCopy);
	}
	public void SendRevivePacket(int toWho, int fromWho, bool sync = false) {
		// Create a new packet
		var packet = Mod.GetPacket();

		// Write the message type
		packet.Write(sync ? (byte)EverlastingOverhaul.MessageType.ReviveSync : (byte)EverlastingOverhaul.MessageType.Revive);

		// Write the player id
		packet.Write((byte)Player.whoAmI);

		// Create a new list to hold all the data
		var flags = new List<bool>();

		// Loop over all the revives
		foreach (var revive in ReviveSystem.Revives) {
			// Get the data object of this revive
			var data = revive.GetData(Player);

			// Create a new bool for saving the data
			bool flag;

			// Determine how to fill the data
			switch (revive.ReviveType) {
				case ReviveType.Conditional: continue;
				case ReviveType.Chance: flag = data.Chance; break;
				case ReviveType.Uses: flag = data.Used; break;
				default: continue;
			}

			// Add the data to the list
			flags.Add(flag);
		}

		// Make sure the list has a count that is divisible by 8
		// (makes the data byte aligned)
		for (int i = 8 - flags.Count % 8; i > 0; i--) {
			flags.Add(false);
		}

		// Write all the flags
		for (int i = 0; i < flags.Count; i += 8) {
			BitsByte data = new();

			for (int j = 0; j < 8; j++) {
				data[j] = flags[i + j];
			}

			packet.Write(data);
		}

		// Send the revive flag packet
		packet.Send(toWho, fromWho);
	}

	public void ReceiveRevivePacket(BinaryReader binaryReader) {
		// Message type and player id were already read!

		// Determine the amount of flags that are received.
		int amount = 0;

		// Loop over all the revives
		foreach (var revive in ReviveSystem.Revives) {
			switch (revive.ReviveType) {
				case ReviveType.Conditional: continue;
				case ReviveType.Chance:
				case ReviveType.Uses: break;
				default: continue;
			}

			amount++;
		}

		// Align to bytes (8 bits)
		amount += 8 - amount % 8;

		// Create List to hold the flags
		var flags = new List<bool>();

		for (int i = 0; i < amount; i += 8) {
			var data = binaryReader.ReadBitsByte();

			for (int j = 0; j < 8; j++) {
				flags.Add(data[j]);
			}
		}

		int index = 0;

		foreach (var revive in ReviveSystem.Revives) {
			// Get the data object of this revive
			var data = revive.GetData(Player);

			// Determine what to write based on the type
			switch (revive.ReviveType) {
				case ReviveType.Conditional: continue;
				case ReviveType.Chance: data.Chance = flags[index]; break;
				case ReviveType.Uses: data.Used = flags[index]; break;
				default: continue;
			}

			// Go to the next index
			index++;
		}
	}
}
