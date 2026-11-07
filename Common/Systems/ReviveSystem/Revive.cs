using Terraria;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Systems.ReviveSystem;

public abstract class Revive : ModType {
	
	/// <summary>
	/// The ID/Type of this Revive in the Revive List.
	/// </summary>
	public int Type { get; private set; }

	/// <summary>
	/// Determines what kind of revive logic is used for
	/// this revive object.
	/// </summary>
	public ReviveType ReviveType { get; protected set; }
	
	/// <summary>
	/// The actual chance value that is used during rolls.
	/// this value should be between 0f and 1f. 20% chance 
	/// should be set as 0.20f.
	/// </summary>
	public float Chance { get; protected set; }

	protected override void Register() {
		SetStaticDefaults();
		Type = ReviveSystem.Add(this);
		ModTypeLookup<Revive>.Register(this);
	}

	/// <summary>
	/// Determines if the revive is currently activated for the
	/// player. Place custom logic here to determine if it is
	/// allowed to be active and revive the player if true.
	/// </summary>
	public abstract bool IsActive(Player player);
	
	/// <summary>
	/// Gets the chance based revive result from the
	/// data object of the player.
	/// </summary>
	public bool GetChanceResult(Player player) {
		return GetData(player).Chance;
	}

	/// <summary>
	/// Checks if this revive was already used. Returns
	/// true when the revive was used.
	/// </summary>
	public bool GetUseResult(Player player) {
		return GetData(player).Used;
	}

	/// <summary>
	/// Gets the player data object holding all the
	/// player specific revive data.
	/// </summary>
	public ReviveData GetData(Player player) {
		// Get the revive player
		var revivePlayer = player.GetModPlayer<RevivePlayer>();

		// Check if the data object for the revive exists
		if (revivePlayer.ReviveData?.TryGetValue(this, out var data) == true) {
			return data;
		}
		
		// Otherwise create a new object
		data = new ReviveData();
		
		// Run on creation
		OnDataCreation(data);
		
		// Make sure the data dictionary is initialized
		revivePlayer.ReviveData ??= [];
		
		// Save the new data on the player
		revivePlayer.ReviveData[this] = data;
		
		// Flag as netUpdate needed
		revivePlayer.NetUpdate = true;
		
		// Return the data object
		return data;
	}

	public virtual void OnDataCreation(ReviveData data) {
		if (ReviveType == ReviveType.Chance) {
			RecalculateChance(data);
		}
	}

	/// <summary>
	/// Set the use flag for this revive
	/// </summary>
	public void Used(Player player) {
		// Get the data object
		var data = GetData(player);
		
		// Save the current value
		bool value = data.Used;
		
		// Modify the value to true
		data.Used = true;
		
		// Check for changes
		if (value != data.Used) {
			player.GetModPlayer<RevivePlayer>().NetUpdate = true;
		}
	}

	/// <summary>
	/// Reset the used flag for this revive.
	/// </summary>
	public void ResetUses(Player player) {
		// Get the data object
		var data = GetData(player);
		
		// Save the current value
		bool value = data.Used;
		
		// Modify the value to false
		data.Used = false;
		
		// Check for changes
		if (value != data.Used) {
			player.GetModPlayer<RevivePlayer>().NetUpdate = true;
		}
	}

	/// <summary>
	/// Recalculates the revive chance result for this player
	/// </summary>
	public void RecalculateChance(Player player) {
		// Get the data object
		var data = GetData(player);
		
		// Save the current value
		bool value = data.Chance;
		
		RecalculateChance(data);
		
		// Check for changes
		if (value != data.Chance) {
			player.GetModPlayer<RevivePlayer>().NetUpdate = true;
		}
	}
	
	/// <summary>
	/// Recalculates the revive chance result for the data object
	/// </summary>
	public void RecalculateChance(ReviveData data) {
		data.Chance = Main.rand.NextFloat() < Chance;
	}

	/// <summary>
	/// Use to do stuff when this revive is used to revive
	/// the given player.
	/// </summary>
	public virtual void OnRevive(Player player) {
		
	}
}

public enum ReviveType {
	Conditional,
	Chance,
	Uses
}


