using ConsoleRolePlayingGame.Overworld;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Renderers;

/// <summary>
/// Renders a rectangular map viewport centered on the player party as a
/// Specter.Console <see cref="Canvas"/>.
/// </summary>
/// <param name="game">The active <see cref="GameManager"/> providing map and entity data.</param>
/// <param name="width">The width of the canvas in pixels.</param>
/// <param name="height">The height of the canvas in pixels.</param>
public class MapRenderer(GameManager game, int width, int height)
{
    /// <summary>
    /// Generates a <see cref="Canvas"/> that visualizes the map window around the
    /// player's current position, coloring each pixel by terrain or entity type.
    /// </summary>
    /// <returns>An <see cref="IRenderable"/> canvas depicting the visible map area.</returns>
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

    /// <summary>
    /// Returns the display <see cref="Color"/> for a map cell, prioritizing entity color
    /// over terrain color when an entity is present.
    /// </summary>
    /// <param name="entity">The entity occupying the cell, or <c>null</c> if empty.</param>
    /// <param name="terrain">The terrain type of the cell.</param>
    /// <returns>The <see cref="Color"/> that should be used to render the cell.</returns>
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