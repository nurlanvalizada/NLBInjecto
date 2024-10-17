namespace NLBInjecto;

public class NlbServiceDescriptor
{
    public NlbServiceDescriptor(Type serviceType, Type implementationType, NlbServiceLifetime lifetime, string? name = null)
    {
        Name = name;
        ServiceType = serviceType;
        ImplementationType = implementationType;
        Lifetime = lifetime;
    }
    
    public NlbServiceDescriptor(Type serviceType, Func<INlbServiceProvider, object> factory, NlbServiceLifetime lifetime)
    {
        ServiceType = serviceType;
        Factory = factory;
        Lifetime = lifetime;
    }

    public string? Name { get; }
    public Type ServiceType { get; set; }
    public Type ImplementationType { get; }
    public Func<INlbServiceProvider, object>? Factory { get; }
    public NlbServiceLifetime Lifetime { get; }
    public object? Implementation { get; set; }
}