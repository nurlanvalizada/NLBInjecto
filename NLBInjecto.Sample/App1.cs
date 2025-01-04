using NLBInjecto.Sample.Generic;
using NLBInjecto.Sample.Scoped;
using NLBInjecto.Sample.Singleton;
using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Sample;

public class App1(ISingletonService singletonService,
                  IScopedService scopedService,
                  IGenericService<App1> genericService,
                  ITransientService transientService)
{
    public void RunAllServices()
    {
        RunSingletonService();
        RunScopedService();
        RunTransientService();
        RunGenericService();
    }
    
    public void RunSingletonService()
    {
        Console.WriteLine($"Singleton Service ID from App1: {singletonService.GetGuid()}");
    }
    
    public void RunScopedService()
    {
        Console.WriteLine($"Scoped Service ID from App1: {scopedService.GetGuid()}");
    }
    
    public void RunTransientService()
    {
        Console.WriteLine($"Transient Service ID from App1: {transientService.GetGuid()}");
    }
    
    public void RunGenericService()
    {
        Console.WriteLine($"Generic scoped Service ID from App1: {genericService.GetGuid()}");
    }
}