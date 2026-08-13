namespace ConsoleAppAdventureGame;

/// <summary>
/// Represents a single node in the adventure story graph.
/// </summary>
public class StoryNode(string id)
{
    /// <summary>
    /// Gets the unique identifier for this story node.
    /// </summary>
    public string Id => id;

    /// <summary>
    /// Gets or initializes the narrative text shown when this node is reached.
    /// </summary>
    public required string[] Text;

    /// <summary>
    /// Gets or initializes the list of choices the player can select from this node.
    /// </summary>
    public Choice[] Choices { get; init; } = [];
}
