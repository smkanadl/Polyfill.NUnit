# Polyfill.NUnit

## Purpose

Polyfill.NUnit lets you cross-target legacy frameworks (e.g. netstandard2.0, netcoreapp3.1, net5.0) while beginning
to use selected features that NUnit 4 introduces. NUnit 4 raises its minimum supported target frameworks
and will never support net5.0, so this library provides a transitional window:
Keep older TFMs in your multi-target matrix for maintenance while adopting newer, more expressive NUnit syntax.

## What it provides

Focused, source-compatible polyfills that mimic NUnit 4 surface area where:
* The API is additive (safe forward removal once you move fully to NUnit 4)
* Behavior can match existing NUnit 4 functionality exactly, but does not guarantee it
* NUnit analyzers (and general modernization efforts) encourage newer syntax (e.g. collection expressions) that would otherwise not compile on NUnit 3

Currently included:
* `Is.EqualTo<T>(T[])` generic array overload on `Is`, enabling collection expression usage: `Is.EqualTo([1, 2, 3])`.
* `Is.EquivalentTo<T>(T[])` generic array overload on `Is`, enabling collection expression usage: `Is.EquivalentTo([1, 2, 3])`.
* `using (Assert.EnterMultipleScope())` for multiple assertion scopes, superseding `Assert.Multiple`.

## Requirements

The test project must use at least C# 14 for the new extensions feature.

## Example

Use NUnit 4 like you usually would, even on legacy target frameworks!

``` csharp
using NUnit.Framework;

[TestFixture]
public class SampleTests
{
    [Test]
    public void ArrayEquality_UsesPolyfilledOverload()
    {
        var data = new[] { 1, 2, 3 };
        Assert.That(data, Is.EqualTo([1, 2, 3]));
    }
    
    [Test]
    public void ArrayEquivalence_UsesPolyfilledOverload()
    {
        var data = new[] { 1, 2, 3 };
        Assert.That(data, Is.EquivalentTo([1, 2, 3]));
    }

    [Test]
    public void MultipleAsserts_CanUseEnterMultipleScope()
    {
        var data = new[] { 1, 2, 3 };
        using (Assert.EnterMultipleScope())
        {
            Assert.That(data, Is.EqualTo([1, 2, 3]));
            Assert.That(data, Is.EquivalentTo([3, 2, 1]));
        }
    }
}
```

## Setup

Include the Polyfill.NUnit NuGet package in your test project that targets legacy frameworks.
This can either be done in the project file or a shared Directory.Build.props file.

``` csproj
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFrameworks>netcoreapp3.1;net5.0;net10.0</TargetFrameworks>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="NUnit" Version="4.4.0" />
    <!-- Other test dependencies like NUnit3TestAdapter, Microsoft.NET.Test.Sdk, etc. -->
  </ItemGroup>

  <ItemGroup>
  </ItemGroup>

  <!-- Include Polyfill.NUnit and NUnit 3 only for legacy target frameworks -->
  <ItemGroup Condition="'$(TargetFramework)'=='net5.0' Or '$(TargetFramework)'=='netcoreapp3.1'">
    <PackageReference Update="NUnit" Version="3.14" />
    <PackageReference Include="Polyfill.NUnit" Version="1.0.0" />
  </ItemGroup>

</Project>
```

## Credits

All credits for the original NUnit design, concepts, and implementation belongs to the NUnit team.
This project is unaffiliated and purely a compatibility aid and is very much slightly adopted code
from the original NUnit source.

https://github.com/nunit/nunit
