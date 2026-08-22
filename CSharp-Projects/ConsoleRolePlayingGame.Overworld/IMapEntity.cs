namespace ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Represents an entity that occupies a cell on the overworld map.
/// </summary>
public interface IMapEntity
{
    /// <summary>Gets the role classification of this entity.</summary>
    EntityType EntityType { get; }

    /// <summary>Gets or sets the entity's current position on the map.</summary>
    Position MapPosition { get; set; }
}