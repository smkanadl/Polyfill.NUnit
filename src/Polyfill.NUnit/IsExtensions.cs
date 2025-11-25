using NUnit.Framework.Constraints;

namespace NUnit.Framework
{
    /// <summary>
    ///  Provides extension methods for the <see cref="Is"/> class to polyfill constraints.
    /// </summary>
    public static class IsExtensions
    {
        extension(Is)
        {
            /// <summary>
            ///  Returns a constraint that tests whether the actual array is equal to the expected array.
            /// </summary>
            /// <typeparam name="T">The type of the array elements.</typeparam>
            /// <param name="expected">The expected array.</param>
            /// <returns>An <see cref="EqualConstraint"/> for the expected array.</returns>
            public static EqualConstraint EqualTo<T>(T[] expected)
            {
                return new(expected);
            }
        }
    }
}
