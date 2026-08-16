namespace EnigmaSimulator.Domain;

public class Reflector(string inputMapping)
{
    private readonly BiDirectionalCharEncoder _mapper = new(inputMapping);

    public char Encode(char input, bool isForward = true) => _mapper.Encode(input, isForward);
}