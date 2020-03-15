using System;

namespace Norml.Core.Exceptions
{
    public class ArgumentEmptyException : ArgumentException
    {
        public ArgumentEmptyException(string parameterName) : base(null, parameterName)
        {
        }

        public ArgumentEmptyException(string message, string parameterName) : base(message, parameterName)
        {
        }
    }
}
