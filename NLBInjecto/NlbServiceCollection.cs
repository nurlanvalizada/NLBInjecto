namespace NLBInjecto;

public class NlbServiceCollection : INlbServiceCollection
{
    private readonly List<NlbServiceDescriptor> _services = [];

    public void AddTransient<TService, TImplementation>(string? name = null) where TImplementation : TService
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TImplementation), NlbServiceLifetime.Transient, name));
    }

    public void AddTransient(Type serviceType, Type implementationType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, implementationType, NlbServiceLifetime.Transient, name));
    }

    public void AddSingleton<TService, TImplementation>(string? name = null) where TImplementation : TService
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TImplementation), NlbServiceLifetime.Singleton, name));
    }
    
    public void AddSingleton(Type serviceType, Type implementationType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, implementationType, NlbServiceLifetime.Singleton));
    }
    
    public void AddScoped<TService, TImplementation>(string? name = null) where TImplementation : TService
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TImplementation), NlbServiceLifetime.Scoped, name));
    }
    
    public void AddScoped(Type serviceType, Type implementationType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, implementationType, NlbServiceLifetime.Scoped));
    }
    
    public void AddDecorator<TService, TDecorator>(string? name = null) where TDecorator : TService
    {
        var descriptor = _services.Find(s => s.ServiceType == typeof(TService) && s.Name == name);
        if (descriptor != null)
        {
            var originalImplementationType = descriptor.ImplementationType;
            var originalLifetime = descriptor.Lifetime;
            
            _services.Remove(descriptor);
            
            // Add a new descriptor that wraps the original implementation within the decorator
            _services.Add(new NlbServiceDescriptor(typeof(TService), provider =>
            {
                // Resolve the original service instance
                var originalInstance = ((NlbServiceProviderSnapshot)provider).CreateInstance(originalImplementationType);

                // Resolve the decorator with the original instance
                return Activator.CreateInstance(typeof(TDecorator), originalInstance);
            }, originalLifetime));
        }
    }
    
    public NlbServiceDescriptor GetServiceDescriptor(Type serviceType, string? name = null)
    {
        return _services.Find(s => s.ServiceType == serviceType && s.Name == name)!;
    }
    
    public INlbServiceProvider BuildServiceProvider()
    {
        return new NlbServiceProviderSnapshot(_services);
    }
}