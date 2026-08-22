using Shouldly;

namespace ConsoleRolePlayingGame.Overworld.Tests;

/// <summary>
/// Unit tests for <see cref="WorldMap"/> behavior.
/// </summary>
public class WorldMapTests
{
    /// <summary>
    /// Verifies that adding an entity makes it visible through the map entity collection.
    /// </summary>
    [Fact]
    public void AddEntity_AddsEntityToTheMap()
    {
        WorldMap map = new(new MapGenerator());
        TestEntity entity = new(new Position(3, 4));

        map.AddEntity(entity);

        map.Entities.ShouldContain(entity);
    }

    /// <summary>
    /// Verifies that removing an entity excludes it from the map entity collection.
    /// </summary>
    [Fact]
    public void RemoveEntity_RemovesEntityFromTheMap()
    {
        WorldMap map = new(new MapGenerator());
        TestEntity entity = new(new Position(3, 4));
        map.AddEntity(entity);

        map.RemoveEntity(entity);

        map.Entities.ShouldNotContain(entity);
    }

    /// <summary>
    /// Verifies that map window generation returns the requested dimensions and expected coordinates.
    /// </summary>
    [Fact]
    public void GetMapWindow_ReturnsExpectedCellsAndPositions()
    {
        MapGenerator generator = new();
        WorldMap map = new(generator);
        Position topLeft = new(2, -1);

        MapCell[,] window = map.GetMapWindow(topLeft, 3, 2);

        window.GetLength(0).ShouldBe(3);
        window.GetLength(1).ShouldBe(2);
        window[0, 0].Position.ShouldBe(new Position(2, -1));
        window[1, 0].Position.ShouldBe(new Position(3, -1));
        window[2, 0].Position.ShouldBe(new Position(4, -1));
        window[0, 1].Position.ShouldBe(new Position(2, 0));
        window[1, 1].Position.ShouldBe(new Position(3, 0));
        window[2, 1].Position.ShouldBe(new Position(4, 0));
        window[0, 0].Terrain.ShouldBe(generator.CalculateTerrain(new Position(2, -1)));
        window[2, 1].Terrain.ShouldBe(generator.CalculateTerrain(new Position(4, 0)));
    }

    private sealed class TestEntity(Position position) : IMapEntity
    {
        public EntityType EntityType { get; } = EntityType.Player;

        public Position MapPosition { get; set; } = position;
    }
}
