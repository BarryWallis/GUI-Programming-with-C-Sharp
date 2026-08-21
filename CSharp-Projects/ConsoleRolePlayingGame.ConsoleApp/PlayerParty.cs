using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.ConsoleApp;

public class PlayerParty : IMapEntity
{
    public const int MaxStat = 10;

    public int Health { get; internal set; } = MaxStat;

    public int Mana { get; internal set; } = MaxStat;

    public string Name { get; init; } = "The Party";

    public int EnemiesDefeated { get; private set; } = 0;

    public Position MapPosition { get; set; } = new(0, 0);

    public EntityType EntityType { get; }

    public void Move(Direction direction) => MapPosition = MapPosition.Move(direction);

    public void EnemyDefeated() => EnemiesDefeated += 1;
}