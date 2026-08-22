using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Renderers;

/// <summary>
/// Renders the player party's current stats (HP, MP, and enemies defeated) as a
/// Specter.Console bar chart inside a named panel.
/// </summary>
/// <param name="party">The <see cref="PlayerParty"/> whose stats are displayed.</param>
public class PartyRenderer(PlayerParty party)
{
    /// <summary>
    /// Builds and returns a panel containing a bar chart of the party's statistics.
    /// </summary>
    /// <returns>An <see cref="IRenderable"/> panel showing HP, MP, and enemies defeated.</returns>
    public IRenderable GenerateVisual()
    {
        IRenderable partyMarkdown = new Rows(new Markup("[bold]Hero[/]"),
                                             new Padder(new BarChart().AddItem("[red]HP[/]", party.Health, Color.Red)
                                                                      .AddItem("[blue]MP[/]", party.Mana, Color.Blue)
                                                                      .AddItem("[green]Enemies Defeated[/]",
                                                                               party.EnemiesDefeated, Color.Green)
                                                                      .WithMaxValue(PlayerParty.MaxStat)
                                                                      .ShowValues()
                                             ));
        return new Panel(new Rows(partyMarkdown)).Header($"[yellow] {party.Name} [/]")
                                                 .Border(BoxBorder.Rounded);
    }
}