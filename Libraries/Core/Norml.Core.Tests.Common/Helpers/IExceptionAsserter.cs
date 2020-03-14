using System;

namespace Norml.Tests.Common.Helpers
{
    public interface IExceptionAsserter
    {
        Exception CaughtException { get; }
        IExceptionAsserter AssertException<TException>(Action exceptionCallback) where TException : Exception;
        IExceptionAsserter AndVerifyMessageContains(string message);
        IExceptionAsserter AndVerifyHasParameter(string parameterName);
    }
}
