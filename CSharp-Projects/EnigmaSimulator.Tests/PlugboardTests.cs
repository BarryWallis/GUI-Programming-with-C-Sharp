using EnigmaSimulator.Domain;

using Shouldly;

namespace EnigmaSimulator.Tests;

/// <summary>
/// Verifies plugboard wiring behavior.
/// </summary>
public class PlugboardTests
{
    /// <summary>
    /// Verifies that configured plugboard pairs are swapped in both directions.
    /// </summary>
    [Theory]
    [InlineData('H', 'O')]
    [InlineData('O', 'H')]
    [InlineData('A', 'W')]
    [InlineData('X', 'X')]
    public void ConnectionPresentAfterBeingConfigured(char input, char expected)
    {
        // Arrange
        Plugboard plugboard = new("OH", "WA");

        // Act
        char output = plugboard.Encode(input);

        // Assert
        output.ShouldBe(expected);
    }

    /// <summary>
    /// Verifies that unconnected letters pass through unchanged.
    /// </summary>
    [Theory]
    [InlineData('N')]
    [InlineData('E')]
    [InlineData('T')]
    public void PegboardShouldReturnInputWhenNotConnected(char input)
    {
        // Arrange
        Plugboard plugboard = new();

        // Act
        char output = plugboard.Encode(input);

        // Assert
        output.ShouldBe(input);
    }

    /// <summary>
    /// Verifies that duplicate connections are rejected.
    /// </summary>
    [Fact]
    public void DuplicateConnectionsAreNotAllowed() => _ = Should.Throw<ArgumentException>(static () => new Plugboard("HI", "ID"));
}
