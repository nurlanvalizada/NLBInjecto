namespace NLBInjecto.Sample.Scoped;

public interface IScopedServiceWithDependency
{
    IScopedService Dependency { get; }
    Guid GetGuid();
}

public class ScopedServiceWithDependency(IScopedService scopedService) : IScopedServiceWithDependency
{
    private readonly Guid _guid = Guid.NewGuid();

    public IScopedService Dependency { get; } = scopedService;
    public Guid GetGuid() => _guid;
}