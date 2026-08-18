namespace EnigmaSimulator.Domain;

/// <summary>
/// Normalizes input characters before they enter the machine.
/// </summary>
public class InputNormalizer : IEnigmaModule
{
    /// <summary>
    /// Gets or sets the next module in the chain.
    /// </summary>
    public IEnigmaModule? NextModule { get; set; }

    /// <summary>
    /// Converts the input character to uppercase.
    /// </summary>
    /// <param name="input">The character to encode.</param>
    /// <param name="isForward">A value indicating whether the signal is moving forward through the chain.</param>
    /// <returns>The uppercase equivalent of the input character.</returns>
    public char Encode(char input, bool isForward) => char.ToUpperInvariant(input);
}
