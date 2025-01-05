using NLBInjecto.Exceptions;
using NLBInjecto.Sample.Decorator;
using NLBInjecto.Sample.Singleton;

namespace NLBInjecto.Tests;

public class SingletonServiceTests
{
    [Fact]
    public void SingletonService_ShouldReturnSameInstanceEachTime()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddSingleton<ISingletonService, SingletonService>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act
        var instance1 = serviceProvider.GetService<ISingletonService>();
        var instance2 = serviceProvider.GetService<ISingletonService>();

        // Assert
        Assert.Same(instance1, instance2);
    }
    
    [Fact]
    public void SingletonService_ShouldThrowExceptionWhenServiceIsNotRegistered()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act & Assert
        Assert.Throws<NlbServiceIsNotRegisteredException>(() => serviceProvider.GetService<ISingletonService>());
    }

    [Fact]
    public void SingletonService_ShouldResolveDependenciesCorrectly()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddSingleton<ISingletonService, SingletonService>();
        serviceCollection.AddSingleton<ISingletonServiceWithDependency, SingletonServiceWithDependency>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Act
        var service = serviceProvider.GetService<ISingletonServiceWithDependency>();

        // Assert
        Assert.NotNull(service);
        Assert.NotNull(service.Dependency);
        Assert.True(service.Dependency is SingletonService);
    }
    
    [Fact]
    public void SingletonService_ShouldDecorateServiceCorrectly()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddSingleton<ISingletonService, SingletonService>();
        serviceCollection.AddDecorator<ISingletonService, SingletonServiceDecorator>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        // Act
        var service = serviceProvider.GetService<ISingletonService>();
        
        // Assert
        Assert.NotNull(service);
        Assert.IsType<SingletonServiceDecorator>(service);
         
        var decorator = service as SingletonServiceDecorator;
        Assert.NotNull(decorator);
        Assert.NotNull(decorator.InnerService);
        Assert.True(decorator.InnerService is SingletonService);
    }
}