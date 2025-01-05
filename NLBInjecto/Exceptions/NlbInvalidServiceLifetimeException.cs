namespace NLBInjecto.Exceptions;

public class NlbInvalidServiceLifetimeException(NlbServiceLifetime lifetime) 
    : Exception($"{lifetime} is not a valid service lifetime.");