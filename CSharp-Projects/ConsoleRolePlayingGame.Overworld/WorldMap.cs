namespace ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Represents the overworld map, providing terrain data and tracking all entities
/// that occupy it.
/// </summary>
/// <param name="map">The <see cref="MapGenerator"/> used to calculate terrain at arbitrary positions.</param>
public class WorldMap(MapGenerator map)
{
    private readonly List<IMapEntity> _entities = [];

    /// <summary>Gets a read-only view of all entities currently on the map.</summary>
    public IEnumerable<IMapEntity> Entities => _entities.AsReadOnly();

    /// <summary>
    /// Returns the <see cref="TerrainType"/> at the specified <paramref name="position"/>. 
    /// </summary>
    /// <param name="position">The map coordinate for which to retrieve the terrain type.</param>
    /// <returns>The <see cref="TerrainType"/> at the specified position.</returns>
    public TerrainType GetTerrain(Position position) => map.CalculateTerrain(position);

    /// <summary>
    /// Returns a rectangular window of <see cref="MapCell"/> values centered around
    /// <paramref name="topLeft"/>.
    /// </summary>
    /// <param name="topLeft">The top-left map coordinate of the window.</param>
    /// <param name="width">The number of columns in the window.</param>
    /// <param name="height">The number of rows in the window.</param>
    /// <returns>A 2-D array of <see cref="MapCell"/> objects with dimensions [width, height].</returns>
    public MapCell[,] GetMapWindow(Position topLeft, int width, int height)
    {
        MapCell[,] mapWindow = new MapCell[width, height];
        for (int y = topLeft.Y; y < topLeft.Y + height; y++)
        {
            for (int x = topLeft.X; x < topLeft.X + width; x++)
            {
                Position pos = new(x, y);
                TerrainType terrain = map.CalculateTerrain(pos);
                mapWindow[x - topLeft.X, y - topLeft.Y] = new MapCell(terrain, pos);
            }
        }

        return mapWindow;
    }

    /// <summary>Adds <paramref name="entity"/> to the map's entity list.</summary>
    /// <param name="entity">The entity to add.</param>
    public void AddEntity(IMapEntity entity) => _entities.Add(entity);

    /// <summary>Removes <paramref name="entity"/> from the map's entity list.</summary>
    /// <param name="entity">The entity to remove.</param>
    public void RemoveEntity(IMapEntity entity) => _entities.Remove(entity);
 }
