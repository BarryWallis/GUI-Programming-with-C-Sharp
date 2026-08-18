namespace EnigmaSimulator.Domain;

/// <summary>
/// Represents a single module in the Enigma processing chain.
/// </summary>
public interface IEnigmaModule
{
    /// <summary>
    /// Gets or sets the next module in the chain.
    /// </summary>
    IEnigmaModule? NextModule { get; set; }

    /// <summary>
    /// Encodes a character in the requested direction.
    /// </summary>
    /// <param name="input">The character to encode.</param>
    /// <param name="isForward">A value indicating whether the signal is moving forward through the chain.</param>
    /// <returns>The encoded character.</returns>
    char Encode(char input, bool isForward);

    /// <summary>
    /// Processes a character through this module and the remainder of the chain.
    /// </summary>
    /// <param name="input">The character to process.</param>
    /// <returns>The processed character.</returns>
    char Process(char input)
    {
        char output = Encode(input, true);
        output = NextModule?.Process(output) ?? output;
        return Encode(output, false);
    }
}
