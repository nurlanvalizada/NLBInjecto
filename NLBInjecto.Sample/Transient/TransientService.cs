namespace NLBInjecto.Sample.Transient;

public interface ITransientService
{
    Guid GetGuid();
}

public class TransientService : ITransientService
{
    public TransientService()
    {
        _guid = Guid.NewGuid();
        //Console.WriteLine("Transient Service created: " + _guid);
    }

    private readonly Guid _guid;
    public Guid GetGuid() => _guid;
}