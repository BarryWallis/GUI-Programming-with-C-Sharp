// A program to load and save stories to storage.

using System.Text.Json;

using ConsoleAppAdventureGame;

using Spectre.Console;

/// <summary>
/// Manages the creation, loading, and editing of a story definition.
/// </summary>
internal class Story
{
    private readonly Dictionary<string, StoryNode> _nodes = [];
    private static readonly JsonSerializerOptions _saveSerializerOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
    };

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
    /// Saves the current story nodes to disk as a JSON file.
    /// </summary>
    internal void Save()
    {
        AnsiConsole.WriteLine();
        string filePath = AnsiConsole.Ask<string>("Enter the [green]file path[/] to save the story:", "story.json");

        try
        {
            string json = JsonSerializer.Serialize(_nodes, _saveSerializerOptions);
            File.WriteAllText(filePath, json);
            AnsiConsole.MarkupLine($"[green]Story saved to '{filePath}'.[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to save story: {ex.Message}[/]");
        }
    }

    /// <summary>
    /// Loads a previously saved story from storage.
    /// </summary>
    internal void Load()
    {
        AnsiConsole.WriteLine();
        string filePath = AnsiConsole.Ask<string>("Enter the [green]file path[/] to load the story:", "story.json");

        if (!File.Exists(filePath))
        {
            AnsiConsole.MarkupLine($"[red]File '{filePath}' was not found.[/]");
            return;
        }

        try
        {
            DoLoad(filePath);
        }
        catch (JsonException ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to parse story JSON: {ex.Message}[/]");
        }
        catch (IOException ex)
        {
            AnsiConsole.MarkupLine($"[red]Failed to read story file: {ex.Message}[/]");
        }
        catch (UnauthorizedAccessException ex)
        {
            AnsiConsole.MarkupLine($"[red]Access denied while loading story: {ex.Message}[/]");
        }
    }

    /// <summary>
    /// Loads and normalizes story nodes from a JSON file, replacing the current in-memory story.
    /// </summary>
    /// <param name="filePath">The path to the JSON file to load.</param>
    private void DoLoad(string filePath)
    {
        bool flowControl = LoadJSON(filePath, out Dictionary<string, StoryNode>? normalizedNodes);
        if (!flowControl)
        {
            return;
        }

        if (normalizedNodes is null)
        {
            AnsiConsole.MarkupLine("[red]Failed to load story: file did not contain valid story data.[/]");
            return;
        }

        _nodes.Clear();
        foreach ((string key, StoryNode node) in normalizedNodes)
        {
            _nodes.Add(key, node);
        }

        string result = Valid() ? $"[green]Story loaded from '{filePath}'.[/]"
                                : "[red]Story loaded, but it contains invalid references.[/]";
        AnsiConsole.MarkupLine(result);
    }

    /// <summary>
    /// Deserializes story JSON and normalizes node IDs to lowercase dictionary keys.
    /// </summary>
    /// <param name="filePath">The path to the JSON file.</param>
    /// <param name="normalizedNodes">The normalized set of nodes when deserialization succeeds.</param>
    /// <returns><see langword="true"/> when the file is valid and nodes were normalized; otherwise, <see langword="false"/>.</returns>
    private static bool LoadJSON(string filePath, out Dictionary<string, StoryNode>? normalizedNodes)
    {
        normalizedNodes = null;
        string json = File.ReadAllText(filePath);
        Dictionary<string, StoryNode>? loadedNodes = JsonSerializer.Deserialize<Dictionary<string, StoryNode>>(json, _saveSerializerOptions);
        if (loadedNodes is null)
        {
            AnsiConsole.MarkupLine("[red]Failed to load story: file did not contain valid story data.[/]");
            return false;
        }

        normalizedNodes = [];
        foreach ((string key, StoryNode node) in loadedNodes)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                AnsiConsole.MarkupLine("[red]Failed to load story: a node key is missing or empty.[/]");
                return false;
            }

            string normalizedKey = key.ToLowerInvariant();
            if (!normalizedNodes.TryAdd(normalizedKey, node))
            {
                AnsiConsole.MarkupLine($"[red]Failed to load story: duplicate node ID '{normalizedKey}'.[/]");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Runs an interactive edit session, looping until the user finishes and optionally saves changes.
    /// </summary>
    internal void Edit()
    {
        AnsiConsole.WriteLine();
        if (_nodes.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]No story nodes to edit. Create or load a story first.[/]");
            return;
        }

        while (true)
        {
            List<string> nodeOptions = [.. _nodes.Keys, "Done editing"];
            string selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a node to edit:[/]")
                    .AddChoices(nodeOptions));

            if (selected == "Done editing")
            {
                if (AnsiConsole.Confirm("Save the story?"))
                {
                    Save();
                }

                break;
            }

            EditNode(selected);
        }
    }

    /// <summary>
    /// Loops over edit options for a single node until the user is done.
    /// </summary>
    /// <param name="nodeId">The identifier of the node to edit.</param>
    private void EditNode(string nodeId)
    {
        while (true)
        {
            AnsiConsole.WriteLine();
            StoryNode currentNode = _nodes[nodeId];
            ShowCurrentLines("Current text", currentNode.Text);
            ShowCurrentChoices(currentNode.Choices);

            string action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]Editing node '[green]{nodeId}[/]'. What do you want to edit?[/]")
                    .AddChoices("Edit text", "Edit choices", "Done with this node"));

            switch (action)
            {
                case "Edit text":
                    _nodes[nodeId].Text = GetText("Enter a line for this part of the story");
                    break;
                case "Edit choices":
                    EditChoices(nodeId);
                    break;
                case "Done with this node":
                    return;
            }
        }
    }

    /// <summary>
    /// Loops over the choices of a node, allowing add, edit, and delete operations.
    /// </summary>
    /// <param name="nodeId">The identifier of the node whose choices are being edited.</param>
    private void EditChoices(string nodeId)
    {
        while (true)
        {
            StoryNode node = _nodes[nodeId];
            List<string> choiceOptions = [.. node.Choices.Select((c, i) => $"{i + 1}: {c.Text}"),
                                           "Add new choice",
                                           "Done editing choices"];

            string selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Select a choice to edit, or add/finish:[/]")
                    .AddChoices(choiceOptions));

            if (selected == "Done editing choices")
            {
                return;
            }

            if (selected == "Add new choice")
            {
                string conditionText = AnsiConsole.Ask<string>("Enter the [green]condition[/] for this choice:");
                string[] whenChosen = GetText("Enter the lines for the [green]when chosen[/] condition");
                string nextNodeId = GetNextNodeId();
                Choice newChoice = new(conditionText) { WhenChosen = whenChosen, NextNodeId = nextNodeId };
                _nodes[nodeId] = new StoryNode(nodeId) { Text = node.Text, Choices = [.. node.Choices, newChoice] };
                continue;
            }

            int index = int.Parse(selected.Split(':')[0]) - 1;
            EditChoice(nodeId, index);
        }
    }

    /// <summary>
    /// Loops over edit options for a single choice until the user is done or deletes it.
    /// </summary>
    /// <param name="nodeId">The identifier of the node that owns the choice.</param>
    /// <param name="choiceIndex">The zero-based index of the choice within the node.</param>
    private void EditChoice(string nodeId, int choiceIndex)
    {
        Choice choice;
        string action;
        do
        {
            choice = _nodes[nodeId].Choices[choiceIndex];
            action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]Editing choice '[green]{choice.Text}[/]'. What do you want to change?[/]")
                    .AddChoices("Edit condition text", "Edit when-chosen text", "Edit next node ID",
                                "Delete this choice", "Done with this choice"));
        } while (EditAction(nodeId, choiceIndex, choice, action));
    }

    /// <summary>
    /// Applies a single choice-edit action and indicates whether editing should continue.
    /// </summary>
    /// <param name="nodeId">The identifier of the node that owns the choice.</param>
    /// <param name="choiceIndex">The zero-based index of the choice being edited.</param>
    /// <param name="choice">The current choice state.</param>
    /// <param name="action">The selected edit action.</param>
    /// <returns><see langword="true"/> to continue editing this choice; otherwise, <see langword="false"/>.</returns>
    private bool EditAction(string nodeId, int choiceIndex, Choice choice, string action)
    {
        switch (action)
        {
            case "Edit condition text":
                string newText = AnsiConsole.Ask<string>("Enter the new [green]condition[/] for this choice:", choice.Text);
                ReplaceChoice(nodeId, choiceIndex, new Choice(newText) { WhenChosen = choice.WhenChosen, NextNodeId = choice.NextNodeId });
                break;
            case "Edit when-chosen text":
                ShowCurrentLines("Current when-chosen text", choice.WhenChosen);
                string[] newWhenChosen = GetText("Enter the lines for the [green]when chosen[/] condition");
                ReplaceChoice(nodeId, choiceIndex, new Choice(choice.Text) { WhenChosen = newWhenChosen, NextNodeId = choice.NextNodeId });
                break;
            case "Edit next node ID":
                AnsiConsole.MarkupLine($"Current next node ID: [green]{choice.NextNodeId}[/]");
                string newNextNodeId = GetNextNodeId();
                ReplaceChoice(nodeId, choiceIndex, new Choice(choice.Text) { WhenChosen = choice.WhenChosen, NextNodeId = newNextNodeId });
                break;
            case "Delete this choice":
                if (!DeleteThisChoice(nodeId, choiceIndex, choice))
                {
                    return false;
                }

                break;
            case "Done with this choice":
                return false;
        }

        return true;
    }

    /// <summary>
    /// Confirms and deletes a choice from a node.
    /// </summary>
    /// <param name="nodeId">The identifier of the node that owns the choice.</param>
    /// <param name="choiceIndex">The zero-based index of the choice to delete.</param>
    /// <param name="choice">The choice being considered for deletion.</param>
    /// <returns><see langword="true"/> when the choice was not deleted and editing should continue; otherwise, <see langword="false"/>.</returns>
    private bool DeleteThisChoice(string nodeId, int choiceIndex, Choice choice)
    {
        if (AnsiConsole.Confirm($"Are you sure you want to delete choice '{choice.Text}'?"))
        {
            StoryNode node = _nodes[nodeId];
            _nodes[nodeId] = new StoryNode(nodeId)
            {
                Text = node.Text,
                Choices = [.. node.Choices.Where((_, i) => i != choiceIndex)]
            };
            return false;
        }

        return true;
    }

    /// <summary>
    /// Displays a labelled list of lines to the console.
    /// </summary>
    /// <param name="label">The heading shown before the lines.</param>
    /// <param name="lines">The lines to display.</param>
    private static void ShowCurrentLines(string label, string[] lines)
    {
        AnsiConsole.MarkupLine($"[grey]{label}:[/]");
        foreach (string line in lines)
        {
            AnsiConsole.MarkupLine($"  [grey]{Markup.Escape(line)}[/]");
        }
    }

    /// <summary>
    /// Displays a summary of all choices for a node to the console.
    /// </summary>
    /// <param name="choices">The choices to display.</param>
    private static void ShowCurrentChoices(Choice[] choices)
    {
        if (choices.Length == 0)
        {
            AnsiConsole.MarkupLine("[grey]Choices: (none)[/]");
            return;
        }

        AnsiConsole.MarkupLine("[grey]Choices:[/]");
        for (int i = 0; i < choices.Length; i++)
        {
            Choice choice = choices[i];
            AnsiConsole.MarkupLine($"  [grey]{i + 1}. {Markup.Escape(choice.Text)} → {Markup.Escape(choice.NextNodeId)}[/]");
            foreach (string line in choice.WhenChosen)
            {
                AnsiConsole.MarkupLine($"       [grey]{Markup.Escape(line)}[/]");
            }
        }
    }

    /// <summary>
    /// Replaces a single choice within a node at the given index.
    /// </summary>
    /// <param name="nodeId">The identifier of the node containing the choice.</param>
    /// <param name="choiceIndex">The zero-based index of the choice to replace.</param>
    /// <param name="updated">The replacement <see cref="Choice"/>.</param>
    private void ReplaceChoice(string nodeId, int choiceIndex, Choice updated)
    {
        StoryNode node = _nodes[nodeId];
        Choice[] choices = [.. node.Choices];
        choices[choiceIndex] = updated;
        _nodes[nodeId] = new StoryNode(nodeId) { Text = node.Text, Choices = choices };
    }
}