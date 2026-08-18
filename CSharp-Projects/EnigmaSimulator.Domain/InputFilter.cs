namespace EnigmaSimulator.Domain;

/// <summary>
/// Filters non-letter input before it enters the Enigma chain.
/// </summary>
public class InputFilter : IEnigmaModule
{
    /// <summary>
    /// Gets or sets the next module in the chain.
    /// </summary>
    public IEnigmaModule? NextModule { get; set; }

    /// <summary>
    /// Returns the input character unchanged.
    /// </summary>
    /// <param name="input">The character to encode.</param>
    /// <param name="isForward">A value indicating whether the signal is moving forward through the chain.</param>
    /// <returns>The original character.</returns>
    public char Encode(char input, bool isForward) => input;

    /// <summary>
    /// Delegates letters to the next module and leaves other characters untouched.
    /// </summary>
    /// <param name="input">The character to process.</param>
    /// <returns>The processed character or the original input.</returns>
    public char Process(char input) => (NextModule is not null && char.IsLetter(input)) ? NextModule.Process(input) : input;
}
