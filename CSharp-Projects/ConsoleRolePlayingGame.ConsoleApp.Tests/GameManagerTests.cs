using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.Overworld;

using Shouldly;

namespace ConsoleRolePlayingGame.ConsoleApp.Tests;

/// <summary>
/// Unit tests for <see cref="GameManager"/>.
/// </summary>
public class GameManagerTests
{
    /// <summary>
    /// Verifies that construction registers the party and seeds the map with the maximum enemy count.
    /// </summary>
    [Fact]
    public void Constructor_AddsPartyAndInitialEnemies()
    {
        PlayerParty party = new() { MapPosition = new Position(4, 6) };
        WorldMap map = new(new MapGenerator());

        GameManager game = new(party, map);

        game.Party.ShouldBeSameAs(party);
        game.Map.Entities.ShouldContain(party);
        game.Map.Entities.OfType<EnemyGroup>().Count().ShouldBe(GameManager.MaxEnemies);
    }

    /// <summary>
    /// Verifies that moving into an enemy removes that enemy and increments defeated enemies.
    /// </summary>
    [Fact]
    public void MoveParty_WhenEnemySharesDestination_RemovesEnemyAndCountsDefeat()
    {
        GameManager game = TestHelpers.CreateGameManager();
        EnemyGroup enemy = new(new Position(1, 0));
        game.Map.AddEntity(enemy);

        game.MoveParty(Direction.East);

        game.Party.MapPosition.ShouldBe(new Position(1, 0));
        game.Party.EnemiesDefeated.ShouldBe(1);
        game.Map.Entities.OfType<EnemyGroup>().ShouldBeEmpty();
    }

    /// <summary>
    /// Verifies that enemies can move twice when the party is on mountain terrain.
    /// </summary>
    [Fact]
    public void Update_WhenPartyOnMountainTerrain_EnemyGetsExtraTurn()
    {
        GameManager game = TestHelpers.CreateGameManager(partyPosition: new Position(8, 0));
        EnemyGroup enemy = new(new Position(6, 0));
        game.Map.AddEntity(enemy);

        game.Update();

        game.Party.Health.ShouldBe(PlayerParty.MaxStat - 1);
    }

    /// <summary>
    /// Verifies that enemies only take one turn when the party is not on mountain or deep water terrain.
    /// </summary>
    [Fact]
    public void Update_WhenPartyOnNormalTerrain_EnemyOnlyMovesOnce()
    {
        GameManager game = TestHelpers.CreateGameManager();
        Position partyPosition = FindNormalTerrainPosition(game.Map);
        game.Party.MapPosition = partyPosition;
        EnemyGroup enemy = new(new Position(partyPosition.X - 2, partyPosition.Y));
        game.Map.AddEntity(enemy);

        game.Update();

        game.Party.Health.ShouldBe(PlayerParty.MaxStat);
        enemy.MapPosition.ShouldBe(new Position(partyPosition.X - 1, partyPosition.Y));
    }

    /// <summary>
    /// Verifies that quit transitions the game into the terminated state.
    /// </summary>
    [Fact]
    public void Quit_SetsStatusToTerminated()
    {
        GameManager game = TestHelpers.CreateGameManager();

        game.Quit();

        game.Status.ShouldBe(GameStatus.Terminated);
    }

    /// <summary>
    /// Finds a position that is not mountain or deep water terrain for tests that require a normal speed tile.
    /// </summary>
    /// <param name="map">The world map used to query terrain data.</param>
    /// <returns>A map position with terrain that does not grant enemies an extra turn.</returns>
    private static Position FindNormalTerrainPosition(WorldMap map)
    {
        for (int y = -20; y <= 20; y++)
        {
            for (int x = -20; x <= 20; x++)
            {
                Position position = new(x, y);
                TerrainType terrain = map.GetTerrain(position);
                if (terrain is not TerrainType.Mountain and not TerrainType.DeepWater)
                {
                    return position;
                }
            }
        }

        throw new InvalidOperationException("Could not find a non-mountain and non-deep water position for testing.");
    }
}

