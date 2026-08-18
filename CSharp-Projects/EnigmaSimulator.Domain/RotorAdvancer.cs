namespace EnigmaSimulator.Domain;

/// <summary>
/// Advances rotor positions before a character enters the rest of the chain.
/// </summary>
public class RotorAdvancer : IEnigmaModule
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
    /// Advances any queued rotors and forwards the character to the next module.
    /// </summary>
    /// <param name="input">The character to process.</param>
    /// <returns>The processed character.</returns>
    public char Process(char input)
    {
        Queue<Rotor> rotorsToAdvance = BuildRotorAdvancementQueue();
        bool shouldAdvance = true;
        while (shouldAdvance && rotorsToAdvance.Count > 0)
        {
            Rotor rotor = rotorsToAdvance.Dequeue();
            shouldAdvance = rotor.Advance();
        }

        return NextModule?.Process(input) ?? input;
    }

    /// <summary>
    /// Builds the queue of rotors that should advance before encoding.
    /// </summary>
    /// <returns>The rotors discovered in chain order.</returns>
    private Queue<Rotor> BuildRotorAdvancementQueue()
    {
        Queue<Rotor> rotors = new();
        IEnigmaModule? currentModule = this;
        while (currentModule is not null)
        {
            if (currentModule is Rotor rotor)
            {
                rotors.Enqueue(rotor);
            }

            currentModule = currentModule.NextModule;
        }

        return rotors;
    }
}
