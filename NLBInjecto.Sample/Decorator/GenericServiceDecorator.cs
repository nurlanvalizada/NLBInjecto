using NLBInjecto.Sample.Generic;

namespace NLBInjecto.Sample.Decorator;

public class GenericServiceDecorator<T>(IGenericService<T> genericService) : IGenericService<T>
{
    public IGenericService<T> InnerService { get; } = genericService;
    public Guid GetGuid()
    {
        var guid = InnerService.GetGuid();
        Console.WriteLine("Decorated generic: " + guid);
        return guid;
    }
}