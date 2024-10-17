namespace NLBInjecto;

public interface INlbServiceCollection
{
    void AddTransient<TService, TImplementation>(string? name = null) where TImplementation : TService;
    void AddTransient(Type serviceType, Type implementationType, string? name = null);
    void AddSingleton<TService, TImplementation>(string? name = null) where TImplementation : TService;
    void AddSingleton(Type serviceType, Type implementationType, string? name = null);
    void AddScoped<TService, TImplementation>(string? name = null) where TImplementation : TService;
    void AddScoped(Type serviceType, Type implementationType, string? name = null);
    void AddDecorator<TService, TDecorator>(string? name = null) where TDecorator : TService;
}