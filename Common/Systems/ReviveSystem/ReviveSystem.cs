using System.Collections.Generic;
using Terraria.ModLoader;

namespace EverlastingOverhaul.Common.Systems.ReviveSystem;

public class ReviveSystem : ModSystem
{

    /// <summary>
    /// The list that holds all the Revive objects
    /// </summary>
    public static List<Revive> Revives { get; private set; }

    /// <summary>
    /// The total amount of Revives that are loaded
    /// </summary>
    public static int Count => Revives.Count;

    /// <summary>
    /// Adds a revive object to the list.
    /// </summary>
    internal static int Add(Revive revive)
    {
        Revives ??= [];

        Revives.Add(revive);
        return Revives.Count - 1;
    }

    /// <summary>
    /// Tries to get the given revive from the list.
    /// Returns whether it was successful, use the
    /// out revive only when this returned true. 
    /// </summary>
    public static bool TryGet(int type, out Revive revive)
    {
        revive = Get(type);
        return revive != null;
    }

    /// <summary>
    /// Gets the revive on the given type from
    /// the revive list. returns null if the
    /// type is invalid.
    /// </summary>
    public static Revive Get(int type)
    {
        if (type < 0 || type >= Count)
        {
            return null;
        }

        return Revives[type];
    }

    /// <summary>
    /// When the mod is loaded initialize the revive list.
    /// The Revive ModType may load before this system, so
    /// only create a new list if it's actually not set yet. 
    /// </summary>
    public override void Load()
    {
        Revives ??= [];
    }

    /// <summary>
    /// When the mod is unloaded clear the revive list.
    /// </summary>
    public override void Unload()
    {
        Revives = null;
    }
}
