using NLBInjecto.Sample.Decorator;
using NLBInjecto.Sample.Generic;

namespace NLBInjecto.Tests;

public class GenericServiceTests
{
    [Fact]
    public void GenericService_ShouldResolveOpenGenericService_WithClosedGenericTypes()
    {
        // Arrange
        var services = new NlbServiceCollection();
        services.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>));

        var provider = services.BuildServiceProvider();

        // Act
        var intService = provider.GetService<IGenericService<int>>();
        var stringService = provider.GetService<IGenericService<string>>();

        // Assert
        Assert.NotNull(intService);
        Assert.NotNull(stringService);

        Assert.IsType<GenericService<int>>(intService);
        Assert.IsType<GenericService<string>>(stringService);
    }
    
    [Fact]
    public void GenericService_WhenSingleton_ShouldResolveSameInstance_WithClosedGenericTypes()
    {
        // Arrange
        var services = new NlbServiceCollection();
        services.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>));

        var provider = services.BuildServiceProvider();

        // Act
        var intService1 = provider.GetService<IGenericService<int>>();
        var intService2 = provider.GetService<IGenericService<int>>();

        // Assert
        Assert.Same(intService1, intService2);
    }  

    [Fact]
    public void GenericService_ShouldDecorateOpenGenericService()
    {
        // Arrange
        var services = new NlbServiceCollection();
        
        services.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>));
        services.AddDecorator(typeof(IGenericService<>), typeof(GenericServiceDecorator<>));

        var provider = services.BuildServiceProvider();

        // Act
        var service = provider.GetService<IGenericService<int>>();

        // Assert
        Assert.IsType<GenericServiceDecorator<int>>(service);

        var decorator = service as GenericServiceDecorator<int>;
        Assert.NotNull(decorator);

        // The decorator should be wrapping the original GenericService<int>
        Assert.IsType<GenericService<int>>(decorator.InnerService);
    }
}