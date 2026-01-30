
[![Build & Test](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/build.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/build.yml)
[![Build & Release](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/release.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/release.yml)
[![Build & Nuget Publish](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/nuget.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/nuget.yml)
[![Release](https://img.shields.io/github/v/release/elminalirzayev/Easy.Tools.Result)](https://github.com/elminalirzayev/Easy.Tools.Result/releases)
[![License](https://img.shields.io/github/license/elminalirzayev/Easy.Tools.Result)](https://github.com/elminalirzayev/Easy.Tools.Result/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/Easy.Tools.Result.svg)](https://www.nuget.org/packages/Easy.Tools.Result)


# Easy.Tools.Result

## Overview
**Easy.Tools.Result** is a lightweight, high-performance, and enterprise-ready .NET library that implements the **Result Pattern**. It helps you write cleaner, more robust code by replacing exceptions with typed return values for flow control.

It supports both **Imperative** (`if (result.IsFailure)`) and **Functional** (`result.Match(...)`) programming styles.

## Features
* ** Zero-Allocation:** Optimized `Success()` results are cached to minimize memory pressure (GC friendly).
* ** Functional Extensions:** Includes `Match`, `Map`, `Tap`, and `Ensure` for fluent chaining.
* ** Type-Safe:** No more `null` checks or magic strings.
* ** Immutable:** Thread-safe and predictable behavior.
* ** Implicit Conversions:** Return `Error` or `Value` directly without verbose syntax.
* ** Multi-Target:** Supports `.NET 10`, `.NET 8`, `.NET 6`, `.NET Standard 2.0/2.1`, and `.NET Framework 4.7.2+`.

## Installation

Install via NuGet Package Manager:

```bash
Install-Package Easy.Tools.Result
```

Or via .NET CLI:

```bash
dotnet add package Easy.Tools.Result
```

## Usage Example

### 1. Defining Errors

Instead of throwing exceptions, define your domain errors statically.

```csharp
public static class DomainErrors
{
    public static readonly Error UserNotFound = new("User.NotFound", "The user with the specified ID was not found.");
    public static readonly Error InvalidEmail = new("User.InvalidEmail", "The email format is invalid.");
}
```

### 2. Returning a Result

Refactor your services to return Result<T>. You can use Implicit Conversions for cleaner code.

```csharp
public Result<User> GetUserById(int id)
{
    var user = _repository.Find(id);

    if (user is null)
    {
        // Implicitly converts Error to Result<User>
        return DomainErrors.UserNotFound; 
    }

    // Implicitly converts User to Result<User>
    return user;
}
```

### 3. Handling the Result

#### Option A: Imperative Style (Traditional)

Use properties to control flow explicitly.
```csharp
[HttpGet("{id}")]
public IActionResult GetUser(int id)
{
    Result<User> result = _userService.GetUserById(id);

    if (result.IsFailure)
    {
        return BadRequest(new { code = result.Error.Code, message = result.Error.Message });
    }

    return Ok(result.Value);
}
```
#### Option B: Functional Style (Fluent & Clean)

Use `Match` to handle success and failure in a single expression.

```csharp
[HttpGet("{id}")]
public IActionResult GetUser(int id)
{
    return _userService.GetUserById(id)
        .Match(
            onSuccess: user => Ok(user),
            onFailure: error => BadRequest(new { error.Code, error.Message })
        );
}
```

#### Option C: Chaining (Advanced)

Use Tap for side effects (logging) and Map for transformation.

```csharp
public IActionResult GetUserDto(int id)
{
    return _userService.GetUserById(id)
        .Tap(user => _logger.LogInformation($"User found: {user.Name}")) // Side-effect (Logging)
        .Map(user => new UserDto(user.Name, user.Email))                // Transformation
        .Match(
            onSuccess: dto => Ok(dto),
            onFailure: error => BadRequest(error)
        );
}
```

## Advanced Features

### Deconstruction

You can deconstruct the result into a tuple, similar to Go or Swift.

```csharp
var (isSuccess, error) = _userService.DeleteUser(id);
if (!isSuccess)
{
    Console.WriteLine($"Error: {error.Code}");
    return;
}
```

### Operator Overloading

You can compare errors directly.

```csharp
if (result.Error == DomainErrors.UserNotFound)
{
    // Handle specific error case
}
```


## Why Use This Pattern?

-   **Exceptions are for Exceptional Circumstances:** A user not being found or a validation error is _not_ an exception; it's a valid business scenario.
    
-   **Performance:** Throwing exceptions is expensive in .NET (stack trace generation). Returning a `Result` object is essentially free, especially with our **Zero-Allocation** optimizations.
    
-   **Readability:** `Result<User>` explicitly tells the developer "This operation might fail", whereas returning `User` lies (it might be null or throw).
    

---

## Contributing

Contributions and suggestions are welcome. Please open an issue or submit a pull request.

---

## License

This project is licensed under the MIT License.

---

  2025 Elmin Alirzayev / Easy Code Tools
