using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.ConsoleApp.Tests;

internal static class TestHelpers
{
    public static GameManager CreateGameManager(string partyName = "The Party", Position? partyPosition = null)
    {
        PlayerParty party = new()
        {
            Name = partyName,
            MapPosition = partyPosition ?? new Position(0, 0)
        };

        WorldMap map = new(new MapGenerator());
        GameManager game = new(party, map);
        RemoveAllEnemies(game);

        return game;
    }

    public static void RemoveAllEnemies(GameManager game)
    {
        foreach (EnemyGroup enemy in game.Map.Entities.OfType<EnemyGroup>().ToArray())
        {
            game.Map.RemoveEntity(enemy);
        }
    }

    public static TestEntity CreateBlockingEntity(Position position, EntityType entityType = EntityType.Player)
        => new(position, entityType);

    internal sealed class TestEntity(Position position, EntityType entityType) : IMapEntity
    {
        public EntityType EntityType { get; } = entityType;

        public Position MapPosition { get; set; } = position;
    }
}
