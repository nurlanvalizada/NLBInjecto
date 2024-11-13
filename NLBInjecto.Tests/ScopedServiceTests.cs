using NLBInjecto.Sample.Scoped;

namespace NLBInjecto.Tests;

public class ScopedServiceTests
{
    [Fact]
    public void ScopedService_ShouldReturnSameInstanceWithinScope()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddScoped<IScopedService, ScopedService>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        
        var instance1 = scope.GetService<IScopedService>();
        var instance2 = scope.GetService<IScopedService>();

        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void ScopedService_ShouldReturnDifferentInstancesAcrossScopes()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddScoped<IScopedService, ScopedService>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        using var scope1 = serviceProvider.CreateScope();
        using var scope2 = serviceProvider.CreateScope();
        
        var instance1 = scope1.GetService<IScopedService>();
        var instance2 = scope2.GetService<IScopedService>();

        Assert.NotSame(instance1, instance2);
    }

    [Fact]
    public void ScopedService_ShouldThrowExceptionWhenServiceNotRegistered()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        using var scope1 = serviceProvider.CreateScope();

        Assert.Throws<InvalidOperationException>(() => scope1.GetService<IScopedService>());
    }

    [Fact]
    public void ScopedService_ShouldResolveDependenciesCorrectly()
    {
        // Arrange
        var serviceCollection = new NlbServiceCollection();
        serviceCollection.AddScoped<IScopedService, ScopedService>();
        serviceCollection.AddScoped<IScopedServiceWithDependency, ScopedServiceWithDependency>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var service = scope.GetService<IScopedServiceWithDependency>();

        Assert.NotNull(service);
        Assert.NotNull(service.Dependency);
        Assert.True(service.Dependency is ScopedService);
    }
}