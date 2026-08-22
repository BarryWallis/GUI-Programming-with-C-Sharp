using ConsoleRolePlayingGame.Overworld;

namespace ConsoleRolePlayingGame.ConsoleApp;

/// <summary>
/// Represents the player-controlled party that moves on the overworld and
/// tracks combat statistics.
/// </summary>
public class PlayerParty : IMapEntity
{
    /// <summary>The maximum value for <see cref="Health"/> and <see cref="Mana"/>.</summary>
    public const int MaxStat = 10;

    /// <summary>Gets or sets the party's current hit points.</summary>
    public int Health { get; internal set; } = MaxStat;

    /// <summary>Gets or sets the party's current mana points.</summary>
    public int Mana { get; internal set; } = MaxStat;

    /// <summary>Gets or initializes the display name of the party.</summary>
    public string Name { get; init; } = "The Party";

    /// <summary>Gets the total number of enemy groups defeated by this party.</summary>
    public int EnemiesDefeated { get; private set; } = 0;

    /// <inheritdoc/>
    public Position MapPosition { get; set; } = new(0, 0);

    /// <inheritdoc/>
    public EntityType EntityType { get; }

    /// <summary>Moves the party one step in the given <paramref name="direction"/>.</summary>
    /// <param name="direction">The direction to move.</param>
    public void Move(Direction direction) => MapPosition = MapPosition.Move(direction);

    /// <summary>Increments <see cref="EnemiesDefeated"/> by one.</summary>
    public void EnemyDefeated() => EnemiesDefeated += 1;
}