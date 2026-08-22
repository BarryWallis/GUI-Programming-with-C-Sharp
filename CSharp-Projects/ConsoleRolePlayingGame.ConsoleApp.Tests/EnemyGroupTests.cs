using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.Overworld;

using Shouldly;

namespace ConsoleRolePlayingGame.ConsoleApp.Tests;

/// <summary>
/// Unit tests for <see cref="EnemyGroup"/> movement behavior.
/// </summary>
public class EnemyGroupTests
{
    /// <summary>
    /// Verifies that an enemy moves one step along the axis with the greatest distance to the target.
    /// </summary>
    [Fact]
    public void MoveTowards_MovesAlongTheDominantAxis()
    {
        WorldMap map = new(new MapGenerator());
        EnemyGroup enemy = new(new Position(0, 0));
        map.AddEntity(enemy);

        enemy.MoveTowards(new Position(3, 1), map);

        enemy.MapPosition.ShouldBe(new Position(1, 0));
    }

    /// <summary>
    /// Verifies that an enemy does not move when it is already at the target position.
    /// </summary>
    [Fact]
    public void MoveTowards_DoesNotMoveWhenAlreadyAtTarget()
    {
        WorldMap map = new(new MapGenerator());
        EnemyGroup enemy = new(new Position(2, 2));
        map.AddEntity(enemy);

        enemy.MoveTowards(new Position(2, 2), map);

        enemy.MapPosition.ShouldBe(new Position(2, 2));
    }
}
