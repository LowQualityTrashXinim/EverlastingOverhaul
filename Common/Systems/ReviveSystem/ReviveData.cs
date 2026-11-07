namespace EverlastingOverhaul.Common.Systems.ReviveSystem;

public class ReviveData {
	/// <summary>
	/// The result of the chance calculation for this revive.
	/// </summary>
	public bool Chance;

	/// <summary>
	/// Whether this revive has already been used by
	/// the player.
	/// </summary>
	public bool Used;

	/// <summary>
	/// Clones this data object. Use memberwise clone
	/// for all the value types.
	/// </summary>
	public ReviveData Clone() {
		return (ReviveData)MemberwiseClone();
	}
}
