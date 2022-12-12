using System;

namespace EasyClap.Seneca.Common.Core
{
    /// <summary>
    /// All exceptions thrown in any <c>EasyClap.Seneca</c> framework is a child of this type.
    /// </summary>
    public abstract class SenecaException : Exception
    {
        protected SenecaException(string message) : base(message) {}
        protected SenecaException(string message, Exception inner) : base(message, inner) {}
    }
}
