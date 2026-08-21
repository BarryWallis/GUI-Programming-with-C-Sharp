using System.Security.Authentication.ExtendedProtection;

using ConsoleRolePlayingGame.ConsoleApp;
using ConsoleRolePlayingGame.Overworld;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;

IAnsiConsole console = AnsiConsole.Console;

try
{
    ServiceCollection services = new();
    _ = services.AddSingleton<GameManager>();
    _ = services.AddSingleton<IAnsiConsole>(console);
    _ = services.AddSingleton<PerlinNoiseProvider>();
    _ = services.AddSingleton<MapGenerator>();
    _ = services.AddSingleton<WorldMap>();
    _ = services.AddSingleton<OpenPositionSelector>();
    _ = services.AddSingleton<PlayerParty>();
    _ = services.AddTransient<ScreenManager>();
    _ = services.AddTransient<OverworldScreen>();

    ServiceProvider provider = services.BuildServiceProvider();

    GameManager game = provider.GetRequiredService<GameManager>();
    ScreenManager screens = provider.GetRequiredService<ScreenManager>();
    while (game.Status != GameStatus.Terminated)
    {
        screens.Run();
        game.Update();
    }
}
catch (Exception ex)
{
    console.WriteException(ex, ExceptionFormats.ShortenEverything);
    _ = console.Input.ReadKey(false);
}
