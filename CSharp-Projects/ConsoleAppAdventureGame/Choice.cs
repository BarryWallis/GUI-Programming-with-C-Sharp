namespace ConsoleAppAdventureGame;

/// <summary>
/// Represents a selectable option within a story node.
/// </summary>
public record Choice(string Text)
{
    public string[] WhenChosen { get; init; } = [];
    public required string NextNodeId { get; init; }

    /// <summary>
    /// Executes the choice by rendering its follow-up action and advancing the adventure to the next story node.
    /// </summary>
    /// <param name="adventure">The adventure instance whose current node should be updated.</param>
    /// <param name="renderer">The renderer used to display the choice action.</param>
    public void Execute(Adventure adventure, SpectreConsoleAdventureRenderer _)
    {
        SpectreConsoleAdventureRenderer.RenderChoiceAction(this);
        adventure.CurrentNode = string.IsNullOrWhiteSpace(NextNodeId) ? null : adventure.GetNode(NextNodeId);
    }
}
