namespace EnigmaSimulator.Domain;

internal class BiDirectionalCharEncoder
{
    private readonly Dictionary<char, char> _mappings = [];
    private readonly Dictionary<char, char> _reverseMappings = [];

    public BiDirectionalCharEncoder(string mapping)
    {
        if (mapping.Length != 26 && !mapping.All(c => c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z')))
        {
            throw new ArgumentException("Mapping must be 26 characters long and contain only letters A-Z.", nameof(mapping));
        }

        mapping = mapping.ToUpperInvariant();
        for (int i = 0; i < mapping.Length; i++)
        {
            char input = (char)('A' + i);
            char output = mapping[i];
            _mappings.Add(input, output);
            _reverseMappings.Add(output, input);
        }

        if (_mappings.Count != _reverseMappings.Count 
            || !_mappings.All(kvp => _reverseMappings.TryGetValue(kvp.Value, out char reverse) && reverse == kvp.Key))
        {
            throw new InvalidOperationException("Internal invariant failed: mapping must be a valid bijection.");
        }
    }

    internal char Encode(char input, bool isForward, int offset = 0)
    {
        const int numLetters = 26;

        if (!char.IsLetter(input) || !char.IsUpper(input))
        {
            throw new ArgumentException("Input must be an uppercase letter A-Z.", nameof(input));
        }

        if (offset is < 0 or >= numLetters)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be between 0 and 25.");
        }

        // Adjust input character for the offset using ' to A
        int inputIndex = input - 'A';
        int adjustIndex = (inputIndex + offset + numLetters) % numLetters; // Ensure positive index
        char adjustedInput = (char)('A' + adjustIndex);

        Dictionary<char, char> mappings = isForward ? _mappings : _reverseMappings;
        char encodedChar = mappings.GetValueOrDefault(adjustedInput, input);

        // Adjust the encoded character back for the offset
        int encodedIndex = encodedChar - 'A';
        int finalIndex = (encodedIndex - offset + numLetters) % numLetters; // Ensure positive index

        return (char)('A' + finalIndex);
    }
}