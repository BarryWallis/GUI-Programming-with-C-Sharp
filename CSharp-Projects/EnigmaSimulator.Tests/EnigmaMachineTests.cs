using EnigmaSimulator.Domain;

using Shouldly;

namespace EnigmaSimulator.Tests;

/// <summary>
/// Verifies end-to-end machine encoding.
/// </summary>
public class EnigmaMachineTests
{
    /// <summary>
    /// Verifies the machine output for a few known single-character inputs.
    /// </summary>
    [Theory]
    [InlineData('A', 'B')]
    [InlineData('G', 'X')]
    [InlineData('X', 'G')]
    [InlineData('Z', 'U')]
    public void EnigmaShouldProduceCorrectOutput(char input, char expected)
    {
        // Arrange
        EnigmaMachine enigma = new(new Plugboard(),
            new Rotor(RotorSets.Enigma3),
            new Rotor(RotorSets.Enigma2),
            new Rotor(RotorSets.Enigma1),
            new Reflector(ReflectorSets.ReflectorB));

        // Act
        char output = enigma.Encode(input);

        // Assert
        output.ShouldBe(expected);
    }

    /// <summary>
    /// Verifies that the first keystroke reaches the reflector with the expected input.
    /// </summary>
    [Theory]
    [InlineData('Z', 'E')]
    public void EnigmaShouldReachReflectorWithCorrectOutputForFirstKeystroke(char input, char expected)
    {
        // Arrange
        CapturingReflector reflector = new(ReflectorSets.ReflectorB);
        EnigmaMachine enigma = new(new Plugboard(),
            new Rotor(RotorSets.Enigma3),
            new Rotor(RotorSets.Enigma2),
            new Rotor(RotorSets.Enigma1),
            reflector);

        // Act
        _ = enigma.Encode(input);

        // Assert
        reflector.LastInput.ShouldBe(expected);
    }

    /// <summary>
    /// Verifies string encoding without a plugboard.
    /// </summary>
    [Theory]
    [InlineData("HELLO", "ILBDA")]
    [InlineData("ILBDA", "HELLO")]
    [InlineData("THEENIGMAMACHINEISENCODINGPROPERLY", "OPCWCLZNLVKKGQONYNOZVDFUSNKXJGUJOZ")]
    [InlineData("OPCWCLZNLVKKGQONYNOZVDFUSNKXJGUJOZ", "THEENIGMAMACHINEISENCODINGPROPERLY")]
    public void EnigmaShouldEncodeStringsCorrectly(string input, string expected)
    {
        // Arrange
        EnigmaMachine enigma = new(new Plugboard(),
            new Rotor(RotorSets.Enigma3),
            new Rotor(RotorSets.Enigma2),
            new Rotor(RotorSets.Enigma1),
            new Reflector(ReflectorSets.ReflectorB));

        // Act
        string output = enigma.Encode(input);

        // Assert
        output.ShouldBe(expected);
    }

    /// <summary>
    /// Verifies string encoding when plugboard swaps are configured.
    /// </summary>
    [Theory]
    [InlineData("HELLO", "IQBDA")]
    [InlineData("THEENIGMAMACHINEISENCODINGPROPERLY", "GPOSRLZELVKKGQSNYEYPVDFUCEKTJGFJOZ")]
    public void EnigmaShouldEncodeStringsCorrectlyWithPlugboard(string input, string expected)
    {
        // Arrange
        EnigmaMachine enigma = new(new Plugboard("NE", "XT"),
            new Rotor(RotorSets.Enigma3),
            new Rotor(RotorSets.Enigma2),
            new Rotor(RotorSets.Enigma1),
            new Reflector(ReflectorSets.ReflectorB));

        // Act
        string output = enigma.Encode(input);

        // Assert
        output.ShouldBe(expected);
    }
}
