using EnigmaSimulator;
using EnigmaSimulator.Domain;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;
using Spectre.Console.Cli;


try
{
    AnsiConsole.Write(new FigletText("Enigma").Color(Color.Green));
    AnsiConsole.WriteLine();

    ServiceCollection services = new();
    _ = services.AddScoped<EnigmaMachine>(static _ => new EnigmaMachine(new Plugboard(),
                                                                        new Rotor(RotorSets.Enigma3),
                                                                        new Rotor(RotorSets.Enigma2),
                                                                        new Rotor(RotorSets.Enigma1),
                                                                        new Reflector(ReflectorSets.ReflectorB))
    ); 
    CommandApp app = new(new MyTypeRegistrar(services));
    app.Configure(static config
                    =>
    {
        _ = config.AddCommand<InteractiveEnigmaCommand>("interactive")
                                     .WithAlias("i")
                                     .WithDescription("Encrypts keystrokes as you type them using Enigma.");
        _ = config.AddCommand<EncodeCommand>("encode")
                                     .WithAlias("e")
                                     .WithDescription("Encodes a message using the Enigma machine and displays the output.")
                                     .WithExample("encode hello");
    });
    return app.Run(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    return 1;
}
