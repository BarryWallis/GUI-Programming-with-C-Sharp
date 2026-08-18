using Microsoft.Extensions.DependencyInjection;

using Spectre.Console.Cli;

namespace EnigmaSimulator;

/// <summary>
/// Represents a custom type registrar for <see cref="Spectre.Console.Cli"/> that uses Microsoft.Extensions.DependencyInjection for dependency injection.
/// </summary>
/// <remarks>
/// Initializes a new instance of the MyTypeRegistrar class with the specified IServiceCollection.
/// </remarks>
/// <param name="services">The IServiceCollection to use for registering services.</param>
public class MyTypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    private readonly IServiceCollection _services = services;

    /// <summary>
    /// Builds and returns an instance of ITypeResolver that resolves types using the registered services.
    /// </summary>
    /// <returns>An instance of ITypeResolver that resolves types using the registered services.</returns>
    public ITypeResolver Build() => new MyTypeResolver(_services.BuildServiceProvider());

    /// <summary>
    /// Registers a service type and its implementation type with the service collection.
    /// </summary>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementation">The type of the implementation to register.</param>
    public void Register(Type service, Type implementation) => _services.AddSingleton(service, implementation);

    /// <summary>
    /// Registers a service type and its implementation instance with the service collection.
    /// </summary>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="implementation">The instance of the implementation to register.</param>
    public void RegisterInstance(Type service, object implementation) => _services.AddSingleton(service, implementation);

    /// <summary>
    /// Registers a service type with a factory method that creates the implementation instance when needed.
    /// </summary>
    /// <param name="service">The type of the service to register.</param>
    /// <param name="factory">A factory method that creates the implementation instance.</param>
    public void RegisterLazy(Type service, Func<object> factory) => _services.AddSingleton(service, provider => factory());
}