using SimplexNoise;

namespace ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Generates terrain for map positions using two independent Perlin noise layers
/// (height and temperature) to produce varied biomes.
/// </summary>
public class MapGenerator
{
    private readonly PerlinNoiseProvider _heightNoise = new(1234);
    private readonly PerlinNoiseProvider _temperatureNoise = new(5678);

    /// <summary>
    /// Determines the <see cref="TerrainType"/> at the given map position by sampling
    /// height and temperature noise values.
    /// </summary>
    /// <param name="position">The map position to evaluate.</param>
    /// <returns>The <see cref="TerrainType"/> that best describes the location.</returns>
    public TerrainType CalculateTerrain(Position position)
    {
        float height = _heightNoise.Generate(position.X, position.Y);
        float temperature = _temperatureNoise.Generate(position.X, position.Y);

        return height switch
        {
            < 0.15f => TerrainType.DeepWater,
            < 0.35f => TerrainType.Water,
            < 0.4f => TerrainType.Desert,
            < 0.8f => TerrainType.Mountain,
            _ => temperature switch
            {
                < 0.4f => TerrainType.Forest,
                < 0.8f => TerrainType.Desert,
                _ => TerrainType.Grass
            }
        };
    }
}