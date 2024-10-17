namespace NLBInjecto;

public enum ServiceLifetime
{
    Singleton,
    Transient,
    Scoped
}

public class Scope(NlbServiceCollection container) : IServiceProvider, IDisposable
{
    private readonly Dictionary<Type, object> _scopedInstances = new();

    public object GetService(Type serviceType)
    {
        var descriptor = container.GetServiceDescriptor(serviceType);
        if (descriptor == null)
        {
            throw new Exception($"Service of type {serviceType.Name} is not registered.");
        }

        if (descriptor.Lifetime == ServiceLifetime.Scoped)
        {
            if (!_scopedInstances.TryGetValue(serviceType, out var service))
            {
                service = container.CreateInstance(descriptor.ImplementationType);
                _scopedInstances[serviceType] = service;
            }
            return service;
        }

        return container.GetService(serviceType);
    }

    public TService GetService<TService>()
    {
        return (TService)GetService(typeof(TService));
    }

    public void Dispose()
    {
        _scopedInstances.Clear();
    }
}
