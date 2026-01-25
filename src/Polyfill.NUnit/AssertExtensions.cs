using NUnit.Framework.Internal;
using System;
using System.Reflection;
using System.Threading;

namespace NUnit.Framework
{
    /// <summary>
    ///  Provides extension methods for the <see cref="Assert"/> class to polyfill constraints.
    /// </summary>
    public static class AssertExtensions
    {
        extension(Assert)
        {
            /// <summary>
            ///  Creates and enters a new assertion scope that allows multiple assertions to be evaluated together.
            /// </summary>
            /// <returns>
            ///  An <see cref="IDisposable"/> that, when disposed, exits the assertion scope and reports any assertion failures.
            /// </returns>
            public static IDisposable EnterMultipleScope()
            {
                return new AssertionScope();
            }
        }
    }

    /// <summary>
    ///  This polyfill is more than less shamelessly copied from NUnit's own MultipleAssertScope implementation.
    /// </summary>
    /// <remarks>
    ///  https://github.com/nunit/nunit/blob/7109fdd26fb35e173b06eb396b58f802b322acb0/src/NUnitFramework/framework/Assert.cs#L263
    /// </remarks>
    sealed file class AssertionScope : IDisposable
    {
        private readonly TestExecutionContext _context;
        private readonly int _assertionCountWhenEnteringScope;
        private readonly int _multipleAssertLevelInScope;

        private int _isDisposed;

        public AssertionScope()
        {
            _context = TestExecutionContext.CurrentContext;
            if (_context is null)
            {
                throw new InvalidOperationException("There is no current test execution context.");
            }

            _assertionCountWhenEnteringScope = _context.CurrentResult.AssertionResults.Count;
            _multipleAssertLevelInScope = _context.IncrementMultipleAssertLevel();
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _isDisposed, 1) == 1)
            {
                return; // Already disposed.
            }

            if (TestExecutionContext.CurrentContext != _context ||
                _context.MultipleAssertLevel != _multipleAssertLevelInScope)
            {
                throw new InvalidOperationException("The assertion scope was disposed out of order.");
            }

            _context.DecrementMultipleAssertLevel();

            if (_context is { MultipleAssertLevel: 0, CurrentResult.PendingFailures: > 0 })
            {
                _context.CurrentResult.RecordTestCompletion();
                if (_context.CurrentResult.AssertionResults.Count > _assertionCountWhenEnteringScope)
                {
                    throw new MultipleAssertException(_context.CurrentResult);
                }
            }
        }
    }

    static file class TextExecutionContextExtensions
    {
        private static readonly PropertyInfo MultipleAssertLevelProperty =
            typeof(TestExecutionContext).GetProperty("MultipleAssertLevel", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;

        extension(TestExecutionContext ctx)
        {
            public int MultipleAssertLevel => (int)MultipleAssertLevelProperty.GetValue(ctx)!;

            public int IncrementMultipleAssertLevel()
            {
                var currentValue = ctx.MultipleAssertLevel;
                ++currentValue;
                ctx.SetMultipleAssertLevel(currentValue);
                return currentValue;
            }

            public int DecrementMultipleAssertLevel()
            {
                var currentValue = ctx.MultipleAssertLevel;
                --currentValue;
                ctx.SetMultipleAssertLevel(currentValue);
                return currentValue;
            }

            private void SetMultipleAssertLevel(int value)
                => MultipleAssertLevelProperty.SetValue(ctx, value);
        }
    }
}
