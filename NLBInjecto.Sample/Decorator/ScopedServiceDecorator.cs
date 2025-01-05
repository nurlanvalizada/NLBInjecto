using NLBInjecto.Sample.Scoped;

namespace NLBInjecto.Sample.Decorator;

public class ScopedServiceDecorator(IScopedService scopedService) : IScopedService
{
    public IScopedService InnerService => scopedService;

    public Guid GetGuid()
    {
        var guid = InnerService.GetGuid();
        Console.WriteLine("Decorated scoped: " + guid);
        return guid;
    }
}