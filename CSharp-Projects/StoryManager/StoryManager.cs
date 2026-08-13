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
    /// Runs the story manager menu and exits when the user requests to quit.
    /// </summary>
    /// <returns><see langword="true"/> when the application should terminate; otherwise, <see langword="false"/>.</returns>
    internal bool Run()
    {
        AnsiConsole.MarkupLine("[bold yellow]Welcome to the Story Manager![/]");
        AnsiConsole.MarkupLine("This program allows you to load and save stories to storage.");
        AnsiConsole.MarkupLine("You can also view and edit stories in a simple text format.");
        AnsiConsole.MarkupLine("Press [bold green]Enter[/] to continue or [bold red]Esc[/] to exit.");
        ConsoleKeyInfo key = Console.ReadKey(intercept: true);
        if (key.Key == ConsoleKey.Escape)
        {
            return true; // Exit the program
        }

        // Load or create a story
        LoadOrCreateStory();

        //// Display the story content
        //DisplayStory(story);
        //// Save the story
        //SaveStory(story);
        return false; // Continue running
    }

    /// <summary>
    /// Presents the user with a choice to create or load story content.
    /// </summary>
    private void LoadOrCreateStory()
    {
        string[] choices = ["Create a new story", "Load a story",];
        string answer = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[Yellow]What do you want to do?[/]")
                .AddChoices(choices));
        if (answer == choices[0])
        {
            _story.Create();
        }
        else if (answer == choices[1])
        {
            _story.Load();
        }
        else
        {
            throw new UnreachableException();
        }
    }
}