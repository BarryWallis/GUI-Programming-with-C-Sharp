using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using ConsoleRolePlayingGame.ConsoleApp.Renderers;
using ConsoleRolePlayingGame.Overworld;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp;

/// <summary>
/// Renders the overworld screen layout and processes player keyboard input
/// to drive party movement or game exit.
/// </summary>
/// <param name="game">The active <see cref="GameManager"/> instance.</param>
/// <param name="console">The Specter.Console <see cref="IAnsiConsole"/> used for output and input.</param>
public class OverworldScreen(GameManager game, IAnsiConsole console)
{
    private readonly HelpRenderer _helpRenderer = new();
    private readonly MapRenderer _mapRenderer = new(game, Width, Width);
    private readonly PartyRenderer _partyRenderer = new(game.Party);

    /// <summary>The width (and height) in characters of the map viewport.</summary>
    public const int Width = 21;

    private readonly Layout _layout = new Layout("Root")
        .SplitRows(new Layout("Header").Size(1)
                                       .Update(new Markup("[bold yellow]Console Role Playing Game[/]")),
                   new Layout("Content").Size(Width)

        .SplitColumns(new Layout("Main").Size(Width * 2),
                      new Layout("Sidebar")));

    /// <summary>
    /// Builds and returns the full overworld <see cref="IRenderable"/> layout, combining
    /// the map, party stats, and help text.
    /// </summary>
    /// <returns>The composed <see cref="IRenderable"/> ready to write to the console.</returns>
    public IRenderable GenerateVisual()
    {
        _ = _layout["Main"].Update(_mapRenderer.GenerateVisual());
        _ = _layout["Sidebar"].Update(new Rows(_partyRenderer.GenerateVisual(), HelpRenderer.GenerateVisual()));
        return _layout;
    }

    /// <summary>
    /// Reads a single key press from the console and translates it into a game action
    /// (movement in a cardinal direction or quitting).
    /// </summary>
    public void HandlePlayerInput()
    {
        console.Markup("[yellow]>[/] ");
        ConsoleKeyInfo? keyInfo = console.Input.ReadKey(true);
        if (keyInfo.HasValue)
        {
            switch (keyInfo.Value.Key)
            {
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    game.MoveParty(Direction.West);
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    game.MoveParty(Direction.East);
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    game.MoveParty(Direction.South);
                    break;
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    game.MoveParty(Direction.North);
                    break;
                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    game.Quit();
                    break;
                default:
                    throw new UnreachableException("Unhandled key input: " + keyInfo.Value.Key);
            }
        }
    }
}
