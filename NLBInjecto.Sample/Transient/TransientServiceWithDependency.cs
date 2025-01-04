namespace NLBInjecto.Sample.Transient;

public interface ITransientServiceWithDependency
{
    ITransientService Dependency { get; }
    Guid GetGuid();
}

public class TransientServiceWithDependency(ITransientService dependency) : ITransientServiceWithDependency
{
    private readonly Guid _guid = Guid.NewGuid();

    public ITransientService Dependency { get; } = dependency;
    public Guid GetGuid() => _guid;
}