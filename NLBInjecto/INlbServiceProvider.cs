namespace NLBInjecto;

public interface INlbServiceProvider
{
    TService GetService<TService>(string? name = null);
    object GetService(Type serviceType, string? name = null);

    NlbScope CreateScope();
}