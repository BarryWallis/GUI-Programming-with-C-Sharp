using Spectre.Console;
using Spectre.Console.Testing;

namespace ConsoleAppAdventureGame.Tests;

/// <summary>
/// Verifies the Story Manager workflow and validation behavior.
/// </summary>
public class StoryManagerTests
{
    /// <summary>
    /// Verifies that a valid story can be created and passes validation.
    /// </summary>
    [Fact]
    public void Story_Create_WithValidChoices_ReportsStoryIsValid()
    {
        TestConsole console = new();
        AnsiConsole.Console = console;

        console.Input.PushTextWithEnter("start");
        console.Input.PushTextWithEnter("Once upon a time");
        console.Input.PushTextWithEnter(string.Empty);
        console.Input.PushTextWithEnter("Continue");
        console.Input.PushTextWithEnter("You continue.");
        console.Input.PushTextWithEnter(string.Empty);
        console.Input.PushTextWithEnter("end");
        console.Input.PushTextWithEnter("y");
        console.Input.PushTextWithEnter("n");
        console.Input.PushTextWithEnter("y");
        console.Input.PushTextWithEnter("end");
        console.Input.PushTextWithEnter("The end.");
        console.Input.PushTextWithEnter(string.Empty);
        console.Input.PushTextWithEnter("Return");
        console.Input.PushTextWithEnter("You return to the beginning.");
        console.Input.PushTextWithEnter(string.Empty);
        console.Input.PushTextWithEnter("start");
        console.Input.PushTextWithEnter("n");
        console.Input.PushTextWithEnter("n");

        try
        {
            Story story = new();
            story.Create();

            Assert.Contains("Story is valid.", console.Output);
        }
        finally
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        }
    }

    /// <summary>
    /// Verifies that invalid references between nodes are reported during validation.
    /// </summary>
    [Fact]
    public void Story_Create_WithMissingNodeReference_ReportsInvalidChoice()
    {
        TestConsole console = new();
        AnsiConsole.Console = console;

        console.Input.PushTextWithEnter("start");
        console.Input.PushTextWithEnter("Once upon a time");
        console.Input.PushTextWithEnter(string.Empty);
        console.Input.PushTextWithEnter("Continue");
        console.Input.PushTextWithEnter("You continue.");
        console.Input.PushTextWithEnter(string.Empty);
        console.Input.PushTextWithEnter("missing");
        console.Input.PushTextWithEnter("y");
        console.Input.PushTextWithEnter("n");
        console.Input.PushTextWithEnter("n");

        try
        {
            Story story = new();
            story.Create();

            Assert.Contains("Invalid choice", console.Output);
            Assert.Contains("points to non-existent node", console.Output);
        }
        finally
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        }
    }

    /// <summary>
    /// Verifies that selecting Exit ends the manager run and shows the welcome message once.
    /// </summary>
    [Fact]
    public void StoryManager_Run_WithImmediateExit_ShowsWelcomeOnce()
    {
        TestConsole console = new();
        _ = console.Interactive();
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);
        AnsiConsole.Console = console;

        try
        {
            StoryManager storyManager = new();
            storyManager.Run();

            Assert.Equal(1, console.Output.Split("Welcome to the Story Manager!").Length - 1);
        }
        finally
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        }
    }

    /// <summary>
    /// Verifies that the menu loops for another action without repeating the welcome message.
    /// </summary>
    [Fact]
    public void StoryManager_Run_AfterAction_ReturnsToMenuWithoutRepeatingWelcome()
    {
        TestConsole console = new();
        _ = console.Interactive();

        // Select "Load a story"
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);
        console.Input.PushTextWithEnter("missing.json");

        // Select "Exit" after returning to the menu.
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);

        AnsiConsole.Console = console;

        try
        {
            StoryManager storyManager = new();
            storyManager.Run();

            Assert.Equal(1, console.Output.Split("Welcome to the Story Manager!").Length - 1);
            Assert.True(console.Output.Split("What do you want to do?").Length - 1 >= 2);
        }
        finally
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        }
    }
}
