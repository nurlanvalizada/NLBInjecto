namespace NLBInjecto;

public class NlbServiceProviderSnapshot(INlbServiceCollection serviceCollection) : INlbServiceProvider
{
    private readonly IReadOnlyList<NlbServiceDescriptor> _serviceDescriptors = serviceCollection.Services;

    public object GetService(Type serviceType, string? name = null)
    {
        var descriptor = _serviceDescriptors.First(s => s.ServiceType == serviceType && (name is null || s.Name == name));
        if(descriptor == null)
        {
            throw new InvalidOperationException($"Service of type {serviceType.Name} with name {name} is not registered.");
        }
        
        Type[]? genericArguments = null;
        if (serviceType.IsGenericType) // Handle open generic types
        {
            genericArguments = serviceType.GetGenericArguments();
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
                descriptor.Implementation = descriptor.Factory(this, genericArguments);
                return descriptor.Implementation;
            }

            descriptor.Implementation = InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments);
            return descriptor.Implementation;
        }

        if(descriptor.Lifetime == NlbServiceLifetime.Scoped)
        {
            throw new ScopeServiceCannotBeResolvedException(serviceType.Name);
        }
        
        // Use the factory function if available
        if(descriptor.Factory != null)
        {
            descriptor.Implementation = descriptor.Factory(this, genericArguments);
            return descriptor.Implementation;
        }

        descriptor.Implementation = InstanceCreatorHelper.CreateInstance(descriptor.ImplementationType, GetService, genericArguments);
        return descriptor.Implementation;
    }

    public TService GetService<TService>(string? name = null)
    {
        return (TService)GetService(typeof(TService), name);
    }

    public NlbScope CreateScope()
    {
        return new NlbScope(serviceCollection);
    }
}