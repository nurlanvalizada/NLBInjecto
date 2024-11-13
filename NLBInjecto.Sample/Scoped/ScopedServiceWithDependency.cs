namespace NLBInjecto.Sample.Scoped;

public interface IScopedServiceWithDependency
{
    IScopedService Dependency { get; }
    Guid GetGuid();
}

public class ScopedServiceWithDependency : IScopedServiceWithDependency
{
    private readonly Guid _guid;
    
    public ScopedServiceWithDependency(IScopedService scopedService)
    {
        Dependency = scopedService;
        _guid = Guid.NewGuid();
    }
    
    public IScopedService Dependency { get; }
    public Guid GetGuid() => _guid;
}