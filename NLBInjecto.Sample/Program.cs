using NLBInjecto.Sample.Decorator;
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
        services.AddDecorator<ITransientService, TransientServiceDecorator>();
        
        services.AddTransient<App1, App1>();
        services.AddTransient<App2, App2>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        services.AddTransient<ITransientService, TransientService>("test");
        
        using (var scope = serviceProvider.CreateScope())
        {
            var app1 = scope.GetService<App1>();
            app1.RunScopedService();  // Scoped service instance #1
            app1.RunSingletonService();
            app1.RunTransientService();
            Console.WriteLine("-------end-------");

            var app2 = scope.GetService<App1>();
            app2.RunScopedService();  // Reuses Scoped service instance #1 within the same scope
            app2.RunSingletonService();
            app2.RunTransientService();
            Console.WriteLine("-------end-------");
        }

        using (var scope = serviceProvider.CreateScope())
        {
            var app3 = scope.GetService<App1>();
            app3.RunScopedService();  // New Scoped service instance #2 in a new scope
            app3.RunSingletonService();
            app3.RunTransientService();
            Console.WriteLine("-------end-------");
        }
        
        // Resolve the App service
        var app = serviceProvider.GetService<App2>();
        app.RunTransientService();
        app.RunSingletonService();

        Console.ReadKey();
    }
}
