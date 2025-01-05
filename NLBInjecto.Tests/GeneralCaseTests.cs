using NLBInjecto.Sample.Singleton;
using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Tests;

public class GeneralCaseTests
{
    [Fact]
    public void GeneralCase_ServiceProvider_ShouldNotAcceptNewServicesAfterCreation()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddTransient<ITransientService, TransientService>();
        
        // Act
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceCount1 = serviceProvider.GetAllServiceCount();
        
        serviceCollection.AddSingleton<ISingletonService, SingletonService>();
        var serviceCount2 = serviceProvider.GetAllServiceCount();

        // Assert
        Assert.Equal(serviceCount1, serviceCount2);
    }
}