namespace NLBInjecto.Sample.Singleton;

public interface ISingletonService
{
    Guid GetGuid();
}

public class SingletonService : ISingletonService
{
    private readonly Guid _guid = Guid.NewGuid();
    public Guid GetGuid() => _guid;
}