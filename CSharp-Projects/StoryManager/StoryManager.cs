// A program to load and save stories to storage.

using System.Diagnostics;

using Spectre.Console;

/// <summary>
/// Hosts the main menu flow for creating and loading story definitions.
/// </summary>
internal class StoryManager
{
    private readonly Story _story = new();

    /// <summary>
    /// Runs the story manager until the user chooses to exit.
    /// </summary>
    internal void Run()
    {
        AnsiConsole.MarkupLine("[bold yellow]Welcome to the Story Manager![/]");
        AnsiConsole.MarkupLine("This program allows you to load and save stories to storage.");
        AnsiConsole.MarkupLine("You can also view and edit stories in a simple text format.");

        while (true)
        {
            if (EnterStoryManager())
            {
                return;
            }
        }
    }

    /// <summary>
    /// Presents the user with a choice to create, load, edit, or exit.
    /// </summary>
    /// <returns><see langword="true"/> when the user chooses to exit; otherwise, <see langword="false"/>.</returns>
    private bool EnterStoryManager()
    {
        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[Yellow]What do you want to do?[/]")
                .AddChoices("Create a new story", "Load a story", "Edit the story", "Exit"));

        switch (choice)
        {
            case "Create a new story":
                _story.Create();
                return false;
            case "Load a story":
                _story.Load();
                return false;
            case "Edit the story":
                _story.Edit();
                return false;
            case "Exit":
                return true;
            default:
                throw new UnreachableException();
        }
    }
}