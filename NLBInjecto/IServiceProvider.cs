namespace NLBInjecto;

public interface INlbServiceProvider
{
    TService GetService<TService>();
    object GetService(Type serviceType);
}