namespace NLBInjecto;

public class NlbScope(NlbServiceProviderSnapshot container) : INlbServiceProvider, IDisposable
{
    private readonly Dictionary<Type, object> _scopedInstances = new();

    public TService GetService<TService>(string? name = null)
    {
        return (TService)GetService(typeof(TService), name);
    }

    public object GetService(Type serviceType, string? name = null)
    {
        var descriptor = container.GetServiceDescriptor(serviceType, name);
        
        Type[]? genericArguments = null;
        // Handle open generic types
        if (serviceType.IsGenericType)
        {
            genericArguments = serviceType.GetGenericArguments();
        }

        if (descriptor.Lifetime == NlbServiceLifetime.Scoped)
        {
            if (!_scopedInstances.TryGetValue(serviceType, out var service))
            {
                service = CreateInstance(descriptor.ImplementationType, genericArguments);
                _scopedInstances[serviceType] = service;
            }
            return service;
        }

        return CreateInstance(descriptor.ImplementationType, genericArguments);
    }

    public NlbScope CreateScope()
    {
        return new NlbScope(container);
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
                catch(Exception exc)
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

    public void Dispose()
    {
        _scopedInstances.Clear();
    }
}