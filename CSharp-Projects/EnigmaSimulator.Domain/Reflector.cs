namespace EnigmaSimulator.Domain;

/// <summary>
/// Represents the reflector stage of the Enigma machine.
/// </summary>
/// <remarks>
/// Initializes a new reflector.
/// </remarks>
/// <param name="inputMapping">The 26-letter reflector mapping.</param>
public class Reflector(string inputMapping) : IEnigmaModule
{
    private readonly BiDirectionalCharEncoder _mapper = new(inputMapping);

    /// <summary>
    /// Gets the reflector's terminal chain position.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when a caller attempts to assign a next module.</exception>
    public IEnigmaModule? NextModule
    {
        get => null;
        set => throw new InvalidOperationException("Reflector cannot have a next module.");
    }

    /// <summary>
    /// Encodes a character through the reflector mapping.
    /// </summary>
    /// <param name="input">The character to encode.</param>
    /// <param name="isForward">A value indicating whether the signal is moving forward through the chain.</param>
    /// <returns>The reflected character.</returns>
    public virtual char Encode(char input, bool isForward = true) => _mapper.Encode(input, isForward);

    /// <summary>
    /// Processes a character through the reflector.
    /// </summary>
    /// <param name="input">The character to process.</param>
    /// <returns>The reflected character.</returns>
    public char Process(char input) => Encode(input, true);
}
