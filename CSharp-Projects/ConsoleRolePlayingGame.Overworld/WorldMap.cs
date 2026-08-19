namespace ConsoleRolePlayingGame.Overworld;

public class WorldMap(MapGenerator map)
{
    private readonly List<IMapEntity> _entities = [];

    public IEnumerable<IMapEntity> Entities => _entities.AsReadOnly();

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

    public void AddEntity(IMapEntity entity) => _entities.Add(entity);

    public void RemoveEntity(IMapEntity entity) => _entities.Remove(entity);
}
