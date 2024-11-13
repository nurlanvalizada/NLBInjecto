namespace NLBInjecto.Sample.Scoped;

public interface IScopedService
{
    Guid GetGuid();
}

public class ScopedService : IScopedService
{
    public ScopedService()
    {
        _guid = Guid.NewGuid();
        //Console.WriteLine("Scoped Service created: " + _guid);
    }

    private readonly Guid _guid;
    public Guid GetGuid() => _guid;
}