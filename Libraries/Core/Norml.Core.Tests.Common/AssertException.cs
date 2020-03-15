using System;

namespace Norml.Core.Tests.Common
{
    public class AssertException : Exception
    {
        public AssertException()
        {
        }

        public AssertException(string message) : base(message)
        {
        }

        public AssertException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
