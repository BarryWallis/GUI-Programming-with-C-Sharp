using System;
using System.Collections.Generic;
using System.Text;

using Spectre.Console;

namespace ConsoleAppAdventureGame;

/// <summary>
/// Renders story content and choices to the Spectre console.
/// </summary>
public class SpectreConsoleAdventureRenderer : IAdventureRenderer
{
    /// <summary>
    /// Displays the text associated with the specified story node to the console.
    /// </summary>
    /// <param name="node">The story node whose text should be rendered.</param>
    public void Render(StoryNode node)
    {
        foreach (string line in node.Text)
        {
            AnsiConsole.MarkupLine(line);
        }
    }

    /// <summary>
    /// Prompts the user to select one of the choices available from the specified story node.
    /// </summary>
    /// <param name="node">The story node that contains the available choices.</param>
    /// <returns>The selected choice.</returns>
    public Choice GetChoice(StoryNode node)
    {
        Choice choice = AnsiConsole.Prompt(
            new SelectionPrompt<Choice>()
                .Title("[Yellow]What do you want to do?[/]")
                .AddChoices(node.Choices)
                .UseConverter(c => c.Text));
        AnsiConsole.MarkupLineInterpolated($"[yellow]>[/] [bold blue]{choice.Text}[/]");
        return choice;
    }

    /// <summary>
    /// Displays the outcome text associated with the specified choice to the console.
    /// </summary>
    /// <param name="choice">The choice whose follow-up action should be rendered.</param>
    public void RenderChoiceAction(Choice choice)
    {
        foreach (string line in choice.WhenChosen)
        {
            AnsiConsole.MarkupLine(line);
        }
    }
}
