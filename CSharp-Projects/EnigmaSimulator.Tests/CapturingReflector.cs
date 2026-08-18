using EnigmaSimulator.Domain;

namespace EnigmaSimulator.Tests;

/// <summary>
/// Captures reflector inputs and outputs for assertions.
/// </summary>
/// <remarks>
/// Initializes a new capturing reflector.
/// </remarks>
/// <param name="inputMapping">The reflector mapping to wrap.</param>
public class CapturingReflector(string inputMapping) : Reflector(inputMapping)
{

    /// <summary>
    /// Gets the most recent reflected output.
    /// </summary>
    public char LastOutput { get; private set; }

    /// <summary>
    /// Gets the most recent reflected input.
    /// </summary>
    public char LastInput { get; private set; }

    /// <summary>
    /// Captures the input and output before returning the base reflector mapping.
    /// </summary>
    /// <param name="input">The character to encode.</param>
    /// <param name="isForward">A value indicating whether the signal is moving forward through the chain.</param>
    /// <returns>The reflected character.</returns>
    public override char Encode(char input, bool isForward)
    {
        LastInput = input;
        char output = base.Encode(input, isForward);
        LastOutput = output;

        return output;
    }
}
