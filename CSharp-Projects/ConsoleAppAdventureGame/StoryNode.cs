using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppAdventureGame;

/// <summary>
/// Represents a single node in the adventure story graph.
/// </summary>
public class StoryNode(string id)
{
    public string Id => id;

    public required string[] Text { get; init; }
    public Choice[] Choices { get; init; } = [];
}
