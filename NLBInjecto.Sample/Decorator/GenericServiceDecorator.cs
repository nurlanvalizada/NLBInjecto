using NLBInjecto.Sample.Generic;

namespace NLBInjecto.Sample.Decorator;

public class GenericServiceDecorator<T>(IGenericService<T> genericService) : IGenericService<T>
{
    public Guid GetGuid()
    {
        var guid = genericService.GetGuid();
        Console.WriteLine("Decorated generic: " + guid);
        return guid;
    }
}