using System;
using System.Collections.Generic;
using System.Text;

using Spectre.Console;

namespace ConsoleAppAdventureGame;

public class SpectreConsoleAdventureRenderer
{
    /// <summary>
    /// Displays the text associated with the specified story node to the console.
    /// </summary>
    /// <param name="node">The story node whose text should be rendered.</param>
    public static void Render(StoryNode node)
    {
        foreach (string line in node.Text)
        {
            AnsiConsole.MarkupLine(line);
        }
    }

    public static Choice GetChoice(StoryNode node)
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
    public static void RenderChoiceAction(Choice choice)
    {
        foreach (string line in choice.WhenChosen)
        {
            AnsiConsole.MarkupLine(line);
        }
    }
}
