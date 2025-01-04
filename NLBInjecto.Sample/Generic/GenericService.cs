namespace NLBInjecto.Sample.Generic;

public interface IGenericService<T>
{
    Guid GetGuid();
}

public class GenericService<T> : IGenericService<T>
{
    private readonly Guid _guid = Guid.NewGuid();

    public Guid GetGuid()
    {
        return _guid;
    }
}