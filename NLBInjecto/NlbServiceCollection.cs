namespace NLBInjecto;

public class NlbServiceCollection : INlbServiceCollection
{
    private readonly IList<NlbServiceDescriptor> _services = [];

    #region Transient Services
   
    public void AddTransient<TService, TImplementation>(string? name = null) where TImplementation : TService
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TImplementation), NlbServiceLifetime.Transient, name));
    }

    public void AddTransient<TService>(string? name = null) where TService : class
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TService), NlbServiceLifetime.Transient, name));
    }

    public void AddTransient<TService>(Func<INlbServiceProvider, Type[]?, object> factory, string? name = null) where TService : class
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), factory, NlbServiceLifetime.Transient, name));
    }

    public void AddTransient(Type serviceType, Type implementationType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, implementationType, NlbServiceLifetime.Transient, name));
    }

    public void AddTransient(Type serviceType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, serviceType, NlbServiceLifetime.Transient, name));
    }
    
    public void AddTransient(Type serviceType, Func<INlbServiceProvider, Type[]?, object> factory, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, factory, NlbServiceLifetime.Transient, name));
    }
    
    #endregion
    
    
    #region Scoped Services

    public void AddScoped<TService, TImplementation>(string? name = null) where TImplementation : TService
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TImplementation), NlbServiceLifetime.Scoped, name));
    }

    public void AddScoped<TService>(string? name = null) where TService : class
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TService), NlbServiceLifetime.Scoped, name));
    }
    
    public void AddScoped<TService>(Func<INlbServiceProvider, Type[]?, object> factory, string? name = null) where TService : class
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), factory, NlbServiceLifetime.Scoped, name));
    }

    public void AddScoped(Type serviceType, Type implementationType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, implementationType, NlbServiceLifetime.Scoped, name));
    }

    public void AddScoped(Type serviceType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, serviceType, NlbServiceLifetime.Scoped, name));
    }
    
    public void AddScoped(Type serviceType, Func<INlbServiceProvider, Type[]?, object> factory, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, factory, NlbServiceLifetime.Scoped, name));
    }
    
    #endregion
    
    
    #region Singleton Services

    public void AddSingleton<TService, TImplementation>(string? name = null) where TImplementation : TService
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TImplementation), NlbServiceLifetime.Singleton, name));
    }

    public void AddSingleton<TService>(string? name = null) where TService : class
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), typeof(TService), NlbServiceLifetime.Singleton, name));
    }
    
    public void AddSingleton<TService>(Func<INlbServiceProvider, Type[]?, object> factory, string? name = null) where TService : class
    {
        _services.Add(new NlbServiceDescriptor(typeof(TService), factory, NlbServiceLifetime.Singleton, name));
    }

    public void AddSingleton(Type serviceType, Type implementationType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, implementationType, NlbServiceLifetime.Singleton, name));
    }

    public void AddSingleton(Type serviceType, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, serviceType, NlbServiceLifetime.Singleton, name));
    }
    
    public void AddSingleton(Type serviceType, Func<INlbServiceProvider, Type[]?, object> factory, string? name = null)
    {
        _services.Add(new NlbServiceDescriptor(serviceType, factory, NlbServiceLifetime.Singleton, name));
    }
    
    #endregion

    public void AddDecorator<TService, TDecorator>(string? name = null) where TDecorator : TService
    {
        AddDecorator(typeof(TService), typeof(TDecorator), name);
    }
    
    public void AddDecorator(Type serviceType, Type decoratorType, string? name = null)
    {
        var descriptor = GetServiceDescriptor(serviceType, name);

        var isFactoryBased = descriptor is { Factory: not null, Implementation: null };
        
        var originalImplementationType = descriptor.ImplementationType;
        var originalLifetime = descriptor.Lifetime;

        _services.Remove(descriptor);

        // Add a new descriptor that wraps the original implementation within the decorator
        _services.Add(new NlbServiceDescriptor(serviceType, (provider, genericTypeArguments) =>
        {
            object? originalInstance;
            switch(provider)
            {
                case NlbScope scope:
                    originalInstance = isFactoryBased 
                        ? descriptor.Factory!(scope, genericTypeArguments) 
                        : InstanceCreatorHelper.CreateInstance(originalImplementationType, scope.GetService, genericTypeArguments);
                    break;
                case NlbServiceProviderSnapshot snapshot:
                    originalInstance = isFactoryBased
                        ? descriptor.Factory!(snapshot, genericTypeArguments)
                        : InstanceCreatorHelper.CreateInstance(originalImplementationType, snapshot.GetService, genericTypeArguments);
                    break;
                default:
                    throw new Exception("Unsupported provider type.");
            }

            if(decoratorType.IsGenericTypeDefinition)
            {
                if(genericTypeArguments == null || genericTypeArguments.Length == 0)
                    throw new InvalidOperationException("Generic type arguments required");

                decoratorType = decoratorType.MakeGenericType(genericTypeArguments);
            }

            // Resolve the decorator with the original instance
            return Activator.CreateInstance(decoratorType, originalInstance)!;

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