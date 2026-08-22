using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.ConsoleApp.Renderers;
using ConsoleRolePlayingGame.Overworld;

using Shouldly;
using Spectre.Console;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;

namespace ConsoleRolePlayingGame.ConsoleApp.Tests;

/// <summary>
/// Unit tests for console presentation and screen input behavior.
/// </summary>
public class PresentationTests
{
    /// <summary>
    /// Verifies that the help renderer output includes core movement and quit instructions.
    /// </summary>
    [Fact]
    public void HelpRenderer_GenerateVisual_WritesControlInstructions()
    {
        TestConsole console = new();

        console.Write(HelpRenderer.GenerateVisual());

        console.Output.ShouldContain("yellow block");
        console.Output.ShouldContain("arrow keys");
        console.Output.ShouldContain("Press q to quit");
    }

    /// <summary>
    /// Verifies that the party renderer output includes the party name and tracked stats.
    /// </summary>
    [Fact]
    public void PartyRenderer_GenerateVisual_WritesPartyStatistics()
    {
        PlayerParty party = new()
        {
            Name = "Heroes"
        };
        party.EnemyDefeated();
        party.EnemyDefeated();
        PartyRenderer renderer = new(party);
        TestConsole console = new();

        console.Write(renderer.GenerateVisual());

        console.Output.ShouldContain("Heroes");
        console.Output.ShouldContain("HP");
        console.Output.ShouldContain("MP");
        console.Output.ShouldContain("Enemies Defeated");
    }

    /// <summary>
    /// Verifies that map rendering returns a canvas and produces visible output.
    /// </summary>
    [Fact]
    public void MapRenderer_GenerateVisual_ReturnsCanvas()
    {
        GameManager game = TestHelpers.CreateGameManager();
        game.Map.AddEntity(new EnemyGroup(new Position(1, 0)));
        MapRenderer renderer = new(game, 5, 5);
        TestConsole console = new();

        IRenderable renderable = renderer.GenerateVisual();
        console.Write(renderable);

        _ = renderable.ShouldBeOfType<Canvas>();
        console.Output.ShouldNotBeNullOrWhiteSpace();
    }

    /// <summary>
    /// Verifies that overworld visual generation includes header, party details, and instructions.
    /// </summary>
    [Fact]
    public void OverworldScreen_GenerateVisual_WritesMapPartyAndHelpContent()
    {
        GameManager game = TestHelpers.CreateGameManager(partyName: "Heroes");
        TestConsole console = new();
        OverworldScreen screen = new(game, console);

        console.Write(screen.GenerateVisual());

        console.Output.ShouldContain("Console Role Playing Game");
        console.Output.ShouldContain("Heroes");
        console.Output.ShouldContain("Instructions");
    }

    /// <summary>
    /// Verifies that movement key input translates into the expected party position changes.
    /// </summary>
    [Theory]
    [InlineData(ConsoleKey.W, 0, -1)]
    [InlineData(ConsoleKey.A, -1, 0)]
    [InlineData(ConsoleKey.S, 0, 1)]
    [InlineData(ConsoleKey.D, 1, 0)]
    [InlineData(ConsoleKey.UpArrow, 0, -1)]
    [InlineData(ConsoleKey.LeftArrow, -1, 0)]
    [InlineData(ConsoleKey.DownArrow, 0, 1)]
    [InlineData(ConsoleKey.RightArrow, 1, 0)]
    public void OverworldScreen_HandlePlayerInput_MovesPartyForDirectionKeys(ConsoleKey key, int expectedX, int expectedY)
    {
        GameManager game = TestHelpers.CreateGameManager();
        TestConsole console = new();
        _ = console.Interactive();
        console.Input.PushKey(key);
        OverworldScreen screen = new(game, console);

        screen.HandlePlayerInput();

        game.Party.MapPosition.ShouldBe(new Position(expectedX, expectedY));
    }

    /// <summary>
    /// Verifies that quit keys transition the game to the terminated status.
    /// </summary>
    [Theory]
    [InlineData(ConsoleKey.Q)]
    [InlineData(ConsoleKey.Escape)]
    public void OverworldScreen_HandlePlayerInput_TerminatesGameForExitKeys(ConsoleKey key)
    {
        GameManager game = TestHelpers.CreateGameManager();
        TestConsole console = new();
        _ = console.Interactive();
        console.Input.PushKey(key);
        OverworldScreen screen = new(game, console);

        screen.HandlePlayerInput();

        game.Status.ShouldBe(GameStatus.Terminated);
    }
}
