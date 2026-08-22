using SimplexNoise;

namespace ConsoleRolePlayingGame.Overworld;

/// <summary>
/// Provides deterministic Perlin-style noise values via the SimplexNoise library,
/// normalized to the range [0, 1].
/// </summary>
/// <param name="seed">The seed used to initialize the noise function.</param>
/// <param name="scale">The frequency scale applied to the noise function. Defaults to <c>0.05</c>.</param>
public class PerlinNoiseProvider(int seed, float scale = 0.05f)
{
    private static readonly Lock _lock = new();

    /// <summary>
    /// Generates a noise value at the specified 2-D coordinates.
    /// </summary>
    /// <param name="x">The X coordinate to sample.</param>
    /// <param name="y">The Y coordinate to sample.</param>
    /// <returns>A value in the range [0, 1].</returns>
    public float Generate(int x, int y)
    {
        const float maxGeneratedValue = 256f;

        float result;
        lock ( _lock)
        {
            Noise.Seed = seed;
            result = Noise.CalcPixel2D(x, y, scale);
        }

        return result / maxGeneratedValue;
    }   
}