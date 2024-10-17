using NLBInjecto.Sample.Generic;
using NLBInjecto.Sample.Scoped;
using NLBInjecto.Sample.Singleton;
using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Sample;

public static class Program
{
    public static void Main()
    {
        var services = new NlbServiceCollection();
        services.AddSingleton<ISingletonService, SingletonService>();
        services.AddScoped<IScopedService, ScopedService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<ITransientService, TransientService>();
        
        services.AddTransient<App, App>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        services.AddTransient<ITransientService, TransientService>("test");
        
        using (var scope = serviceProvider.CreateScope())
        {
            var app1 = scope.GetService<App>();
            app1.RunScopedService();  // Scoped service instance #1

            var app2 = scope.GetService<App>();
            app2.RunScopedService();  // Reuses Scoped service instance #1 within the same scope
        }

        using (var scope = serviceProvider.CreateScope())
        {
            var app3 = scope.GetService<App>();
            app3.RunScopedService();  // New Scoped service instance #2 in a new scope
        }
        
        // Resolve the App service
        var app = serviceProvider.GetService<App>();
        app.RunTransientService();
        app.RunGenericService();
    }
}
