namespace ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Represents an immutable 2-D integer coordinate on the overworld map.
/// </summary>
/// <param name="X">The horizontal map coordinate.</param>
/// <param name="Y">The vertical map coordinate.</param>
public record Position(int X, int Y)
{
    /// <summary>
    /// Returns a new <see cref="Position"/> shifted one step in the given <paramref name="direction"/>.
    /// </summary>
    /// <param name="direction">The direction to move.</param>
    /// <returns>A new <see cref="Position"/> one step in the requested direction.</returns>
    public Position Move(Direction direction) => direction switch
    {
        Direction.North => this with { Y = Y - 1 },
        Direction.South => this with { Y = Y + 1 },
        Direction.East => this with { X = X + 1 },
        Direction.West => this with { X = X - 1 },
        _ => this
    };
}