using System;
using System.Collections.Generic;
using System.Text;

using EnigmaSimulator.Domain;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EnigmaSimulator;

/// <summary>
/// Command to encode a message using the Enigma machine.
/// </summary>
/// <param name="enigma">The Enigma machine to use for encoding.</param>
public class EncodeCommand(EnigmaMachine enigma) : Command<EncodeCommand.Settings>
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
        string output = enigma.Encode(settings.Input);
        AnsiConsole.MarkupLine($"[yellow]{output}[/]");
        return 0;
    }

    /// <summary>
    /// Settings for the EncodeCommand.
    /// </summary>
    public class Settings : CommandSettings
    {
        /// <summary>
        /// Gets or sets the input message to be encoded.
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
