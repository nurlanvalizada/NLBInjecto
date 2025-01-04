using NLBInjecto.Sample.Singleton;

namespace NLBInjecto.Sample.Decorator;

public class SingletonServiceDecorator(ISingletonService singletonService) : ISingletonService
{
    public Guid GetGuid()
    {
        var guid = singletonService.GetGuid();
        Console.WriteLine("Decorated singleton: " + guid);
        return guid;
    }
}