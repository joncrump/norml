using System;
using System.Collections.Generic;

namespace Norml.Tests.Common.Helpers
{
    public interface IAssertHelper
    {
        void AssertEquality<TValue>(TValue expected, TValue actual, IEnumerable<string> propertiesToIgnore = null, 
            IDictionary<string, object> additionalParameters = null, bool recurseObjectProperties = true);
        void AssertHasClassAttributes<TValue>(TValue value, IEnumerable<AttributeInfo> attributeInfos);
        void AssertHasPropertyAttributes<TValue>(TValue value, IEnumerable<PropertyAttributeInfo> attributeInfos);
        IExceptionAsserter AssertException<TException>(Action exceptionCallback) where TException : Exception;
    }
}
