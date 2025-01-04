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
        services.AddDecorator<ISingletonService, SingletonServiceDecorator>();
        
        services.AddScoped<IScopedService, ScopedService>();
        services.AddDecorator<IScopedService, ScopedServiceDecorator>();
        
        services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddDecorator(typeof(IGenericService<>), typeof(GenericServiceDecorator<>));
        
        services.AddTransient<ITransientService, TransientService>("test");
        services.AddTransient<ITransientService>((provider, types) => new TransientServiceKeyed());
        services.AddDecorator<ITransientService, TransientServiceDecorator>();
        
        services.AddTransient<App1>();
        services.AddTransient<App2>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        using (var scope = serviceProvider.CreateScope())
        {
            Console.WriteLine("-------start-same-scoped-------\r\n");
            
            Console.WriteLine("-------App1-first-start-------");
            var app11 = scope.GetService<App1>();
            app11.RunAllServices();
            Console.WriteLine("-------App1-first-end-------\r\n");

            Console.WriteLine("-------App1-second-start-------");
            var app12 = scope.GetService<App1>();
            app12.RunAllServices();
            Console.WriteLine("-------App1-second-end-------\r\n");
           
            Console.WriteLine("-------App2-first-start-------");
            var app21 = scope.GetService<App2>();
            app21.RunAllServices();
            Console.WriteLine("-------App2-first-end-------\r\n");
            
            Console.WriteLine("-------App2-second-start-------");
            var app22 = scope.GetService<App2>();
            app22.RunAllServices();
            Console.WriteLine("-------App2-second-end-------\r\n");
            
            Console.WriteLine("-------end-same-scoped-------\r\n");
        }

        using (var scope = serviceProvider.CreateScope())
        {
            Console.WriteLine("-------start-different-scoped-------\r\n");
            
            Console.WriteLine("-------App1-first-start-------");
            var app11 = scope.GetService<App1>();
            app11.RunAllServices();
            Console.WriteLine("-------App1-first-end-------\r\n");

            Console.WriteLine("-------App1-second-start-------");
            var app12 = scope.GetService<App1>();
            app12.RunAllServices();
            Console.WriteLine("-------App1-second-end-------\r\n");
           
            Console.WriteLine("-------App2-first-start-------");
            var app21 = scope.GetService<App2>();
            app21.RunAllServices();
            Console.WriteLine("-------App2-first-end-------\r\n");
            
            Console.WriteLine("-------App2-second-start-------");
            var app22 = scope.GetService<App2>();
            app22.RunAllServices();
            Console.WriteLine("-------App2-second-end-------\r\n");
            
            Console.WriteLine("-------end-different-scoped-------\r\n");
        }
        
        var transientService = serviceProvider.GetService<ITransientService>("test");
        Console.WriteLine("Transient keyed service: " + transientService.GetGuid());

        Console.ReadKey();
    }
}
