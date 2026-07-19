# LkNic

LkNic is a .NET library for parsing and validating Sri Lankan National Identity Card numbers.

This early alpha currently validates the basic NIC format and extracts the birth year, birth date, and gender.

## Install

```powershell
dotnet add package LkNic --prerelease
```

## Usage

```csharp
using LkNic;

var nic = LkNic.Parse("199912345678");

Console.WriteLine(nic.BirthYear); // 1999
Console.WriteLine(nic.BirthDate); // 1999-05-01
Console.WriteLine(nic.Gender);    // Male
```

For user input, use `TryParse` to avoid exceptions during normal validation:

```csharp
if (LkNic.TryParse("199912345678", out var parsedNic))
{
    Console.WriteLine(parsedNic.BirthDate);
}
```

## Supported Formats

- New NIC format: 12 digits, for example `200112345678`
- Old NIC format: 9 digits followed by `V`, `v`, `X`, or `x`, for example `752345678V`

## Current Result

```csharp
public class Nic
{
    public int BirthYear { get; init; }
    public DateOnly BirthDate { get; init; }
    public Gender Gender { get; init; }
}
```

## Notes

This package is still in alpha. The public API and validation rules may change before a stable release.
