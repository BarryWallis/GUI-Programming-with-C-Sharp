using System;
using System.Collections.Generic;
using System.Text;

using Shouldly;

namespace ConsoleRolePlayingGame.Overworld.Tests;

/// <summary>
/// Unit tests for <see cref="PerlinNoiseProvider"/>.
/// </summary>
public class PerlinNoiseProviderTests
{
    /// <summary>
    /// Verifies that <see cref="PerlinNoiseProvider.Generate"/> returns the same value
    /// for a given seed and coordinate pair across multiple invocations.
    /// </summary>
    [Theory]
    [InlineData(1234, 1, 1, 0.8184)]
    [InlineData(1234, 1, -1, 0.8185)]
    [InlineData(5678, 1, -1, 0.3938)]
    [InlineData(1234, 8, 0, 0.332)]
    [InlineData(5678, 8, 0, 0.1325)]
    public void ProducesDeterministicResults(int seed, int x, int y, float expected)
    {
        // Arrange
        PerlinNoiseProvider noiseProvider = new(seed, 0.1f);

        // Act
        float result = noiseProvider.Generate(x, y);

        // Assert
        result.ShouldBe(expected, 0.001);
    }
}
