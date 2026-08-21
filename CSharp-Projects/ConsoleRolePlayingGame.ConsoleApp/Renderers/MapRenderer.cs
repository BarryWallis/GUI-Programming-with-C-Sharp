using ConsoleRolePlayingGame.Overworld;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Renderers;

public class MapRenderer(GameManager game, int width, int height)
{
    public IRenderable GenerateVisual()
    {
        Position center = game.Party.MapPosition;
        int offsetX = (int)Math.Ceiling(width / 2.0);
        int offsetY = (int)Math.Ceiling(height / 2.0);
        Position upperLeft = new(center.X - offsetX, center.Y - offsetY);
        MapCell[,] window = game.Map.GetMapWindow(upperLeft, width, height);
        Canvas canvas = new(window.GetLength(0), window.GetLength(1));
        for (int y = 0; y < window.GetLength(1); y++)
        {
            for (int x = 0; x < window.GetLength(0); x++)
            {
                MapCell cell = window[x, y];
                IMapEntity? entity = game.Map.Entities.FirstOrDefault(e => e.MapPosition == cell.Position);
                _ = canvas.SetPixel(x, y, GetCellColor(entity, cell.Terrain));
            }
        }

        return canvas;
    }

    private static Color GetCellColor(IMapEntity? entity, TerrainType terrain)
        => entity is not null ? entity.EntityType switch
                                {
                                    EntityType.Player => Color.Yellow1,
                                    EntityType.Enemy => Color.Red,
                                    _ => Color.Magenta1
                                }
                              : terrain switch
                                {
                                    TerrainType.Grass => Color.Green,
                                    TerrainType.Water => Color.Blue,
                                    TerrainType.DeepWater => Color.Blue3_1,
                                    TerrainType.Mountain => new Color(128, 128, 128),
                                    TerrainType.Forest => Color.DarkGreen,
                                    TerrainType.Desert => Color.MistyRose1,
                                    _ => Color.DarkMagenta
                                };
}