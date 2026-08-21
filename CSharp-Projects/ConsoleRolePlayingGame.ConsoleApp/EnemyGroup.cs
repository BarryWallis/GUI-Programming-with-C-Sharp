using System.Data;

using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.ConsoleApp;

public class EnemyGroup(Position position) : IMapEntity
{
    public Position MapPosition { get; set; } = position;

    public EntityType EntityType { get; } = EntityType.Enemy;

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