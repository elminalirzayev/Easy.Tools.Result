
[![Build & Test](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/build.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/build.yml)
[![Build & Release](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/release.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/release.yml)
[![Build & Nuget Publish](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/nuget.yml/badge.svg)](https://github.com/elminalirzayev/Easy.Tools.Result/actions/workflows/nuget.yml)
[![Release](https://img.shields.io/github/v/release/elminalirzayev/Easy.Tools.Result)](https://github.com/elminalirzayev/Easy.Tools.Result/releases)
[![License](https://img.shields.io/github/license/elminalirzayev/Easy.Tools.Result)](https://github.com/elminalirzayev/Easy.Tools.Result/blob/master/LICENSE.txt)
[![NuGet](https://img.shields.io/nuget/v/Easy.Tools.Result.svg)](https://www.nuget.org/packages/Easy.Tools.Result)


# Easy.Tools.Result

## Overview
**Easy.Tools.Result** is a lightweight, professional .NET library that implements the **Result Pattern**. It helps you write cleaner, more robust code by replacing exceptions with typed return values for flow control.

## Features
* **Type-Safe:** No more `null` checks or magic strings.
* **Expressive:** Clearly distinguish between Success and Failure.
* **Immutable:** Thread-safe and predictable.
* **Standardized:** Consistent error handling across your entire application.
* **Multi-Target:** Supports `.NET Standard 2.0`,`.NET Standard 2.1`, `.NET 8.0`, `.NET 10.0`, `.NET 6.0`, `.net48`, `.net47`.

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

Instead of throwing exceptions, define your domain errors:

```csharp
public static class DomainErrors
{
    public static readonly Error UserNotFound = new("User.NotFound", "The user with the specified ID was not found.");
    public static readonly Error InvalidEmail = new("User.InvalidEmail", "The email format is invalid.");
}
```

### 2. Returning a Result

Refactor your services to return `Result<T>` instead of raw objects.

```csharp
public Result<User> GetUserById(int id)
{
    var user = _repository.Find(id);

    if (user is null)
    {
        // Return a clean failure object
        return Result.Failure<User>(DomainErrors.UserNotFound);
    }

    // Return success with data
    return Result.Success(user);
}
```

### 3. Handling the Result (The Clean Way)

Use the properties to control flow in your Controllers or Managers.
```csharp
[HttpGet("{id}")]
public IActionResult GetUser(int id)
{
    Result<User> result = _userService.GetUserById(id);

    if (result.IsFailure)
    {
        // Return 400 or 404 based on the error
        return BadRequest(new { code = result.Error.Code, message = result.Error.Message });
    }

    return Ok(result.Value);
}
```

---

## Why Use This Pattern?

-   **Exceptions are for Exceptional Circumstances:** User not found or validation error is _not_ an exception; it's a valid business scenario.
    
-   **Performance:** Throwing exceptions is expensive in .NET. Returning a `Result` object is essentially free.
    
-   **Readability:** `Result<User>` tells the developer "This might fail", whereas `User` lies (it might be null or throw).

 ---

## Contributing
Contributions and suggestions are welcome. Please open an issue or submit a pull request.

---

## License
MIT License

---

© 2025 Elmin Alirzayev / Easy Code Tools
