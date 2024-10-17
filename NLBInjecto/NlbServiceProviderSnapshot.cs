namespace NLBInjecto;

public class NlbServiceProviderSnapshot(List<NlbServiceDescriptor> serviceDescriptors) : INlbServiceProvider
{
    private readonly IReadOnlyList<NlbServiceDescriptor> _serviceDescriptors = serviceDescriptors.ToList().AsReadOnly();

    public object GetService(Type serviceType, string? name = null)
    {
        var descriptor = _serviceDescriptors.First(s => s.ServiceType == serviceType && (name is null || s.Name == name));
        if(descriptor == null)
        {
            throw new Exception($"Service of type {serviceType.Name} with name {name} is not registered.");
        }

        // Use the factory function if available
        if(descriptor.Factory != null)
        {
            return descriptor.Factory(this);
        }

        if(descriptor.Lifetime == NlbServiceLifetime.Singleton)
        {
            return descriptor.Implementation ?? (descriptor.Implementation = CreateInstance(descriptor.ImplementationType));
        }

        if(descriptor.Lifetime == NlbServiceLifetime.Scoped)
        {
            throw new InvalidOperationException("Cannot resolve scoped services from root provider.");
        }

        return CreateInstance(descriptor.ImplementationType);
    }

    public TService GetService<TService>(string? name = null)
    {
        return (TService)GetService(typeof(TService), name);
    }

    public NlbServiceDescriptor GetServiceDescriptor(Type serviceType, string? name = null)
    {
        var descriptor = _serviceDescriptors.FirstOrDefault(s => s.ServiceType == serviceType && s.Name == name);
        if (descriptor == null && serviceType.IsGenericType)
        {
            // If no direct match is found and the requested service is a closed generic,
            // look for an open generic definition that matches the generic type definition.
            var genericDefinition = serviceType.GetGenericTypeDefinition();
            descriptor = _serviceDescriptors.FirstOrDefault(s => s.ServiceType.IsGenericTypeDefinition &&
                                                                 s.ServiceType.GetGenericTypeDefinition() == genericDefinition);
        }
        
        return descriptor ?? throw new Exception($"Service of type {serviceType.Name} with name {name} is not registered.");
    }

    public NlbScope CreateScope()
    {
        return new NlbScope(this);
    }
    public object CreateInstance(Type implementationType, Type[]? genericArguments = null)
    {
        if(implementationType.IsGenericTypeDefinition)
        {
            if(genericArguments == null || genericArguments.Length == 0)
                throw new InvalidOperationException("Generic type arguments required");

            implementationType = implementationType.MakeGenericType(genericArguments);
        }

        var constructors = implementationType.GetConstructors().OrderByDescending(c => c.GetParameters().Length);
        foreach(var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var parameterInstances = new object[parameters.Length];

            bool canResolveAllParameters = true;
            for(int i = 0; i < parameters.Length; i++)
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

            if(canResolveAllParameters)
            {
                return Activator.CreateInstance(implementationType, parameterInstances);
            }
        }

        throw new Exception($"Cannot resolve parameters for {implementationType.Name}");
    }
}