using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Sample.Decorator;

public class TransientServiceDecorator : ITransientService
{
    private readonly ITransientService _transientService;

    public TransientServiceDecorator(ITransientService transientService)
    {
        _transientService = transientService;
    }

    public Guid GetGuid()
    {
        var guid = _transientService.GetGuid();
       Console.WriteLine("Decorated: " + guid);
        return guid;
    }
}