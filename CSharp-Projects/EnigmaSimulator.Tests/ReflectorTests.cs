using EnigmaSimulator.Domain;

using Shouldly;

namespace EnigmaSimulator.Tests;

public class ReflectorTests
{
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
