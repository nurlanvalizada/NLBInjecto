using NLBInjecto.Exceptions;

namespace NLBInjecto;

internal static class InstanceCreatorHelper
{
    public static object CreateInstance(Type implementationType, Func<Type, string?, object> serviceFactory, Type[]? genericArguments = null)
    {
        if(implementationType.IsGenericTypeDefinition)
        {
            if(genericArguments == null || genericArguments.Length == 0)
                throw new NlbGenericServiceRequireGenericParametersException(implementationType.Name);

            implementationType = implementationType.MakeGenericType(genericArguments);
        }

        var constructors = implementationType.GetConstructors().OrderByDescending(c => c.GetParameters().Length);
        foreach(var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            var parameterInstances = new object[parameters.Length];

            bool canResolveAllParameters = true;
            for(int i = 0; i < parameters.Length; i++)
            {
                try
                {
                    parameterInstances[i] = serviceFactory(parameters[i].ParameterType, null);
                }
                catch(NlbScopedServiceCannotBeResolvedException)
                {
                    throw;
                }
                catch(Exception exc)
                {
                    Console.WriteLine("\r\n Exception info: " + exc.Message + "\r\n");
                    canResolveAllParameters = false;
                    break;
                }
            }

            if(canResolveAllParameters)
            {
                return Activator.CreateInstance(implementationType, parameterInstances)!;
            }
        }

        throw new Exception($"Cannot resolve parameters for {implementationType.Name}");
    }
}