namespace ConsoleAppAdventureGame;

/// <summary>
/// Defines a renderer for presenting adventure story content and collecting player choices.
/// </summary>
public interface IAdventureRenderer
{
    /// <summary>
    /// Prompts the player to select a choice from the specified story node.
    /// </summary>
    /// <param name="node">The story node that provides the available choices.</param>
    /// <returns>The selected choice.</returns>
    Choice GetChoice(StoryNode node);

    /// <summary>
    /// Renders the specified story node to the player.
    /// </summary>
    /// <param name="node">The story node to render.</param>
    void Render(StoryNode node);

    /// <summary>
    /// Renders the action associated with the specified choice.
    /// </summary>
    /// <param name="choice">The choice whose action should be rendered.</param>
    void RenderChoiceAction(Choice choice);
}
