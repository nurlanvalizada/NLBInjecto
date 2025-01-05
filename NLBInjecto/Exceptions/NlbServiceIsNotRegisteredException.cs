namespace NLBInjecto.Exceptions;

public class NlbServiceIsNotRegisteredException(string serviceName, string? registeredKey = null) 
    : Exception($"Service {serviceName} with {registeredKey} is not registered");