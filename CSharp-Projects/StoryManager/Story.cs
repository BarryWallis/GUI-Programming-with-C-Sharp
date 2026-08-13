// A program to load and save stories to storage.

using ConsoleAppAdventureGame;

using Spectre.Console;

/// <summary>
/// Manages the creation and loading of a story definition.
/// </summary>
internal class Story
{
    private readonly Dictionary<string, StoryNode> _nodes = [];

    /// <summary>
    /// Prompts the user to create a new story and validates each node against the connected choices.
    /// </summary>
    internal void Create()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Creating a new story...");
        do
        {
            string id = GetNodeId();
            string[] text = GetText("Enter a line for this part of the story");
            Choice[] choices = GetChoices();
            StoryNode node = new(id) { Text = text, Choices = choices };
            _nodes.Add(id, node);
        } while (AnsiConsole.Confirm("Add another part to the story?"));

        AnsiConsole.WriteLine("Validating story...");
        if (Valid())
        {
            AnsiConsole.MarkupLine("[green]Story is valid.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Story is invalid. Please fix the issues and try again.[/]");
        }
    }

    /// <summary>
    /// Validates that each choice points to an existing story node.
    /// </summary>
    /// <returns><see langword="true"/> when every choice references a valid node; otherwise, <see langword="false"/>.</returns>
    private bool Valid()
    {
        foreach (KeyValuePair<string, StoryNode> entry in _nodes)
        {
            StoryNode node = entry.Value;
            foreach (Choice choice in node.Choices)
            {
                if (!_nodes.ContainsKey(choice.NextNodeId.ToLowerInvariant()))
                {
                    AnsiConsole.MarkupLine($"[red]Invalid choice: '{choice.Text}' in node '{node.Id}' points to non-existent node '{choice.NextNodeId}'.[/]");
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Collects the player-defined choices for a story node.
    /// </summary>
    /// <returns>The choices associated with the current story segment.</returns>
    private Choice[] GetChoices()
    {
        List<Choice> choices = [];
        do
        {
            string choiceText = AnsiConsole.Ask<string>("Enter the [green]condition[/] for this choice:");
            string[] whenChosen = GetText("Enter the lines for the [green]when chosen[/] condition");
            string nextNodeId = GetNextNodeId();
            choices.Add(new Choice(choiceText) { WhenChosen = whenChosen, NextNodeId = nextNodeId });
        } while (AnsiConsole.Confirm("Add another choice for this part of the story?"));
        return [.. choices];
    }

    /// <summary>
    /// Prompts the user for the identifier of the next story node and validates the input.
    /// </summary>
    /// <returns>The resolved identifier of the next node.</returns>
    private string GetNextNodeId()
    {
        string nextNodeId = AnsiConsole.Ask<string>("Enter the [green]ID[/] of the next part of the story:");
        return !_nodes.ContainsKey(nextNodeId.ToLowerInvariant())
                    ? AnsiConsole.Confirm($"The ID '{nextNodeId}' does not exist yet. Do you want to use it?")
                        ? nextNodeId.ToLowerInvariant()
                        : GetNextNodeId()
                    : nextNodeId.ToLowerInvariant();
    }

    /// <summary>
    /// Reads a multi-line block of story text from the console.
    /// </summary>
    /// <param name="prompt">The message shown to the user before entering the text.</param>
    /// <returns>The lines of text entered by the user.</returns>
    private static string[] GetText(string prompt)
    {
        List<string> text = [];
        string line;
        do
        {
            TextPrompt<string> textPrompt = new TextPrompt<string>($"{prompt} (leave empty to finish):").AllowEmpty();
            line = AnsiConsole.Prompt(textPrompt);
            if (!string.IsNullOrWhiteSpace(line))
            {
                text.Add(line);
            }
        } while (!string.IsNullOrWhiteSpace(line));

        if (text.Count == 0 && text.All(string.IsNullOrWhiteSpace))
        {
            AnsiConsole.MarkupLine("[red]You must enter at least one line of text for this part of the story.[/]");
            return GetText(prompt);
        }

        return [.. text];
    }

    /// <summary>
    /// Prompts the user for a unique identifier for the story node.
    /// </summary>
    /// <returns>A normalized, case-insensitive identifier for the node.</returns>
    private string GetNodeId()
    {
        TextPrompt<string> id = new TextPrompt<string>("Enter the [green]ID[/] for this part of the story:")
            .Validate(k => !_nodes.ContainsKey(k.ToLowerInvariant()),
                           $"[red]A story part with that ID already exists. Please enter a unique ID.[/]");
        return AnsiConsole.Prompt(id).ToLowerInvariant();
    }

    /// <summary>
    /// Loads a previously saved story from storage.
    /// </summary>
    internal void Load()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Loading a story...");
    }
}