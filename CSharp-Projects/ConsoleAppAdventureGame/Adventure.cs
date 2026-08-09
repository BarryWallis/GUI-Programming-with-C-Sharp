using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppAdventureGame;

public class Adventure
{
    public StoryNode? CurrentNode { get; internal set; }
    private readonly Dictionary<string, StoryNode> _nodes;
    public StoryNode GetNode(string id) => _nodes[id];

    public Adventure(IEnumerable<StoryNode> nodes, string startNodeId = "Start")
    {
        StringComparer comparison = StringComparer.OrdinalIgnoreCase;
        _nodes = nodes.ToDictionary(n => n.Id, comparison);
        CurrentNode = _nodes[startNodeId];
    }
}
