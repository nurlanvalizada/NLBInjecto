using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Sample.Decorator;

public class TransientServiceDecorator(ITransientService transientService) : ITransientService
{
    public Guid GetGuid()
    {
        var guid = transientService.GetGuid();
        Console.WriteLine("Decorated transient: " + guid);
        return guid;
    }
}