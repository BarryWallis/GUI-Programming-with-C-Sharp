using System.Diagnostics;

using ConsoleRolePlayingGame.ConsoleApp;

using Spectre.Console;

/// <summary>
/// Manages the active screen in the game loop, delegating rendering and input
/// to the appropriate screen handler based on the current <see cref="GameStatus"/>.
/// </summary>
/// <param name="game">The active <see cref="GameManager"/> instance.</param>
/// <param name="console">The Specter.Console <see cref="IAnsiConsole"/> used for output and input.</param>
/// <param name="overworld">The overworld screen handler.</param>
internal class ScreenManager(GameManager game, IAnsiConsole console, OverworldScreen overworld)
{
    /// <summary>
    /// Clears the console and renders the screen appropriate to the current game status,
    /// then processes one round of player input.
    /// </summary>
    public void Run()
    {
        console.Clear();
        switch (game.Status)
        {
            case GameStatus.Overworld:
                console.Write(overworld.GenerateVisual());
                overworld.HandlePlayerInput();
                break;
            case GameStatus.GameOver:
                console.MarkupLine("[red]Game Over![/]");
                console.MarkupLine("[yellow]Press any key to exit...[/]");
                _ = console.Input.ReadKey(true);
                game.Quit();
                break;
            default:
                throw new UnreachableException($"Unhandled game status: {game.Status}");    
        }
    }
}