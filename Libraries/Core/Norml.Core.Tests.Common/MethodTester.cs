using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Norml.Common;
using Norml.Common.Extensions;
using Norml.Tests.Common.Base;
using Norml.Tests.Common.Helpers;

namespace Norml.Tests.Common
{
    public class MethodTester : IMethodTester
    {
        private readonly IDataGenerator _dataGenerator;
        private int _nullIndex = 0;

        public MethodTester(IDataGenerator dataGenerator)
        {
            _dataGenerator = dataGenerator;
        }

        public void TestMethodParameters<TItemUnderTest>(string methodName, 
            IEnumerable<string> parametersToSkip = null)
            where TItemUnderTest : class
        {
            Guard.ThrowIfNullOrEmpty("methodName", methodName);
            
            var methodInfo = typeof(TItemUnderTest).GetMethod(methodName);

            if (methodInfo.IsNull())
            {
                throw new InvalidOperationException("The method {0} was not found."
                    .FormatString(methodName));
            }

            var parameters = methodInfo.GetParameters();

            TestParameters<TItemUnderTest>(parameters, methodInfo, parametersToSkip);
        }

        public void TestParameters<TItemUnderTest>(IEnumerable<ParameterInfo> parameters, 
            MethodInfo method, IEnumerable<string> parametersToSkip = null)
            where TItemUnderTest : class
        {
            if (parameters.IsNullOrEmpty())
            {
                return;
            }

            var failedParameters = new List<string>();
            var instance = ConstructInstance<TItemUnderTest>();

            foreach (var parameter in parameters)
            {
                var parameter1 = parameter;

                if (parametersToSkip
                    .Where(p => p.ToLower() == parameter1.Name.ToLower())
                    .IsNullOrEmpty())
                {
                    continue;
                }

                var failed = TestMethodInfoForNullParameter(method, instance);

                if (failed.IsNotNullOrEmpty())
                {
                    failedParameters.AddRange(failed
                        .Where(p => !failedParameters.Contains(p))
                        .ToList());
                }
            }

            if (failedParameters.IsNotNullOrEmpty())
            {
                throw new AssertException("{0}\r\n{1}".FormatString("The following parameters failed the null check:",
                    failedParameters.ToDelimitedString("\r\n")));
            }
        }

        public IEnumerable<object> InitializeMethodParameters(IList<ParameterInfo> parameters)
        {
            var instances = new List<object>();

            for (var index = 0; index < parameters.Count; index++)
            {
                if (index == _nullIndex)
                {
                    instances.Add(null);
                }
                else
                {
					AddInstance(parameters, instances, index);
                }
            }

            return instances;
        }

        public void AddInstance(IList<ParameterInfo> parameters, IList<Object> instances, int index)
		{
			var type = parameters[index].ParameterType;

			if (type == typeof(string))
			{
				instances.Add(_dataGenerator.GenerateString());
			}
			else
			{
				var mockType = typeof(Mock<>).MakeGenericType(parameters[index].ParameterType);

				var mock = Convert.ChangeType(Activator.CreateInstance(mockType,
																	   null), mockType);

				instances.Add(GetObjectFromMock(mock, parameters[index].ParameterType));
			}
		}

        public IEnumerable<string> TestMethodInfoForNullParameter(MethodInfo method, object instance)
        {
            var parameters = method.GetParameters().ToList();
            var failedParameters = new List<string>();

            for (var index = 0; index < parameters.Count; index++)
            {
                var argumentNullExceptionThrown = false;
                _nullIndex = index;
                var parameterInstances = InitializeMethodParameters(parameters);

                try
                {
                    method.Invoke(instance, parameterInstances.ToArray());
                }
                catch (Exception exception)
                {
                    var innerException = exception.InnerException as ArgumentNullException;

                    if (innerException.IsNull())
                    {
                        throw;
                    }

                    argumentNullExceptionThrown = true;
                }
                finally
                {
                    if (!argumentNullExceptionThrown)
                    {
                        failedParameters.Add(parameters[index].Name);
                    }
                }
            }

            return failedParameters;
        }

        public TItemUnderTest ConstructInstance<TItemUnderTest>()
            where TItemUnderTest : class
        {
            TItemUnderTest instance;

            var constructors = typeof(TItemUnderTest)
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            var parameterlessConstructor = constructors
                .FirstOrDefault(c => c.GetParameters().IsNullOrEmpty());

            if (parameterlessConstructor.IsNotNull())
            {
                instance = Activator.CreateInstance<TItemUnderTest>();    
            }
            else
            {
                var constructor = constructors.First();
                var parameters = constructor.GetParameters();
                var values = InitializeParameters(parameters);

                instance = (TItemUnderTest)Activator.CreateInstance(typeof(TItemUnderTest), values);
            }

            return instance;
        }

        public IEnumerable<object> InitializeParameters(IList<ParameterInfo> parameters)
        {
            var instances = new List<object>();

            foreach (ParameterInfo parameter in parameters)
            {
                var type = parameter.ParameterType;

                if (type == typeof(string))
                {
                    instances.Add(_dataGenerator.GenerateString());
                }
                else
                {
                    var mockType = typeof(Mock<>).MakeGenericType(parameter.ParameterType);

                    var mock = Convert.ChangeType(Activator.CreateInstance(mockType,
                        null), mockType);

                    instances.Add(GetObjectFromMock(mock, parameter.ParameterType));
                }
            }

            return instances;
        }

        public object GetObjectFromMock(object mock, Type propertyType)
        {
            var type = mock.GetType();

            var property = type.GetProperty("Object", propertyType);

            return property.GetValue(mock);
        }
    }
}
