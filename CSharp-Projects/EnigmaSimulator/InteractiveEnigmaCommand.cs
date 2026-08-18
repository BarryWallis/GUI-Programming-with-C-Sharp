using System;
using System.Collections.Generic;
using System.Text;

using EnigmaSimulator.Domain;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EnigmaSimulator;

/// <summary>
/// Represents a command for interacting with the Enigma machine in a command-line interface.
/// </summary>
/// <param name="enigma"></param>
public class InteractiveEnigmaCommand(EnigmaMachine enigma) : Command
{
    /// <inheritdoc/>
    protected override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        AnsiConsole.Write(new FigletText("Enigma").Color(Color.Green));
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("Enigma will encode until you press [cyan]enter[/].");
        AnsiConsole.WriteLine();
        char output;
        do
        {
            ConsoleKeyInfo? key = AnsiConsole.Console.Input.ReadKey(true);
            char input = key.GetValueOrDefault().KeyChar;
            output = enigma.Encode(input);
            AnsiConsole.Write(output);
        } while (!Environment.NewLine.Contains(output));

        return 0;
    }
}
