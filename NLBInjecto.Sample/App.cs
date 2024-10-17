using NLBInjecto.Sample.Generic;
using NLBInjecto.Sample.Scoped;
using NLBInjecto.Sample.Singleton;
using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Sample;

public class App
{
    private readonly ISingletonService _greetingService;
    private readonly IRepository<App> _repository;
    private readonly IScopedService _scopedService;
    private readonly ITransientService _transientService;

    public App(ISingletonService greetingService,
               IScopedService scopedService, 
               IRepository<App> repository, 
               ITransientService transientService)
    {
        _greetingService = greetingService;
        _scopedService = scopedService;
        _repository = repository;
        _transientService = transientService;
    }

    public void RunSingletonService()
    {
        Console.WriteLine($"Singleton Service ID: {_scopedService.GetGuid()}");
    }
    
    public void RunScopedService()
    {
        Console.WriteLine($"Scoped Service ID: {_scopedService.GetGuid()}");
    }
    
    public void RunTransientService()
    {
        Console.WriteLine($"Transient Service ID: {_transientService.GetGuid()}");
    }
    
    public void RunGenericService()
    {
        _repository.Save(this);
    }
}