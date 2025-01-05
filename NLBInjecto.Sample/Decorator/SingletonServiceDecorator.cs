using NLBInjecto.Sample.Singleton;

namespace NLBInjecto.Sample.Decorator;

public class SingletonServiceDecorator(ISingletonService singletonService) : ISingletonService
{
    public ISingletonService InnerService => singletonService;

    public Guid GetGuid()
    {
        var guid = InnerService.GetGuid();
        Console.WriteLine("Decorated singleton: " + guid);
        return guid;
    }
}