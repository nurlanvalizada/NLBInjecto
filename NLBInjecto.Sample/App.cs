using NLBInjecto.Sample.Generic;
using NLBInjecto.Sample.Scoped;
using NLBInjecto.Sample.Singleton;

namespace NLBInjecto.Sample;

public class App
{
    private readonly IGreetingService _greetingService;
    private readonly IRepository<App> _repository;
    private readonly IScopedService _scopedService;

    public App(IGreetingService greetingService, IScopedService scopedService, IRepository<App> repository)
    {
        _greetingService = greetingService;
        _scopedService = scopedService;
        _repository = repository;
    }

    public void RunTransientService()
    {
        _greetingService.Greet("World");
    }
    
    public void RunScopedService()
    {
        Console.WriteLine($"Scoped Service ID: {_scopedService.GetGuid()}");
    }
    
    public void RunGenericService()
    {
        _repository.Save(this);
    }
}