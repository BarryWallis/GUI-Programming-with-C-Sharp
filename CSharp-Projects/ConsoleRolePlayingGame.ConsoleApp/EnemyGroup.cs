using System.Data;

using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.ConsoleApp;

/// <summary>
/// Represents a group of enemies that pursues the player party on the overworld map.
/// </summary>
/// <param name="position">The initial map position of this enemy group.</param>
public class EnemyGroup(Position position) : IMapEntity
{
    /// <inheritdoc/>
    public Position MapPosition { get; set; } = position;

    /// <inheritdoc/>
    public EntityType EntityType { get; } = EntityType.Enemy;

    /// <summary>
    /// Moves this enemy group one step toward <paramref name="target"/> along the axis
    /// with the greatest remaining distance, provided the destination cell is unoccupied.
    /// </summary>
    /// <param name="target">The map position to move toward.</param>
    /// <param name="map">The world map used to check for occupied positions.</param>
    public void MoveTowards(Position target, WorldMap map)
    {
        if (target == MapPosition)
        {
            return;
        }

        int xDiff = target.X - MapPosition.X;
        int yDiff = target.Y - MapPosition.Y;

        Direction direction = Math.Abs(xDiff) > Math.Abs(yDiff)
                              ? xDiff > 0 ? Direction.East : Direction.West
                              : yDiff > 0 ? Direction.South : Direction.North;

        Position newPosition = MapPosition.Move(direction);
        if (map.Entities.All(e=>e.MapPosition != newPosition || e != this))
        {
            MapPosition = newPosition;
        }
    }
}