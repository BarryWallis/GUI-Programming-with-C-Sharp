using System.Text.RegularExpressions;

using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.ConsoleApp;

public class GameManager
{
    public WorldMap Map { get; }
    public GameStatus Status { get; private set; } = GameStatus.Overworld;
    public PlayerParty Party { get; }

    public const int MaxEnemies = 5;

    public GameManager(PlayerParty party, WorldMap map)
    {
        Party = party;
        Map = map;
        Map.AddEntity(Party);
        for (int i = 0; i < MaxEnemies; i++)
        {
            SpawnNearbyEncounter();
        }
    }

    private void SpawnNearbyEncounter()
    {
        OpenPositionSelector selector = new(Map);
        Position point = selector.GetOpenPositionNear(Party.MapPosition, 5, 10);
        Map.AddEntity(new EnemyGroup(point));
    }

    public void MoveParty(Direction direction)
    {
        Party.Move(direction);
        List<EnemyGroup> enemies = [.. Map.Entities.OfType<EnemyGroup>().Where(e => e.MapPosition == Party.MapPosition)];
        foreach (EnemyGroup group in enemies)
        {
            Map.RemoveEntity(group);
            Party.EnemyDefeated();
        }
    }

    public void Quit() => Status = GameStatus.Terminated;

    public void Update()
    {
        if (Status != GameStatus.Overworld)
        {
            return;
        }

        List<EnemyGroup> enemies = [.. Map.Entities.OfType<EnemyGroup>()];
        foreach (EnemyGroup group in enemies)
        {
            group.MoveTowards(Party.MapPosition, Map);
            if (group.MapPosition == Party.MapPosition)
            {
                Map.RemoveEntity(group);
                Party.Health -= 1;
            }
        }

        if (Party.Health <= 0)
        {
            Status = GameStatus.GameOver;
        }

        if (Map.Entities.OfType<EnemyGroup>().Count() < MaxEnemies)
        {
            SpawnNearbyEncounter();
        }
    }
}