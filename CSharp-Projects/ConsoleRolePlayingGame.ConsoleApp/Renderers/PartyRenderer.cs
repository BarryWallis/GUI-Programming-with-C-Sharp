using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRolePlayingGame.ConsoleApp.Renderers;

public class PartyRenderer(PlayerParty party)
{
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