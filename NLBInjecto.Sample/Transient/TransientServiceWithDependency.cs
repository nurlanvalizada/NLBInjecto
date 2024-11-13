namespace NLBInjecto.Sample.Transient;

public interface ITransientServiceWithDependency
{
    ITransientService Dependency { get; }
    Guid GetGuid();
}

public class TransientServiceWithDependency : ITransientServiceWithDependency
{
    private readonly Guid _guid;
    
    public TransientServiceWithDependency(ITransientService dependency)
    {
        Dependency = dependency;
        _guid = Guid.NewGuid();
    }
    
    public ITransientService Dependency { get; }
    public Guid GetGuid() => _guid;
}