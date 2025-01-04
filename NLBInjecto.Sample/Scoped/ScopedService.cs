namespace NLBInjecto.Sample.Scoped;

public interface IScopedService
{
    Guid GetGuid();
}

public class ScopedService : IScopedService
{
    private readonly Guid _guid = Guid.NewGuid();
    public Guid GetGuid() => _guid;
}