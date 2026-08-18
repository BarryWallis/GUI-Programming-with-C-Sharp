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
    /// Verifies that invalid reflector mappings are rejected.
    /// </summary>
    [Theory]
    [InlineData("ABC!")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXY!")]
    public void ReflectorShouldRejectInvalidMappings(string mapping) => Should.Throw<ArgumentException>(() => new Reflector(mapping));

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

    /// <summary>
    /// Verifies that queued rotors advance in order.
    /// </summary>
    [Fact]
    public void RotorAdvancerShouldAdvanceDomainRotorsInOrder()
    {
        RotorAdvancer advancer = new();
        IEnigmaModule rotor1 = (IEnigmaModule)CreateDomainRotor(RotorSets.Enigma1, 17);
        IEnigmaModule rotor2 = (IEnigmaModule)CreateDomainRotor(RotorSets.Enigma2, 1);
        RecordingModule terminal = new();

        rotor2.NextModule = terminal;
        rotor1.NextModule = rotor2;
        advancer.NextModule = rotor1;

        _ = advancer.Process('A');

        GetDomainRotorPosition(rotor1).ShouldBe(18);
        GetDomainRotorPosition(rotor2).ShouldBe(2);
        terminal.ProcessCalls.ShouldBe(1);
    }

    /// <summary>
    /// Verifies the rotor encoding contract through reflection.
    /// </summary>
    [Theory]
    [InlineData('A', RotorSets.Enigma1, 1, true, 'E')]
    [InlineData('A', RotorSets.Enigma1, 2, true, 'J')]
    [InlineData('N', RotorSets.Enigma1, 1, false, 'K')]
    [InlineData('D', RotorSets.Enigma3, 3, false, 'A')]
    public void DomainRotorShouldEncodeCorrectly(char input, string mapping, int position, bool isForward, char expected)
    {
        object rotor = CreateDomainRotor(mapping, position);

        InvokeRotorEncode(rotor, input, isForward).ShouldBe(expected);
    }

    /// <summary>
    /// Verifies that invalid rotor mappings are rejected.
    /// </summary>
    [Fact]
    public void DomainRotorShouldRejectInvalidMappings() => Should.Throw<TargetInvocationException>(static () => CreateDomainRotor("ABC!"))
            .InnerException.ShouldBeOfType<ArgumentException>();

    /// <summary>
    /// Verifies that invalid rotor inputs and offsets fail.
    /// </summary>
    [Fact]
    public void DomainRotorShouldRejectLowercaseAndInvalidOffsets()
    {
        object rotor = CreateDomainRotor(RotorSets.Enigma1, 0);

        _ = Should.Throw<TargetInvocationException>(() => InvokeRotorEncode(rotor, 'a', true))
            .InnerException.ShouldBeOfType<ArgumentException>();

        rotor = CreateDomainRotor(RotorSets.Enigma1, 27);

        _ = Should.Throw<TargetInvocationException>(() => InvokeRotorEncode(rotor, 'A', true))
            .InnerException.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// Verifies that the rotor set constants expose the expected values.
    /// </summary>
    [Theory]
    [InlineData(nameof(RotorSets.Enigma1), "EKMFLGDQVZNTOWYHXUSPAIBRCJ-Q")]
    [InlineData(nameof(RotorSets.Enigma2), "AJDKSIRUXBLHWTMCQGZNPYFVOE-E")]
    [InlineData(nameof(RotorSets.Enigma3), "BDFHJLCPRTXVZNYEIWGAKMUSQO-V")]
    [InlineData(nameof(RotorSets.Enigma4), "ESOVPZJAYQUIRHXLNFTGKDCMWB-J")]
    [InlineData(nameof(RotorSets.Enigma5), "VZBRGITYUPSDNHLXAWMJQOFECK-Z")]
    public void DomainRotorSetsShouldExposeExpectedConstants(string fieldName, string expected)
    {
        FieldInfo field = _domainRotorSetsType.GetField(fieldName, BindingFlags.Public | BindingFlags.Static)!;

        field.GetRawConstantValue().ShouldBe(expected);
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
