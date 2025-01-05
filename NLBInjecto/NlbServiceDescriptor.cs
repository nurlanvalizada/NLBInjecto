namespace NLBInjecto;

public class NlbServiceDescriptor(Type serviceType, Type implementationType, NlbServiceLifetime lifetime, string? name = null)
{
    public NlbServiceDescriptor(Type serviceType, Func<INlbServiceProvider, Type[]?, object> factory, NlbServiceLifetime lifetime, string? name = null)
        : this(serviceType, factory.Method.ReturnType, lifetime, name)
    {
        Factory = factory;
    }

    public string? Name { get; } = name;
    public Type ServiceType { get; set; } = serviceType;
    public Type ImplementationType { get; } = implementationType;

    /// <summary>
    /// Here we store the factory function that will be used to create the service instance.
    /// Type[]? is used to pass generic arguments to the factory function.
    /// </summary>
    public Func<INlbServiceProvider, Type[]?, object>? Factory { get; }

    public NlbServiceLifetime Lifetime { get; } = lifetime;
    private object? Implementation { get; set; }
    
    public object? GetImplementation(Type type)
    {
        return type.IsGenericType 
            ? _closedGenericImplementations.GetValueOrDefault(type) 
            : Implementation;
    }
    
    public void SetImplementation(Type type, object implementation)
    {
        if(type.IsGenericType)
        {
            _closedGenericImplementations.Add(type, implementation);
        }
        else
        {
            Implementation = implementation;
        }
    }
    
    private readonly Dictionary<Type, object> _closedGenericImplementations = new();
}