namespace NLBInjecto;

public class ServiceDescriptor
{
    public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime, string? name = null)
    {
        Name = name;
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;
    }
    
    public ServiceDescriptor(Type serviceType, Func<NlbServiceCollection, object> factory, ServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        Factory = factory;
        Lifetime = lifetime;
    }

    public string? Name { get; }
    public Type ServiceType { get; }
    public Type ImplementationType { get; }
    public Func<NlbServiceCollection, object>? Factory { get; }
    public ServiceLifetime Lifetime { get; }
    public object? Implementation { get; set; }
}