using NLBInjecto.Sample.Generic;
using NLBInjecto.Sample.Scoped;
using NLBInjecto.Sample.Singleton;

namespace NLBInjecto.Sample;

public static class Program
{
    public static void Main()
    {
        var services = new NlbServiceCollection();
        services.AddTransient<IGreetingService, GreetingService>();
        services.AddScoped<IScopedService, ScopedService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        services.AddSingleton<App, App>();
        
        using (var scope = services.CreateScope())
        {
            var app1 = scope.GetService<App>();
            app1.RunScopedService();  // Scoped service instance #1

            var app2 = scope.GetService<App>();
            app2.RunScopedService();  // Reuses Scoped service instance #1 within the same scope
        }

        using (var scope = services.CreateScope())
        {
            var app3 = scope.GetService<App>();
            app3.RunScopedService();  // New Scoped service instance #2 in a new scope
        }

        // Resolve the App service
        var app = services.GetService<App>();
        app.RunTransientService();
        app.RunGenericService();
    }
}
