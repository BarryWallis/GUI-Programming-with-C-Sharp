namespace EnigmaSimulator.Domain;

/// <summary>
/// Represents a rotor with configurable wiring, notches, and position.
/// </summary>
public class Rotor : IEnigmaModule
{
    private readonly HashSet<char> _notches;
    private readonly BiDirectionalCharEncoder _mappings;

    /// <summary>
    /// Gets the rotor position, from 1 to 26.
    /// </summary>
    public int Position { get; private set; }

    /// <summary>
    /// Gets or sets the next module in the chain.
    /// </summary>
    public IEnigmaModule? NextModule { get; set; }

    /// <summary>
    /// Initializes a new rotor from a wiring string and optional starting position.
    /// </summary>
    /// <param name="characterMapping">The rotor wiring, optionally followed by notch letters after a hyphen.</param>
    /// <param name="position">The starting position, from 1 to 26.</param>
    public Rotor(string characterMapping, int position = 1)
    {
        if (!characterMapping.All(static c => char.IsLetter(c) || c == '-'))
        {
            throw new ArgumentException("Character mapping must be all letters or include a hyphen", nameof(characterMapping));
        }

        Position = position;
        string[] parts = characterMapping.Split('-');
        _mappings = new BiDirectionalCharEncoder(parts[0]);
        _notches = parts.Length > 1 ? [.. parts[1]] : [];
    }

    /// <summary>
    /// Encodes a character through the rotor wiring.
    /// </summary>
    /// <param name="input">The character to encode.</param>
    /// <param name="isForward">A value indicating whether the signal is moving forward through the chain.</param>
    /// <returns>The encoded character.</returns>
    public char Encode(char input, bool isForward) => _mappings.Encode(input, isForward, Position - 1);

    /// <summary>
    /// Determines whether the supplied position contains a notch.
    /// </summary>
    /// <param name="position">The rotor position to inspect.</param>
    /// <returns><see langword="true"/> when the position contains a notch; otherwise, <see langword="false"/>.</returns>
    public bool HasNotch(int position) => _notches.Contains((char)('A' + position - 1));

    /// <summary>
    /// Advances the rotor to its next position.
    /// </summary>
    /// <returns><see langword="true"/> when the rotor was sitting on a notch before advancing; otherwise, <see langword="false"/>.</returns>
    public bool Advance()
    {
        bool hadNotch = HasNotch(Position);
        const int numLetters = 26;
        Position = (Position % numLetters) + 1;
        return hadNotch;
    }
}
