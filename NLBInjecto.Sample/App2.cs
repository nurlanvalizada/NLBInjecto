using NLBInjecto.Sample.Generic;
using NLBInjecto.Sample.Scoped;
using NLBInjecto.Sample.Singleton;
using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Sample;

public class App2
{
    private readonly ISingletonService _singletonService;
    private readonly ITransientService _transientService;

    public App2(ISingletonService greetingService,
                ITransientService transientService)
    {
        _singletonService = greetingService;
        _transientService = transientService;
    }

    public void RunSingletonService()
    {
        Console.WriteLine($"Singleton Service ID: {_singletonService.GetGuid()}");
    }

    public void RunTransientService()
    {
        Console.WriteLine($"Transient Service ID: {_transientService.GetGuid()}");
    }
}