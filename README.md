# Polyfill.NUnit

## Purpose

Polyfill.NUnit lets you cross-target legacy frameworks (e.g. netcoreapp3.1, net5.0) while beginning
to use selected features that NUnit 4 introduces. NUnit 4 raises its minimum supported target frameworks
and will never support net5.0, so this library provides a transitional window:
keep older TFMs in your multi-target matrix for maintenance while adopting newer, more expressive NUnit syntax.

## What it provides

Focused, source-compatible polyfills that mimic NUnit 4 surface area where:
- The API is additive (safe forward removal once you move fully to NUnit 4)
- Behavior can match existing NUnit 4 functionality exactly, but does not guarantee it
- NUnit analyzers (and general modernization efforts) encourage newer syntax (e.g. collection expressions) that would otherwise not compile on NUnit 3

Currently included:
- `Is.EqualTo<T>(T[])` generic array overload via a C# 14 extension on `Is`, enabling collection expression usage: `Is.EqualTo([1, 2, 3])`.
- `using (Assert.EnterMultipleScope())` for multiple assertion scopes.

## Example

```csharp
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

## Credits

All credits for the original NUnit design, concepts, and implementation belongs to the NUnit team.
This project is unaffiliated and purely a compatibility aid and is very much slightly adopted code
from the original NUnit source.

NUnit GitHub: https://github.com/nunit/nunit

## License
MIT (see LICENSE file).
