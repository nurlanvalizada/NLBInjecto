namespace NLBInjecto;

public class NlbServiceCollection : INlbServiceCollection, INlbServiceProvider
{
    private readonly List<ServiceDescriptor> _services = [];

    public void AddTransient<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
    }
    
    public void AddTransient<TService, TImplementation>(string name) where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient, name));
    }
    
    public void AddTransient(Type serviceType, Type implementationType)
    {
        _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
    }

    public void AddSingleton<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
    }
    
    public void AddSingleton<TService, TImplementation>(string name) where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton, name));
    }
    
    public void AddSingleton(Type serviceType, Type implementationType)
    {
        _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
    }
    
    public void AddScoped<TService, TImplementation>() where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
    }
    
    public void AddScoped<TService, TImplementation>(string name) where TImplementation : TService
    {
        _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped, name));
    }
    
    public void AddScoped(Type serviceType, Type implementationType)
    {
        _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Scoped));
    }
    
    public void AddDecorator<TService, TDecorator>() where TDecorator : TService
    {
        var descriptor = _services.Find(s => s.ServiceType == typeof(TService));
        if (descriptor != null)
        {
            var originalImplementationType = descriptor.ImplementationType;
            var originalLifetime = descriptor.Lifetime;
            
            _services.Remove(descriptor);
            
            // Add a new descriptor that wraps the original implementation within the decorator
            _services.Add(new ServiceDescriptor(typeof(TService), provider =>
            {
                // Resolve the original service instance
                var originalInstance = provider.CreateInstance(originalImplementationType);

                // Resolve the decorator with the original instance
                return Activator.CreateInstance(typeof(TDecorator), originalInstance);
            }, originalLifetime));
        }
    }
    
    public object GetService(Type serviceType)
    {
       return GetService(serviceType, null);
    }

    public object GetService(Type serviceType, string? name)
    {
        var descriptor = _services.Find(s => s.ServiceType == serviceType && (name is null || s.Name == name));
        if (descriptor == null)
        {
            throw new Exception($"Service of type {serviceType.Name} with name {name} is not registered.");
        }
        
        // Use the factory function if available
        if (descriptor.Factory != null)
        {
            return descriptor.Factory(this);
        }

        if (descriptor.Lifetime == ServiceLifetime.Singleton)
        {
            if (descriptor.Implementation == null)
            {
                descriptor.Implementation = CreateInstance(descriptor.ImplementationType);
            }
            return descriptor.Implementation;
        }
        
        if (descriptor.Lifetime == ServiceLifetime.Scoped)
        {
            throw new InvalidOperationException("Cannot resolve scoped services from root provider.");
        }

        return CreateInstance(descriptor.ImplementationType);
    }

    public TService GetService<TService>()
    {
        return (TService)GetService(typeof(TService));
    }
    
    public TService GetService<TService>(string? name)
    {
        return (TService)GetService(typeof(TService), name);
    }
    
    public ServiceDescriptor GetServiceDescriptor(Type serviceType)
    {
        return _services.Find(s => s.ServiceType == serviceType)!;
    }
    
    public Scope CreateScope()
    {
        return new Scope(this);
    }
    
    public object CreateInstance(Type implementationType, Type[] genericArguments = null)
    {
        if (implementationType.IsGenericTypeDefinition)
        {
            if (genericArguments == null || genericArguments.Length == 0)
                throw new InvalidOperationException("Generic type arguments required");

            implementationType = implementationType.MakeGenericType(genericArguments);
        }
        
        var constructors = implementationType.GetConstructors().OrderByDescending(c => c.GetParameters().Length);
        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var parameterInstances = new object[parameters.Length];

            bool canResolveAllParameters = true;
            for (int i = 0; i < parameters.Length; i++)
            {
                try
                {
                    parameterInstances[i] = GetService(parameters[i].ParameterType);
                }
                catch
                {
                    canResolveAllParameters = false;
                    break;
                }
            }

            if (canResolveAllParameters)
            {
                return Activator.CreateInstance(implementationType, parameterInstances);
            }
        }

        throw new Exception($"Cannot resolve parameters for {implementationType.Name}");
    }
}