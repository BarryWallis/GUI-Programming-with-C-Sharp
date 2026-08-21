using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using ConsoleRolePlayingGame.ConsoleApp.Renderers;
using ConsoleRolePlayingGame.Overworld;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp;

public class OverworldScreen(GameManager game, IAnsiConsole console)
{
    private readonly HelpRenderer _helpRenderer = new();
    private readonly MapRenderer _mapRenderer = new(game, Width, Width);
    private readonly PartyRenderer _partyRenderer = new(game.Party);

    public const int Width = 21;

    private readonly Layout _layout = new Layout("Root")
        .SplitRows(new Layout("Header").Size(1)
                                       .Update(new Markup("[bold yellow]Console Role Playing Game[/]")),
                   new Layout("Content").Size(Width)

        .SplitColumns(new Layout("Main").Size(Width * 2),
                      new Layout("Sidebar")));

    public IRenderable GenerateVisual()
    {
        _ = _layout["Main"].Update(_mapRenderer.GenerateVisual());
        _ = _layout["Sidebar"].Update(new Rows(_partyRenderer.GenerateVisual(), HelpRenderer.GenerateVisual()));
        return _layout;
    }

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
