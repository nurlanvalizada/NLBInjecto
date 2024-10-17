namespace NLBInjecto.Sample.Singleton;

public interface IGreetingService
{
    void Greet(string name);
}

public class GreetingService : IGreetingService
{
    private readonly Guid _guid = Guid.NewGuid();
    public void Greet(string name)
    {
        Console.WriteLine($"Hello, {name} with {_guid}!");
    }
}