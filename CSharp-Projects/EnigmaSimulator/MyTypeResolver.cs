using Spectre.Console.Cli;

internal class MyTypeResolver(IServiceProvider provider) : ITypeResolver
{
    private readonly IServiceProvider _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    public object? Resolve(Type? type) => type switch
    {
        null => null,
        _ => _provider.GetService(type)
    };
}