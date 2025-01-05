using NLBInjecto.Exceptions;

namespace NLBInjecto;

public class NlbServiceProviderSnapshot(IReadOnlyList<NlbServiceDescriptor> serviceDescriptors) : INlbServiceProvider
{
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
            case NlbServiceLifetime.Singleton:
            {
                var implementationSingletonAvailable = descriptor.GetImplementation(serviceType);
                if(implementationSingletonAvailable != null)
                {
                    return implementationSingletonAvailable;
                }

                // Use the factory function if available
                if(descriptor.Factory != null)
                {
                    var implementationSingletonFactoryBased = descriptor.Factory(this, genericArguments);
                    descriptor.SetImplementation(serviceType, implementationSingletonFactoryBased);
                    return implementationSingletonFactoryBased;
                }

                var implementationSingleton = InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments); 
                descriptor.SetImplementation(serviceType, implementationSingleton);
                return implementationSingleton;
            }
            case NlbServiceLifetime.Scoped:
                throw new NlbScopedServiceCannotBeResolvedException(serviceType.Name);
            case NlbServiceLifetime.Transient:
                // Use the factory function if available
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

    public TService GetService<TService>(string? name = null)
    {
        return (TService)GetService(typeof(TService), name);
    }

    public NlbScope CreateScope()
    {
        return new NlbScope(serviceDescriptors);
    }

    public int GetAllServiceCount()
    {
        return serviceDescriptors.Count;
    }
}