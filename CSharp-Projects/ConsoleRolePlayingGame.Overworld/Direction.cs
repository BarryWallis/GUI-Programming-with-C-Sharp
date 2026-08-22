namespace ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Represents the cardinal directions available for movement on the overworld map.
/// </summary>
public enum Direction
{
    /// <summary>Move upward (decreasing Y).</summary>
    North,
    /// <summary>Move downward (increasing Y).</summary>
    South,
    /// <summary>Move right (increasing X).</summary>
    East,
    /// <summary>Move left (decreasing X).</summary>
    West
}