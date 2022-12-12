using System;

namespace EasyClap.Seneca.Common.Core
{
    /// <summary>
    /// All exceptions thrown in <c>EasyClap.Seneca.Common</c> is either this exact type, or a child of this type.
    /// </summary>
    public class SenecaCommonException : SenecaException
    {
        internal SenecaCommonException(string message) : base(message) {}
        internal SenecaCommonException(string message, Exception inner) : base(message, inner) {}

        internal static SenecaCommonException FromArgumentOutOfRange(string paramName, object paramValue, string details = null)
        {
            var message = $"{paramName} is out of range with a value of {paramValue}";

            if (!string.IsNullOrWhiteSpace(details))
            {
                message += $"\nDetails: {details}";
            }

            return new SenecaCommonException(message, new ArgumentOutOfRangeException(paramName, paramValue, message));
        }

        internal static SenecaCommonException FromNotSupported(string message)
        {
            return new SenecaCommonException(message, new NotSupportedException(message));
        }
    }
}
