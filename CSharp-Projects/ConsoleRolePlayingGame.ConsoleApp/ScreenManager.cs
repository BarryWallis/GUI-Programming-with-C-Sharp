using System.Diagnostics;

using ConsoleRolePlayingGame.ConsoleApp;

using Spectre.Console;

internal class ScreenManager(GameManager game, IAnsiConsole console, OverworldScreen overworld)
{
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