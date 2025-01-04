namespace NLBInjecto;

public class ScopeServiceCannotBeResolvedException(string serviceName) : InvalidOperationException($"Cannot resolve scoped service {serviceName} from root provider");