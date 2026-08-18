using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using EnigmaSimulator.Domain;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EnigmaSimulator;

/// <summary>
/// Represents a command that encodes a file using an Enigma machine.
/// </summary>
/// <param name="enigma">The Enigma machine instance used for encoding operations.</param>
public class EncodeFileCommand(EnigmaMachine enigma) : Command<EncodeFileCommand.Settings>
{
    /// <summary>
    /// Executes the command with the provided context, settings, and cancellation token.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings for the command.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The exit code.</returns>
    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            string fileContent = File.ReadAllText(settings.Input);
            string output = enigma.Encode(fileContent);
            AnsiConsole.MarkupLine($"[yellow]{output}[/]");
            return 0;
        }
        catch (FileNotFoundException)
        {
            AnsiConsole.MarkupLine($"[red]Error: File not found: {settings.Input}[/]");
            return 1;
        }
        catch (IOException ex)
        {
            AnsiConsole.MarkupLine($"[red]Error reading file: {ex.Message}[/]");
            return 1;
        }
    }

    /// <summary>
    /// Settings for the EncodeFileCommand.
    /// </summary>
    public class Settings : CommandSettings
    {
        /// <summary>
        /// Gets or sets the path to the input file to be encoded.
        /// </summary>
        [CommandArgument(0, "[Input]")]
        public required string Input
        {
            get; init;
        }
    }

    /// <inheritdoc/>
    protected override ValidationResult Validate(CommandContext context, Settings settings)
        => string.IsNullOrWhiteSpace(settings.Input) ? ValidationResult.Error("Input is required")
                                                     : base.Validate(context, settings);
}
