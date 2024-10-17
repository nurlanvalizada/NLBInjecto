namespace NLBInjecto;

public interface INlbServiceCollection
{
    void AddTransient<TService, TImplementation>() where TImplementation : TService;
    void AddSingleton<TService, TImplementation>() where TImplementation : TService;
    object GetService(Type serviceType);
    object GetService(Type serviceType, string? name);
    TService GetService<TService>();
    TService GetService<TService>(string? name);
}