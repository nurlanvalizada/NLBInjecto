namespace NLBInjecto.Sample.Transient;

public interface ITransientService
{
    Guid GetGuid();
}

public class TransientService : ITransientService
{
    private readonly Guid _guid = Guid.NewGuid();
    public Guid GetGuid() => _guid;
}