using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.Overworld;

using Shouldly;

namespace ConsoleRolePlayingGame.ConsoleApp.Tests;

/// <summary>
/// Unit tests for <see cref="PlayerParty"/>.
/// </summary>
public class PlayerPartyTests
{
    /// <summary>
    /// Verifies that moving the party in each cardinal direction updates coordinates as expected.
    /// </summary>
    [Theory]
    [InlineData(Direction.North, 0, -1)]
    [InlineData(Direction.South, 0, 1)]
    [InlineData(Direction.East, 1, 0)]
    [InlineData(Direction.West, -1, 0)]
    public void Move_ShiftsPartyPosition(Direction direction, int expectedX, int expectedY)
    {
        PlayerParty party = new();

        party.Move(direction);

        party.MapPosition.ShouldBe(new Position(expectedX, expectedY));
    }

    /// <summary>
    /// Verifies that recording a defeated enemy increments the defeat counter.
    /// </summary>
    [Fact]
    public void EnemyDefeated_IncrementsCounter()
    {
        PlayerParty party = new();

        party.EnemyDefeated();

        party.EnemiesDefeated.ShouldBe(1);
    }
}
