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
    
    public void AddScoped<TService, TImplementation>(string? name = null) where TImplementation : TService
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TImplementation), NlbServiceLifetime.Scoped, name));
    }
    
    public void AddScoped(Type serviceType, Type implementationType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, implementationType, NlbServiceLifetime.Scoped, name));
    }

    public void AddSingleton<TService, TImplementation>(string? name = null) where TImplementation : TService
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TImplementation), NlbServiceLifetime.Singleton, name));
    }
    
    public void AddSingleton(Type serviceType, Type implementationType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, implementationType, NlbServiceLifetime.Singleton, name));
    }
    
    public void AddDecorator<TService, TDecorator>(string? name = null) where TDecorator : TService
    {
        var descriptor = GetServiceDescriptor(typeof(TService), name);

        var originalImplementationType = descriptor.ImplementationType;
        var originalLifetime = descriptor.Lifetime;
            
        _services.Remove(descriptor);
            
        // Add a new descriptor that wraps the original implementation within the decorator
        _services.Add(new NlbServiceDescriptor(typeof(TService), provider =>
        {
            object originalInstance = provider switch
            {
                NlbScope scope => InstanceCreatorHelper.CreateInstance(originalImplementationType, scope.GetService),
                NlbServiceProviderSnapshot snapshot => InstanceCreatorHelper.CreateInstance(originalImplementationType, snapshot.GetService),
                _ => throw new Exception("Unsupported provider type.")
            };

            // Resolve the decorator with the original instance
            return Activator.CreateInstance(typeof(TDecorator), originalInstance)!;
        }, originalLifetime));
    }
    
    public NlbServiceDescriptor GetServiceDescriptor(Type serviceType, string? name = null)
    {
        var descriptor = _services.FirstOrDefault(s => s.ServiceType == serviceType && s.Name == name);
        if(descriptor == null && serviceType.IsGenericType)
        {
            // If no direct match is found and the requested service is a closed generic,
            // look for an open generic definition that matches the generic type definition.
            var genericDefinition = serviceType.GetGenericTypeDefinition();
            descriptor = _services.FirstOrDefault(s => s.ServiceType.IsGenericTypeDefinition &&
                                                       s.ServiceType.GetGenericTypeDefinition() == genericDefinition);
        }
        
        return descriptor ?? throw new InvalidOperationException($"Service of type {serviceType.Name} with name {name} is not registered.");
    }

    public IReadOnlyList<NlbServiceDescriptor> Services => _services.ToList().AsReadOnly();

    public INlbServiceProvider BuildServiceProvider()
    {
        return new NlbServiceProviderSnapshot(this);
    }
}