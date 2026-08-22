namespace ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Represents a single cell on the overworld map, combining terrain information with its map position.
/// </summary>
/// <param name="Terrain">The type of terrain occupying this cell.</param>
/// <param name="Position">The map coordinates of this cell.</param>
public record MapCell(TerrainType Terrain, Position Position);
