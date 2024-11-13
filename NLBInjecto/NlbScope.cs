namespace NLBInjecto;

public class NlbScope(INlbServiceCollection serviceCollection) : INlbServiceProvider, IDisposable
{
    private readonly Dictionary<Type, object> _scopedInstances = new();

    public TService GetService<TService>(string? name = null)
    {
        return (TService)GetService(typeof(TService), name);
    }

    public object GetService(Type serviceType, string? name = null)
    {
        var descriptor = serviceCollection.GetServiceDescriptor(serviceType, name);
        if(descriptor == null)
        {
            throw new Exception($"Service of type {serviceType.Name} with name {name} is not registered.");
        }
        
        Type[]? genericArguments = null;
        if (serviceType.IsGenericType) // Handle open generic types
        {
            genericArguments = serviceType.GetGenericArguments();
        }

        if (descriptor.Lifetime == NlbServiceLifetime.Scoped)
        {
            if (!_scopedInstances.TryGetValue(serviceType, out var service))
            {
                if(descriptor.Factory != null)
                {
                    descriptor.Implementation = descriptor.Factory(this);
                    return descriptor.Implementation;
                }
                
                service = InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments);
                _scopedInstances[serviceType] = service;
            }
            return service;
        }

        if(descriptor.Lifetime == NlbServiceLifetime.Singleton)
        {
            if(descriptor.Implementation != null)
            {
                return descriptor.Implementation;
            }
            
            
            // Use the factory function if available
            if(descriptor.Factory != null)
            {
                descriptor.Implementation = descriptor.Factory(this);
                return descriptor.Implementation;
            }
            
            descriptor.Implementation = InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments);
            return descriptor.Implementation;
        }
        
        if(descriptor.Factory != null)
        {
            descriptor.Implementation = descriptor.Factory(this);
            return descriptor.Implementation;
        }

        descriptor.Implementation = InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments);
        return descriptor.Implementation;
    }

    public NlbScope CreateScope()
    {
        return new NlbScope(serviceCollection);
    }

    public void Dispose()
    {
        _scopedInstances.Clear();
    }
}