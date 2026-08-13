namespace ConsoleAppAdventureGame;

/// <summary>
/// Represents a selectable option within a story node.
/// </summary>
/// <param name="Text">The text displayed to the player for the choice.</param>
public record Choice(string Text)
{
    /// <summary>
    /// Gets or initializes the text shown after the player selects this choice.
    /// </summary>
    public string[] WhenChosen { get; init; } = [];

    /// <summary>
    /// Gets or initializes the identifier of the next story node reached when the choice is selected.
    /// </summary>
    public required string NextNodeId { get; init; }

    /// <summary>
    /// Executes the choice by rendering its follow-up action and advancing the adventure to the next story node.
    /// </summary>
    /// <param name="adventure">The adventure instance whose current node should be updated.</param>
    /// <param name="renderer">The renderer used to display the choice action.</param>
    public void Execute(Adventure adventure, IAdventureRenderer renderer)
    {
        renderer.RenderChoiceAction(this);
        adventure.CurrentNode = string.IsNullOrWhiteSpace(NextNodeId) ? null : adventure.GetNode(NextNodeId);
    }
}
