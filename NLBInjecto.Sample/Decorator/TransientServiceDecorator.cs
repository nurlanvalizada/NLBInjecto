using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Sample.Decorator;

public class TransientServiceDecorator(ITransientService transientService) : ITransientService
{
    public ITransientService InnerService => transientService;

    public Guid GetGuid()
    {
        var guid = InnerService.GetGuid();
        Console.WriteLine("Decorated transient: " + guid);
        return guid;
    }
}