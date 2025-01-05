# NlbInjecto - Custom Dependency Injection Container

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

NlbInjecto is a **lightweight**, **extensible**, and **testable** dependency injection (DI) container for .NET applications. Built from scratch to help you **understand** DI internals, **customize** registrations, and **extend** behavior beyond the built-in .NET DI framework.

## Table of Contents

- [NlbInjecto - Custom Dependency Injection Container](#nlbinjecto---custom-dependency-injection-container)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Motivation](#motivation)
  - [Installation](#installation)
  - [Getting Started](#getting-started)
    - [Basic Registration \& Resolution](#basic-registration--resolution)
    - [Open Generic Registration](#open-generic-registration)
    - [Decorators](#decorators)
    - [Scopes](#scopes)
    - [Keyed Services](#keyed-services)
  - [Contributing](#contributing)
    - [How to Contribute](#how-to-contribute)
  - [License](#license)

---

## Features

- **Open Generic Support**  
  Register types like `IGenericService<T>` -> `GenericService<T>` and let the container resolve any closed generic at runtime.

- **Decorator Support**  
  Easily wrap services with decorators (e.g., logging, caching, or cross-cutting concerns) for both non-generic and open-generic services.

- **Multiple Lifetimes**  
  Supports Singleton, Scoped, and Transient lifetimes. Control exactly how and when your services are instantiated.

- **Named Registrations**  
  Optionally register services with a name to allow multiple implementations of the same interface.

- **Resolving service with most parametered contructor**
  When resolving services NLBInjecto takes contructor with most parameters

- **Testable**  
  Includes comprehensive unit tests demonstrating behavior and best practices.

---

## Motivation

My motivation for built such repository is to show how we can build complex systems step by step. Maybe this is not a complete solution but it offer most of the functionality which modern DI containers offer. 

---

## Installation

1. Clone or download the repository.
2. Open the solution in Visual Studio or JetBrains Rider.
3. Build the project. You’re set to include this library in your application.

*(Alternatively, it is available as a NuGet package. You can add it to your projects like this `dotnet add package NLBInjecto`.)*

---

## Getting Started

### Basic Registration & Resolution

```csharp
var services = new NlbServiceCollection();

// Register: IMyService -> MyService
services.AddScoped<IMyService, MyService>();

// Build the provider
var provider = services.BuildServiceProvider();

// Resolve
var myService = provider.GetService<IMyService>();
myService.DoWork();
```

### Open Generic Registration

```csharp
var services = new NlbServiceCollection();

// Register open generic: IGenericService<T> -> GenericService<T>
services.AddSingleton(typeof(IGenericService<>), typeof(GenericService<>));

var provider = services.BuildServiceProvider();

// Resolve closed generics
var intService = provider.GetService<IGenericService<int>>();
var stringService = provider.GetService<IGenericService<string>>();

Console.WriteLine(intService.GetValue());
Console.WriteLine(stringService.GetValue());
```

### Decorators

```csharp
var services = new NlbServiceCollection();

// Base registration
services.AddScoped<IOrderService, OrderService>();

// Decorator registration
services.AddDecorator<IOrderService, OrderServiceLoggingDecorator>();

var provider = services.BuildServiceProvider();

var orderService = provider.GetService<IOrderService>(); 
// -> Actually returns OrderServiceLoggingDecorator 
//    that wraps the real OrderService
```

And for **open generics**:
```csharp
services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
services.AddDecorator(typeof(IGenericService<>), typeof(GenericServiceDecorator<>));
```

### Scopes

```csharp
var services = new NlbServiceCollection();
services.AddScoped<IMyService, MyService>();

var provider = services.BuildServiceProvider();

// Create first scope
using (var scopeA = provider.CreateScope())
{
    var serviceA1 = scopeA.ServiceProvider.GetService<IMyService>();
    var serviceA2 = scopeA.ServiceProvider.GetService<IMyService>();
    // serviceA1 == serviceA2 (same scope, same instance if scoped)
}

// Create second scope
using (var scopeB = provider.CreateScope())
{
    var serviceB1 = scopeB.ServiceProvider.GetService<IMyService>();
    // serviceB1 != serviceA1 (different scope, new instance)
}
```

### Keyed Services

```csharp
var services = new NlbServiceCollection();

services.AddTransient<ITransientService, TransientService>("test");
services.AddDecorator<ITransientService, TransientServiceDecorator>();

var provider = services.BuildServiceProvider();

var transientService = serviceProvider.GetService<ITransientService>("test");
// transientService here is TransientService
```

## Contributing

**Contributions are welcome!** I’d love to see **pull requests** for:

- New features (e.g., advanced constructor injection, property injection).  
- Bug fixes if you find any.  
- Enhancements to documentation or samples.  
- Performance improvements.

### How to Contribute

1. **Fork** this repository and **clone** it locally.  
2. Create a new **branch** for your feature or fix.  
3. **Commit** your changes with clear messages.  
4. Push the branch to your **fork**.  
5. Open a **Pull Request** from your fork’s branch to this repository’s `main` branch.  
6. Once reviewed, I’ll **merge** your changes.

Feel free to open an **issue** if you find a bug or have a feature request.


## License
This project is licensed under the MIT License. You are free to use it in both commercial and open-source software.