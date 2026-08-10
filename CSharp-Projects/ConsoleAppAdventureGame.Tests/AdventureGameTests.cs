using Spectre.Console;
using Spectre.Console.Testing;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ConsoleAppAdventureGame.Tests;

public class AdventureGameTests
{
    [Fact]
    public void ChoiceExecute_RendersActionAndAdvancesAdventure()
    {
        StoryNode startNode = new("Start")
        {
            Text = ["Start node"]
        };
        StoryNode nextNode = new("Next")
        {
            Text = ["Next node"]
        };
        Choice choice = new("Proceed")
        {
            NextNodeId = nextNode.Id,
            WhenChosen = ["You proceed."]
        };

        Adventure adventure = new([startNode, nextNode], startNode.Id);
        RecordingRenderer renderer = new();

        choice.Execute(adventure, renderer);

        Assert.Equal(nextNode.Id, GetCurrentNode(adventure)?.Id);
        _ = Assert.Single(renderer.RenderedActions);
        Assert.Same(choice, renderer.RenderedActions[0]);
    }

    [Fact]
    public void ChoiceExecute_WithoutNextNode_ClearsCurrentNode()
    {
        StoryNode node = new("Start")
        {
            Text = ["Start node"]
        };
        Choice choice = new("Stop")
        {
            NextNodeId = string.Empty,
            WhenChosen = ["You stop."]
        };

        Adventure adventure = new([node], node.Id);
        RecordingRenderer renderer = new();

        choice.Execute(adventure, renderer);

        Assert.Null(GetCurrentNode(adventure));
        _ = Assert.Single(renderer.RenderedActions);
    }

    [Fact]
    public void AdventureConstructorAndGetNode_UseConfiguredStartNodeAndCaseInsensitiveLookup()
    {
        StoryNode startNode = new("Start")
        {
            Text = ["Start node"]
        };
        StoryNode otherNode = new("Other")
        {
            Text = ["Other node"]
        };

        Adventure adventure = new([startNode, otherNode], startNode.Id);

        Assert.Equal(startNode.Id, GetCurrentNode(adventure)?.Id);
        Assert.Same(startNode, adventure.GetNode("START"));
        Assert.Same(otherNode, adventure.GetNode("other"));
    }

    [Fact]
    public void AdventureRun_RendersUntilStoryEnds()
    {
        StoryNode startNode = new("Start")
        {
            Text = ["Start node"],
            Choices =
            [
                new Choice("Proceed")
                {
                    NextNodeId = "Middle",
                    WhenChosen = ["You proceed"]
                }
            ]
        };
        StoryNode middleNode = new("Middle")
        {
            Text = ["Middle node"],
            Choices =
            [
                new Choice("Finish")
                {
                    NextNodeId = string.Empty,
                    WhenChosen = ["You finish"]
                }
            ]
        };

        Adventure adventure = new([startNode, middleNode], startNode.Id);
        RecordingRenderer renderer = new([startNode.Choices[0], middleNode.Choices[0]]);

        adventure.Run(renderer);

        Assert.Null(GetCurrentNode(adventure));
        Assert.Equal([startNode, middleNode], renderer.RenderedNodes);
        Assert.Equal(2, renderer.RenderedActions.Count);
    }

    [Fact]
    public void ConsoleAdventureRenderer_Render_WritesNodeText()
    {
        TextWriter originalOut = Console.Out;
        StringWriter writer = new();
        Console.SetOut(writer);

        try
        {
            ConsoleAdventureRenderer renderer = new();
            StoryNode node = new("Node")
            {
                Text = ["Line 1", "Line 2"]
            };

            renderer.Render(node);

            Assert.Equal("Line 1" + Environment.NewLine + "Line 2" + Environment.NewLine, writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ConsoleAdventureRenderer_GetChoice_ReturnsSelectedChoice()
    {
        TextReader originalIn = Console.In;
        TextWriter originalOut = Console.Out;
        StringReader input = new("3\n2\n");
        StringWriter writer = new();
        Console.SetIn(input);
        Console.SetOut(writer);

        try
        {
            ConsoleAdventureRenderer renderer = new();
            StoryNode node = new("Node")
            {
                Text = ["Choose"],
                Choices =
                [
                    new Choice("First") { NextNodeId = "next" },
                    new Choice("Second") { NextNodeId = "next" }
                ]
            };

            Choice choice = renderer.GetChoice(node);

            Assert.Equal("Second", choice.Text);
            Assert.Contains("Invalid choice.", writer.ToString());
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void ConsoleAdventureRenderer_RenderChoiceAction_WritesOutcomeText()
    {
        TextWriter originalOut = Console.Out;
        StringWriter writer = new();
        Console.SetOut(writer);

        try
        {
            ConsoleAdventureRenderer renderer = new();
            Choice choice = new("Proceed")
            {
                NextNodeId = "next",
                WhenChosen = ["Outcome line 1", "Outcome line 2"]
            };

            renderer.RenderChoiceAction(choice);

            Assert.Equal("Outcome line 1" + Environment.NewLine + "Outcome line 2" + Environment.NewLine, writer.ToString());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }

    [Fact]
    public void SpectreConsoleAdventureRenderer_Render_WritesNodeText()
    {
        TestConsole console = new();
        AnsiConsole.Console = console;

        try
        {
            SpectreConsoleAdventureRenderer renderer = new();
            StoryNode node = new("Node")
            {
                Text = ["Spectre line 1", "Spectre line 2"]
            };

            renderer.Render(node);

            Assert.Contains("Spectre line 1", console.Output);
            Assert.Contains("Spectre line 2", console.Output);
        }
        finally
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        }
    }

    [Fact]
    public void SpectreConsoleAdventureRenderer_GetChoice_ReturnsSelectedChoice()
    {
        TestConsole console = new();
        _ = console.Interactive();
        console.Input.PushKey(ConsoleKey.DownArrow);
        console.Input.PushKey(ConsoleKey.Enter);
        AnsiConsole.Console = console;

        try
        {
            SpectreConsoleAdventureRenderer renderer = new();
            StoryNode node = new("Node")
            {
                Text = ["Choose"],
                Choices =
                [
                    new Choice("First") { NextNodeId = "next" },
                    new Choice("Second") { NextNodeId = "next" }
                ]
            };

            Choice choice = renderer.GetChoice(node);

            Assert.Equal("Second", choice.Text);
            Assert.Contains("What do you want to do?", console.Output);
        }
        finally
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        }
    }

    [Fact]
    public void SpectreConsoleAdventureRenderer_RenderChoiceAction_WritesOutcomeText()
    {
        TestConsole console = new();
        AnsiConsole.Console = console;

        try
        {
            SpectreConsoleAdventureRenderer renderer = new();
            Choice choice = new("Proceed")
            {
                NextNodeId = "next",
                WhenChosen = ["Outcome line 1", "Outcome line 2"]
            };

            renderer.RenderChoiceAction(choice);

            Assert.Contains("Outcome line 1", console.Output);
            Assert.Contains("Outcome line 2", console.Output);
        }
        finally
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        }
    }

    private static StoryNode? GetCurrentNode(Adventure adventure) => (StoryNode?)typeof(Adventure).GetProperty(nameof(Adventure.CurrentNode))!.GetValue(adventure);

    private sealed class RecordingRenderer(IEnumerable<Choice>? choices = null) : IAdventureRenderer
    {
        private readonly Queue<Choice> _choices = new(choices ?? []);

        public List<StoryNode> RenderedNodes { get; } = [];
        public List<Choice> RenderedActions { get; } = [];

        public Choice GetChoice(StoryNode node) => _choices.Count == 0 ? throw new InvalidOperationException("No choice was supplied for the renderer.") : _choices.Dequeue();

        public void Render(StoryNode node) => RenderedNodes.Add(node);

        public void RenderChoiceAction(Choice choice) => RenderedActions.Add(choice);
    }
}
