using System.Text.RegularExpressions;

using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.ConsoleApp;

/// <summary>
/// Coordinates overall game state: spawning enemies, processing player input,
/// advancing enemy AI, and detecting win/loss conditions.
/// </summary>
public class GameManager
{
    /// <summary>Gets the world map for the current game session.</summary>
    public WorldMap Map { get; }

    /// <summary>Gets the current high-level state of the game.</summary>
    public GameStatus Status { get; private set; } = GameStatus.Overworld;

    /// <summary>Gets the player-controlled party.</summary>
    public PlayerParty Party { get; }

    /// <summary>The maximum number of enemy groups that may exist on the map simultaneously.</summary>
    public const int MaxEnemies = 5;

    /// <summary>
    /// Initializes a new <see cref="GameManager"/>, registers the player party on the map,
    /// and spawns the initial set of enemy encounters.
    /// </summary>
    /// <param name="party">The player's party.</param>
    /// <param name="map">The world map for this session.</param>
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

    /// <summary>
    /// Moves the player party one step in the given <paramref name="direction"/> and
    /// removes any enemy groups that now occupy the same cell, crediting a defeat for each.
    /// </summary>
    /// <param name="direction">The direction in which to move the party.</param>
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

    /// <summary>Transitions the game status to <see cref="GameStatus.Terminated"/>, ending the game loop.</summary>
    public void Quit() => Status = GameStatus.Terminated;

    /// <summary>
    /// Advances enemy AI for one tick: moves each enemy toward the party, applies damage
    /// on collision, checks for a game-over condition, and respawns enemies as needed.
    /// </summary>
    public void Update()
    {
        if (Status != GameStatus.Overworld)
        {
            return;
        }

        List<EnemyGroup> enemies = [.. Map.Entities.OfType<EnemyGroup>()];
        foreach (EnemyGroup group in enemies)
        {
            // If the party is on difficult terrain, give the enemy an extra move to catch up.
            for (int i = 0; i < 2; i++)
            {
                group.MoveTowards(Party.MapPosition, Map);
                if (group.MapPosition == Party.MapPosition)
                {
                    Map.RemoveEntity(group);
                    Party.Health -= 1;
                }

                if (Map.GetTerrain(Party.MapPosition) is not TerrainType.Mountain
                    and not TerrainType.DeepWater)
                {
                    break;
                }
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