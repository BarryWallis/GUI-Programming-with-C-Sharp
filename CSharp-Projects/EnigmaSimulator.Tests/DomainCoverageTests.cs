using System.Reflection;

using EnigmaSimulator.Domain;

using Shouldly;

namespace EnigmaSimulator.Tests;

/// <summary>
/// Covers shared domain behaviors and edge cases.
/// </summary>
public class DomainCoverageTests
{
    private static readonly Assembly _domainAssembly = typeof(EnigmaMachine).Assembly;
    private static readonly Type _domainRotorType = _domainAssembly.GetType("EnigmaSimulator.Tests.Rotor", throwOnError: true)!;
    private static readonly Type _domainRotorSetsType = _domainAssembly.GetType("EnigmaSimulator.Tests.RotorSets", throwOnError: true)!;

    /// <summary>
    /// Verifies the default interface pipeline behavior.
    /// </summary>
    [Fact]
    public void DefaultInterfaceProcessShouldRunForwardThroughNextModuleAndBackAgain()
    {
        DefaultProcessModule module = new();

        char output = ((IEnigmaModule)module).Process('A');

        output.ShouldBe('Z');
        module.ForwardCalls.ShouldBe(1);
        module.ReverseCalls.ShouldBe(1);
        module.LastForwardInput.ShouldBe('A');
        module.LastReverseInput.ShouldBe('B');
    }

    /// <summary>
    /// Verifies that the input filter only delegates letters.
    /// </summary>
    [Fact]
    public void InputFilterShouldOnlyDelegateLetters()
    {
        RecordingModule next = new();
        InputFilter filter = new()
        {
            NextModule = next
        };

        filter.Encode('x', true).ShouldBe('x');
        filter.Process('A').ShouldBe('A');
        filter.Process('1').ShouldBe('1');
        next.ProcessCalls.ShouldBe(1);
    }

    /// <summary>
    /// Verifies that the input normalizer uppercases characters.
    /// </summary>
    [Fact]
    public void InputNormalizerShouldUppercaseInput()
    {
        InputNormalizer normalizer = new();

        normalizer.Encode('a', true).ShouldBe('A');
        ((IEnigmaModule)normalizer).Process('b').ShouldBe('B');
    }

    /// <summary>
    /// Verifies that invalid plugboard pairs are rejected.
    /// </summary>
    [Fact]
    public void PlugboardShouldRejectInvalidPairs() => Should.Throw<ArgumentException>(static () => new Plugboard("A"));

    /// <summary>
    /// Verifies that a reflector cannot be chained to another module.
    /// </summary>
    [Fact]
    public void ReflectorShouldRejectNextModuleAssignment()
    {
        Reflector reflector = new(ReflectorSets.ReflectorB);

        _ = Should.Throw<InvalidOperationException>(() => reflector.NextModule = new InputFilter());
    }

    /// <summary>
    /// Verifies that the machine preserves an explicitly supplied rotor advancer.
    /// </summary>
    [Fact]
    public void EnigmaMachineShouldKeepAnExplicitRotorAdvancer()
    {
        RotorAdvancer advancer = new();
        RecordingModule terminal = new();
        EnigmaMachine machine = new(advancer, terminal);

        machine.NextModule.ShouldBeSameAs(advancer);
        advancer.NextModule.ShouldBeSameAs(terminal);
    }

    /// <summary>
    /// Verifies that the machine returns input unchanged when the chain is removed.
    /// </summary>
    [Fact]
    public void EnigmaMachineShouldReturnInputWhenNextModuleIsNull()
    {
        EnigmaMachine machine = new(new InputFilter(), new InputNormalizer(), new RotorAdvancer(), new RecordingModule())
        {
            NextModule = null!
        };

        machine.Encode("HELLO").ShouldBe("HELLO");
    }

    /// <summary>
    /// Verifies that whitespace input is left untouched.
    /// </summary>
    [Fact]
    public void EnigmaMachineShouldReturnWhitespaceInputUnchanged()
    {
        EnigmaMachine machine = new(new Plugboard(), new Reflector(ReflectorSets.ReflectorB));

        machine.Encode("   ").ShouldBe("   ");
    }

    /// <summary>
    /// Verifies the rotor advancer's direct encoding behavior.
    /// </summary>
    [Fact]
    public void RotorAdvancerShouldExposeItsEncodeImplementation()
    {
        RotorAdvancer advancer = new();

        advancer.Encode('Q', true).ShouldBe('Q');
    }

    /// <summary>
    /// Verifies that no queued rotors leaves the input unchanged.
    /// </summary>
    [Fact]
    public void RotorAdvancerShouldReturnInputWhenNoRotorsAreQueued()
    {
        RotorAdvancer advancer = new()
        {
            NextModule = new RecordingModule()
        };

        advancer.Process('Q').ShouldBe('Q');
    }

    private static object CreateDomainRotor(string mapping, int position = 1) => Activator.CreateInstance(_domainRotorType, mapping, position)!
        ;

    private static char InvokeRotorEncode(object rotor, char input, bool isForward) => (char)_domainRotorType.GetMethod("Encode", [typeof(char), typeof(bool)])!.Invoke(rotor, [input, isForward])!;

    private static int GetDomainRotorPosition(IEnigmaModule rotor) => (int)_domainRotorType.GetProperty("Position", BindingFlags.Public | BindingFlags.Instance)!.GetValue(rotor)!;

    private sealed class DefaultProcessModule : IEnigmaModule
    {
        public IEnigmaModule? NextModule { get; set; }

        public int ForwardCalls { get; private set; }

        public int ReverseCalls { get; private set; }

        public char LastForwardInput { get; private set; }

        public char LastReverseInput { get; private set; }

        public char Encode(char input, bool isForward)
        {
            if (isForward)
            {
                ForwardCalls++;
                LastForwardInput = input;
                return (char)(input + 1);
            }

            ReverseCalls++;
            LastReverseInput = input;
            return 'Z';
        }
    }

    private sealed class RecordingModule : IEnigmaModule
    {
        public IEnigmaModule? NextModule { get; set; }

        public int ProcessCalls { get; private set; }

        public char Encode(char input, bool isForward) => input;

        public char Process(char input)
        {
            ProcessCalls++;
            return input;
        }
    }
}
