namespace ConsoleAppAdventureGame;

/// <summary>
/// Represents the current state of an interactive story adventure.
/// </summary>
public class Adventure
{
    private readonly Dictionary<string, StoryNode> _nodes;

    /// <summary>
    /// Initializes a new instance of the <see cref="Adventure"/> class using the provided story nodes.
    /// </summary>
    /// <param name="nodes">The story nodes that make up the adventure.</param>
    /// <param name="startNodeId">The identifier of the story node to use as the starting point.</param>
    public Adventure(IEnumerable<StoryNode> nodes, string startNodeId = "Start")
    {
        StringComparer comparison = StringComparer.OrdinalIgnoreCase;
        _nodes = nodes.ToDictionary(n => n.Id, comparison);
        CurrentNode = _nodes[startNodeId];
    }

    /// <summary>
    /// Gets the story node currently active in the adventure.
    /// </summary>
    /// <value>The active story node, or <see langword="null"/> when the story has ended.</value>
    public StoryNode? CurrentNode { get; internal set; }

    /// <summary>
    /// Retrieves the story node with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the story node to retrieve.</param>
    /// <returns>The story node with the specified identifier.</returns>
    public StoryNode GetNode(string id) => _nodes[id];

    /// <summary>
    /// Runs the adventure by rendering the current story node and processing user-selected choices until the story ends.
    /// </summary>
    /// <param name="renderer">The renderer used to display the adventure content.</param>
    public void Run(IAdventureRenderer renderer)
    {
        while (CurrentNode is not null)
        {
            renderer.Render(CurrentNode);
            if (CurrentNode.Choices.Length == 0)
            {
                CurrentNode = null;
            }
            else
            {
                Choice choice = renderer.GetChoice(CurrentNode);
                choice.Execute(this, renderer);
            }
        }
    }
}
