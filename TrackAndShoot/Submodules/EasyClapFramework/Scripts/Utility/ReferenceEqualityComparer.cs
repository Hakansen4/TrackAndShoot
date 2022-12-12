using System.Collections.Generic;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.Utility
{
    /// <summary>
    /// Compares operands based solely on reference equality.
    /// </summary>
    /// <seealso cref="object.ReferenceEquals"/>
    [PublicAPI]
    public class ReferenceEqualityComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// A default, cached instance of this generic variant of the reference equality comparer.
        /// </summary>
        public static readonly ReferenceEqualityComparer<T> Default = new ReferenceEqualityComparer<T>();

        /// <summary>
        /// Returns true if the object references are equal.
        /// </summary>
        public bool Equals(T a, T b)
        {
            return ReferenceEquals(a, b);
        }

        /// <summary>
        /// Returns the result of the object's own GetHashCode method.
        /// </summary>
        public int GetHashCode(T obj)
        {
            // TODO: Can we use 'RuntimeHelpers.GetHashCode(object)'?
            return obj.GetHashCode();
        }
    }
}
