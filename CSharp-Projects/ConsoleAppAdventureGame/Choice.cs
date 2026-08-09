namespace ConsoleAppAdventureGame;

public record Choice(string Text)
{
    public string[] WhenChosen { get; init; } = [];
    public required string NextNodeId { get; init; }
}