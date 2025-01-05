using NLBInjecto.Exceptions;

namespace NLBInjecto;

public static class NlbServiceCollectionExtensions
{
    public static NlbServiceDescriptor GetServiceDescriptor(this IReadOnlyList<NlbServiceDescriptor> services, Type serviceType, string? name = null)
    {
        var descriptor = services.FirstOrDefault(s => s.ServiceType == serviceType && s.Name == name);
        if(descriptor == null && serviceType.IsGenericType)
        {
            // If no direct match is found and the requested service is a closed generic,
            // look for an open generic definition that matches the generic type definition.
            var genericDefinition = serviceType.GetGenericTypeDefinition();
            descriptor = services.FirstOrDefault(s => s.ServiceType.IsGenericTypeDefinition &&
                                                      s.ServiceType.GetGenericTypeDefinition() == genericDefinition);
        }

        return descriptor ?? throw new NlbServiceIsNotRegisteredException(serviceType.Name, name);
    }
}