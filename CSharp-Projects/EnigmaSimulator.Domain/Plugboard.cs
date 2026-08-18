namespace EnigmaSimulator.Domain;

/// <summary>
/// Represents the Enigma plugboard wiring.
/// </summary>
public class Plugboard : IEnigmaModule
{
    private readonly Dictionary<char, char> _mappings = [];

    /// <summary>
    /// Initializes a new plugboard with the supplied letter pairs.
    /// </summary>
    /// <param name="pairs">The two-letter pairs to connect.</param>
    public Plugboard(params string[] pairs)
    {
        if (!pairs.All(s => s is { Length: 2 } && s.All(char.IsLetter)))
        {
            throw new ArgumentException("Each pair must have exactly two letters", nameof(pairs));
        }

        foreach (string pair in pairs)
        {
            _mappings.Add(char.ToUpperInvariant(pair[0]), char.ToUpperInvariant(pair[1]));
            _mappings.Add(char.ToUpperInvariant(pair[1]), char.ToUpperInvariant(pair[0]));
        }
    }

    /// <summary>
    /// Gets or sets the next module in the chain.
    /// </summary>
    public IEnigmaModule? NextModule { get; set; }

    /// <summary>
    /// Maps a character through the configured plugboard wiring.
    /// </summary>
    /// <param name="input">The character to encode.</param>
    /// <param name="isForward">A value indicating whether the signal is moving forward through the chain.</param>
    /// <returns>The mapped character, or the original character when no mapping exists.</returns>
    public char Encode(char input, bool isForward = true) => _mappings.GetValueOrDefault(input, input);
}
