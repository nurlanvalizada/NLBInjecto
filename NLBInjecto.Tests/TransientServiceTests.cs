using NLBInjecto.Sample.Transient;

namespace NLBInjecto.Tests;

public class TransientServiceTests
{
    [Fact]
    public void TransientService_ShouldReturnNewInstanceEachTime()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddTransient<ITransientService, TransientService>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act
        var instance1 = serviceProvider.GetService<ITransientService>();
        var instance2 = serviceProvider.GetService<ITransientService>();

        // Assert
        Assert.NotSame(instance1, instance2);
    }
    
    [Fact]
    public void TransientService_ShouldThrowExceptionWhenServiceNotRegistered()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => serviceProvider.GetService<ITransientService>());
    }

    [Fact]
    public void TransientService_ShouldResolveDependenciesCorrectly()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddTransient<ITransientService, TransientService>();
        serviceCollection.AddTransient<ITransientServiceWithDependency, TransientServiceWithDependency>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<ITransientServiceWithDependency>();

        // Assert
        Assert.NotNull(service);
        Assert.NotNull(service.Dependency);
        Assert.True(service.Dependency is TransientService);
    }
}