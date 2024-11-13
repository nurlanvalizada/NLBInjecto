namespace NLBInjecto.Sample.Singleton;

public interface ISingletonService
{
    Guid GetGuid();
}

public class SingletonService : ISingletonService
{
    public SingletonService()
    {
        _guid = Guid.NewGuid();
        //Console.WriteLine("SingletonService created: " + _guid);
    }
    
    private readonly Guid _guid ;
    public Guid GetGuid() => _guid;
}