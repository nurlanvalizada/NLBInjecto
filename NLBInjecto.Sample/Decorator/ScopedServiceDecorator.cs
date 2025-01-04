using NLBInjecto.Sample.Scoped;

namespace NLBInjecto.Sample.Decorator;

public class ScopedServiceDecorator(IScopedService scopedService) : IScopedService
{
    public Guid GetGuid()
    {
        var guid = scopedService.GetGuid();
        Console.WriteLine("Decorated scoped: " + guid);
        return guid;
    }
}