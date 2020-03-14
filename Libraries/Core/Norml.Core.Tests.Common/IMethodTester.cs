using System;
using System.Collections.Generic;
using System.Reflection;

namespace Norml.Tests.Common
{
    public interface IMethodTester
    {
        void TestMethodParameters<TItemUnderTest>(string methodName, 
            IEnumerable<string> parametersToSkip = null)
            where TItemUnderTest : class;
        void TestParameters<TItemUnderTest>(IEnumerable<ParameterInfo> parameters, 
            MethodInfo method, IEnumerable<string> parametersToSkip = null)
            where TItemUnderTest : class;
        IEnumerable<object> InitializeMethodParameters(IList<ParameterInfo> parameters);
        void AddInstance(IList<ParameterInfo> parameters, IList<Object> instances, int index);
        IEnumerable<string> TestMethodInfoForNullParameter(MethodInfo method, object instance);
        TItemUnderTest ConstructInstance<TItemUnderTest>()
            where TItemUnderTest : class;
        IEnumerable<object> InitializeParameters(IList<ParameterInfo> parameters);
        object GetObjectFromMock(object mock, Type propertyType);
    }
}