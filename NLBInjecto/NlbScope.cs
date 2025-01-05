using NLBInjecto.Exceptions;

namespace NLBInjecto;

public class NlbScope(IReadOnlyList<NlbServiceDescriptor> serviceDescriptors) : INlbServiceProvider, IDisposable
{
    private readonly Dictionary<Type, object> _scopedInstances = new();

    public TService GetService<TService>(string? name = null)
    {
        return (TService)GetService(typeof(TService), name);
    }

    public object GetService(Type serviceType, string? name = null)
    {
        var descriptor = serviceDescriptors.GetServiceDescriptor(serviceType, name);
        if(descriptor == null)
        {
            throw new NlbServiceIsNotRegisteredException(serviceType.Name, name);
        }
        
        Type[]? genericArguments = null;
        if (serviceType.IsGenericType) // Handle open generic types
        {
            genericArguments = serviceType.GetGenericArguments();
        }

        switch(descriptor.Lifetime)
        {
            case NlbServiceLifetime.Scoped:
            {
                if(_scopedInstances.TryGetValue(serviceType, out var service)) 
                    return service;
                
                service = descriptor.Factory != null 
                    ? descriptor.Factory(this, genericArguments) 
                    : InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments);
                
                descriptor.SetImplementation(serviceType, service);
                _scopedInstances[serviceType] = service;
                return service;
            }
            case NlbServiceLifetime.Singleton:
                var implementationSingletonAvailable = descriptor.GetImplementation(serviceType);
                if(implementationSingletonAvailable != null)
                {
                    return implementationSingletonAvailable;
                }

                if(descriptor.Factory != null)
                {
                    var implementationSingletonFactoryBased = descriptor.Factory(this, genericArguments);
                    descriptor.SetImplementation(serviceType, implementationSingletonFactoryBased);
                    return implementationSingletonFactoryBased;
                }
                
                var implementationSingleton = InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments);
                descriptor.SetImplementation(serviceType, implementationSingleton);
                return implementationSingleton;
            case NlbServiceLifetime.Transient:
                if(descriptor.Factory != null)
                {
                    var implementationTransientFactoryBased = descriptor.Factory(this, genericArguments);
                    descriptor.SetImplementation(serviceType, implementationTransientFactoryBased);
                    return implementationTransientFactoryBased;
                }

                var implementationTransient = InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments);
                descriptor.SetImplementation(serviceType, implementationTransient);
                return implementationTransient;
            default:
                throw new NlbInvalidServiceLifetimeException(descriptor.Lifetime);
        }
    }

    public NlbScope CreateScope()
    {
        return new NlbScope(serviceDescriptors);
    }

    public int GetAllServiceCount()
    {
        return serviceDescriptors.Count;
    }

    public void Dispose()
    {
        _scopedInstances.Clear();
    }
}