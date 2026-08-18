namespace EnigmaSimulator.Domain;

/// <summary>
/// Represents a complete Enigma machine configuration.
/// </summary>
public class EnigmaMachine
{
    /// <summary>
    /// Gets or sets the first module in the chain.
    /// </summary>
    public IEnigmaModule NextModule { get; set; }

    /// <summary>
    /// Initializes a new machine from the supplied modules.
    /// </summary>
    /// <param name="modules">The modules that make up the machine, in processing order.</param>
    public EnigmaMachine(params IEnigmaModule[] modules)
    {
        if (!modules.OfType<RotorAdvancer>().Any())
        {
            modules = [
                new InputFilter(),
                new InputNormalizer(),
                new RotorAdvancer(),
                .. modules
            ];
        }

        NextModule = modules[0];
        for (int i = 0; i < modules.Length - 1; i++)
        {
            modules[i].NextModule = modules[i + 1];
        }
    }

    /// <summary>
    /// Encodes a single character.
    /// </summary>
    /// <param name="input">The character to encode.</param>
    /// <returns>The encoded character.</returns>
    public char Encode(char input) => NextModule?.Process(input) ?? input;

    /// <summary>
    /// Encodes an entire string.
    /// </summary>
    /// <param name="input">The text to encode.</param>
    /// <returns>The encoded text, or the original input when the machine is not configured.</returns>
    public string Encode(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || NextModule is null)
        {
            return input;
        }

        char[] encodedLetters = [.. input.Select(NextModule.Process)];
        return new string(encodedLetters);
    }
}
