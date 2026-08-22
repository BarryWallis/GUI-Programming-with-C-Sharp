namespace ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Describes the type of terrain that occupies a map cell.
/// </summary>
public enum TerrainType
{
    /// <summary>Open grassland — the default traversable biome.</summary>
    Grass,
    /// <summary>Shallow coastal water.</summary>
    Water,
    /// <summary>Deep ocean water, further from shore than <see cref="Water"/>.</summary>
    DeepWater,
    /// <summary>Rocky mountain terrain.</summary>
    Mountain,
    /// <summary>Dense forested area.</summary>
    Forest,
    /// <summary>Arid desert terrain.</summary>
    Desert,
}