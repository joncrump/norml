using System;

namespace Norml.Core.Exceptions
{
    public class ArgumentInvalidException : ArgumentException
    {
         public ArgumentInvalidException(string parameterName) : base(null, parameterName)
        {
        }

        public ArgumentInvalidException(string message, string parameterName) : base(message, parameterName)
        {
        }
    }
}
