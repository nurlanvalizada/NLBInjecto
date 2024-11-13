namespace NLBInjecto;

public class ScopeServiceCannotBeResolvedException() : InvalidOperationException("Cannot resolve scoped services from root provider");