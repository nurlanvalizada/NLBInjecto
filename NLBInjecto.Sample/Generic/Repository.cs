namespace NLBInjecto.Sample.Generic;

public interface IRepository<T>
{
    void Save(T entity);
}

public class Repository<T> : IRepository<T>
{
    private readonly Guid _guid = Guid.NewGuid();
    
    public void Save(T entity)
    {
        Console.WriteLine($"Saving {entity} with {_guid}");
    }
}