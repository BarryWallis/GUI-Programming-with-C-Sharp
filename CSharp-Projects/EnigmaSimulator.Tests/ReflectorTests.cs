using EnigmaSimulator.Domain;

using Shouldly;

namespace EnigmaSimulator.Tests;

/// <summary>
/// Verifies reflector mappings and symmetry.
/// </summary>
public class ReflectorTests
{
    /// <summary>
    /// Verifies the reflector output for a few known mappings.
    /// </summary>
    [Theory]
    [InlineData('A', 'Y')]
    [InlineData('J', 'X')]
    [InlineData('X', 'J')]
    public void ReflectorsShouldHaveCorrectMappings(char input, char expected)
    {
        // Arrange
        Reflector reflector = new(ReflectorSets.ReflectorB);

        // Act
        char output = reflector.Encode(input);

        // Assert
        output.ShouldBe(expected);
    }

    /// <summary>
    /// Verifies that every reflector preset is its own inverse.
    /// </summary>
    [Theory]
    [InlineData(ReflectorSets.ReflectorA)]
    [InlineData(ReflectorSets.ReflectorB)]
    [InlineData(ReflectorSets.ReflectorC)]
    public void ReflectorsShouldBeBidirectional(string mapping)
    {
        // Arrange
        Reflector reflector = new(mapping);

        // Act & Assert
        for (char c = 'A'; c <= 'Z'; c++)
        {
            char encoded = reflector.Encode(c);
            char decoded = reflector.Encode(encoded);
            decoded.ShouldBe(c);
        }
    }
}
