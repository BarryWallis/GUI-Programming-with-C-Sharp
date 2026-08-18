using EnigmaSimulator;
using EnigmaSimulator.Domain;

using Microsoft.Extensions.DependencyInjection;

using Spectre.Console;
using Spectre.Console.Cli;


try
{
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
        _ = config.AddCommand<EncodeFileCommand>("encode-file")
                                     .WithAlias("ef")
                                     .WithDescription("Encodes a file using the Enigma machine and displays the output.")
                                     .WithExample("encode-file input.txt");
    });
    return app.Run(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    return 1;
}
