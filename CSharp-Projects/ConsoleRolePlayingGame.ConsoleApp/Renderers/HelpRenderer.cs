using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Renderers;

/// <summary>
/// Renders a static help panel that explains the game controls to the player.
/// </summary>
public class HelpRenderer 
{
    /// <summary>
    /// Builds and returns the help instructions panel as an <see cref="IRenderable"/>.
    /// </summary>
    /// <returns>A rounded-border <see cref="Panel"/> containing the control instructions.</returns>
    public static IRenderable GenerateVisual()
        => new Panel(new Rows(new Markup("The [yellow]yellow block[/] is your party."),
                              new Markup("Use [cyan]arrow keys[/] or [cyan]WASD[/] to move."),
                              new Markup("Enter [red]red blocks[/] to begin combat"),
                              new Markup("Press [cyan]q[/] to quit."))
            ).Header("[Yellow] Instructions [/]")
             .Padding(1, 1, 1, 0)
             .Border(BoxBorder.Rounded);
}