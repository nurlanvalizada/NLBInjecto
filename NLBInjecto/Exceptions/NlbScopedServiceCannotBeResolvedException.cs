namespace NLBInjecto.Exceptions;

public class NlbScopedServiceCannotBeResolvedException(string serviceName)
    : Exception($"Cannot resolve scoped service {serviceName} from root provider");