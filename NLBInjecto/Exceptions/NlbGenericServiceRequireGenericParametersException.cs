namespace NLBInjecto.Exceptions;

public class NlbGenericServiceRequireGenericParametersException(string serviceName) 
    : Exception($"Generic type arguments required for service {serviceName}");