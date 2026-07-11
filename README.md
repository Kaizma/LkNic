# LkNic

LkNic is a small .NET library for parsing and validating Sri Lankan National Identity Card numbers.

This early alpha currently validates the basic NIC format and extracts the birth year.

## Install

```powershell
dotnet add package LkNic --prerelease
```

## Usage

```csharp
using LkNic;

var nic = LkNic.Parse("199912345678");

Console.WriteLine(nic.BirthYear); // 1999
```

## Supported Formats

- New NIC format: 12 digits, for example `200112345678`
- Old NIC format: 9 digits followed by `V`, `v`, `X`, or `x`, for example `752345678V`

## Current Result

```csharp
public class Nic
{
    public int BirthYear { get; init; }
}
```

## Notes

This package is still in alpha. More NIC details, such as birth date and gender, are planned for future versions.
