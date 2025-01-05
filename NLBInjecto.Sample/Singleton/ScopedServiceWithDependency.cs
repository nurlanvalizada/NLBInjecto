namespace NLBInjecto.Sample.Singleton;

public interface ISingletonServiceWithDependency
{
    ISingletonService Dependency { get; }
    Guid GetGuid();
}

public class SingletonServiceWithDependency(ISingletonService singletonService) : ISingletonServiceWithDependency
{
    private readonly Guid _guid = Guid.NewGuid();

    public ISingletonService Dependency { get; } = singletonService;
    public Guid GetGuid() => _guid;
}