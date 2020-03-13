using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using Norml.Common;
using Norml.Common.Extensions;
using Norml.Tests.Common.Helpers;

namespace Norml.Tests.Common.Base
{
    public abstract class MockTestBase<TItemUnderTest> : TestBase where TItemUnderTest : class
    {
        protected TItemUnderTest SystemUnderTest { get; set; }
        protected PropertyBag Mocks { get; set; }

        protected MockTestBase() : this(AsserterFactory.GetAssertAdapter(UnitTestFrameworkType.Nunit))
        {
            Setup();
        }
        
        protected MockTestBase(UnitTestFrameworkType frameworkType) : this(AsserterFactory.GetAssertAdapter(frameworkType))
        {
            Setup();
        }
        
        protected MockTestBase(IAssertAdapter assertAdapter)
            : base(new RandomDataGenerator(), new ReflectionBasedObjectCreator(), assertAdapter, 
            new SurrogateAsserter(assertAdapter))
        {
            Setup();
        }

        protected MockTestBase(IDataGenerator dataGenerator, IAssertAdapter assertAdapter)
            : base(dataGenerator, new ReflectionBasedObjectCreator(), assertAdapter, 
            new SurrogateAsserter(assertAdapter))
        {
            Setup();
        }

        protected MockTestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator, 
            IAssertAdapter assertAdapter)
            : base(dataGenerator, objectCreator, assertAdapter)
        {
            Setup();
        }

        protected MockTestBase(IObjectCreator objectCreator, IAssertAdapter assertAdapter)
            : base(new RandomDataGenerator(), objectCreator, assertAdapter, 
            new SurrogateAsserter(assertAdapter))
        {
            Setup();
        }

        protected override void Setup()
        {
            base.Setup();

            Mocks = new PropertyBag();
            BuildSystemUnderTest();
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            SystemUnderTest = null;
            Mocks = null;
        }

        private void BuildSystemUnderTest()
        {
            var type = typeof(TItemUnderTest);

            if (!type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Where(c => c.GetParameters().IsNullOrEmpty()).IsNullOrEmpty())
            {
                CreateInstanceWithConstructorNoParameters();
            }
            else
            {
                CreateInstanceWithConstructorParameters(type);
            }
        }

        private void CreateInstanceWithConstructorNoParameters()
        {
            SystemUnderTest = (TItemUnderTest)Activator.CreateInstance(typeof(TItemUnderTest), null);
        }

        private void CreateInstanceWithConstructorParameters(Type type)
        {
            var values = new List<object>();

            var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                                  .ToSafeList().First(c => !c.GetParameters().IsNullOrEmpty());

            foreach (var parameter in constructor.GetParameters())
            {
                if (Mocks.HasValue(parameter.Name))
                {
                    continue;
                }

                var parameterType = parameter.ParameterType;
                object value = null;

                if (parameter.ParameterType == typeof(string))
                {
                    value = DataGenerator.GenerateString();
                }
                else
                {
                    parameterType = typeof(Mock<>).MakeGenericType(parameter.ParameterType);

                    var mock = Convert.ChangeType(Activator.CreateInstance(parameterType,
                                                                           null), parameterType);

					var method = Mocks.GetType().GetMethod("Add");
					var genericMethod = method.MakeGenericMethod(parameter.ParameterType);

					genericMethod.Invoke(Mocks, new object[] { parameter.Name, mock });

                    value = GetObjectFromMock(mock, parameter.ParameterType);
                }

                values.Add(value);
            }

            SystemUnderTest = (TItemUnderTest)Activator.CreateInstance(typeof(TItemUnderTest), values.ToArray());
        }
    }
}
