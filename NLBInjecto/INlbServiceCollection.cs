namespace NLBInjecto;

public interface INlbServiceCollection
{
    #region Transient Services
    
    void AddTransient<TService, TImplementation>(string? name = null) where TImplementation : TService;
    void AddTransient<TService>(string? name = null) where TService : class;
    void AddTransient<TService>(Func<INlbServiceProvider, Type[]?, object> factory, string? name = null);
    void AddTransient(Type serviceType, Type implementationType, string? name = null);
    void AddTransient(Type serviceType, string? name = null);
    void AddTransient(Type serviceType, Func<INlbServiceProvider, Type[]?, object> factory, string? name = null);
    
    #endregion
    
    #region Scoped Services
    
    void AddScoped<TService, TImplementation>(string? name = null) where TImplementation : TService;
    void AddScoped<TService>(string? name = null) where TService : class;
    void AddScoped<TService>(Func<INlbServiceProvider, Type[]?, object> factory, string? name = null);
    void AddScoped(Type serviceType, Type implementationType, string? name = null);
    void AddScoped(Type serviceType, string? name = null);
    void AddScoped(Type serviceType, Func<INlbServiceProvider, Type[]?, object> factory, string? name = null);
    
    #endregion
    
    #region Singleton Services
    
    void AddSingleton<TService, TImplementation>(string? name = null) where TImplementation : TService;
    void AddSingleton<TService>(string? name = null) where TService : class;
    void AddSingleton<TService>(Func<INlbServiceProvider, Type[]?, object> factory, string? name = null);
    void AddSingleton(Type serviceType, Type implementationType, string? name = null);
    void AddSingleton(Type serviceType, string? name = null);
    void AddSingleton(Type serviceType, Func<INlbServiceProvider, Type[]?, object> factory, string? name = null);
    
    #endregion
    
    void AddDecorator<TService, TDecorator>(string? name = null) where TDecorator : TService;
    void AddDecorator(Type serviceType, Type decoratorType, string? name = null);
    
    IReadOnlyList<NlbServiceDescriptor> Services { get; }
    INlbServiceProvider BuildServiceProvider();
}