using System;
using System.Collections.Generic;
using Norml.Common.Extensions;
using Norml.Tests.Common.Helpers;
using Norml.Tests.Common.Helpers.Strategies;

namespace Norml.Tests.Common.Base
{
    public abstract class TestBase : UtilityBase
    {
        private readonly IAssertAdapter _assertAdapter;

        protected IMethodTester MethodTester { get; set; }
        protected IObjectCreator ObjectCreator { get; set; }
        protected IConstructorTester ConstructorTester { get; set; }
        protected IAssertHelper Asserter { get; set; }
        
        protected TestBase() : this(new RandomDataGenerator())
        {
        }

        protected TestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator = null, IAssertAdapter assertAdapter = null, 
            IAssertHelper assertHelper = null, IMethodTester methodTester = null, IConstructorTester constructorTester = null)
            : base(dataGenerator)
        {
            ObjectCreator = objectCreator ?? new ReflectionBasedObjectCreator();
            _assertAdapter = assertAdapter ?? new NUnitAssertAdapter();
            Asserter = assertHelper ?? new SurrogateAsserter(_assertAdapter, new AsserterStrategyFactory(_assertAdapter));
            MethodTester = methodTester ?? new MethodTester(dataGenerator);
            ConstructorTester = constructorTester ?? new ConstructorTester(dataGenerator);
        }

        public IExceptionAsserter AssertException<TException>(Action exceptionCallback)
            where TException : Exception
        {
            return Asserter.AssertException<TException>(exceptionCallback);
        }

        public void AssertIsNullOrNot<T>(T expectedValue, T actualValue, Action<T, T> assertDelegate = null)
            where T : class
        {
            if (expectedValue == null)
            {
                _assertAdapter.IsNull(actualValue);
                return;
            }

            _assertAdapter.IsNotNull(actualValue);

            assertDelegate?.Invoke(expectedValue, actualValue);
        }

        protected virtual void Setup()
        {
        }

        protected virtual void CleanUp()
        {
        }

        protected void IgnoreException<TException>(Action action, string expectedMessage = null)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                if (exception.GetType() != typeof(TException))
                {
                    throw;
                }

                if (expectedMessage.IsNullOrEmpty())
                {
                    return;
                }

                if (String.Compare(expectedMessage, exception.Message, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return;
                }

                throw;
            }
        }

        protected void DoConstructorTests<TItem>() where TItem : class
        {
            ConstructorTester.TestConstructorsForNullParameters<TItem>();
        }

        protected void DoMethodTests<TItem>(string methodName, IEnumerable<string> parametersToSkip = null) where TItem : class
        {
            MethodTester.TestMethodParameters<TItem>(methodName, parametersToSkip);
        }

        protected IEnumerable<T> CreateEnumerableOfItems<T>(int numberOfItems = 5) where T : class, new()
        {
            return CreateEnumerableOfItems(() => ObjectCreator.CreateNew<T>(), numberOfItems);
        }
    }

    public abstract class TestBase<TItemUnderTest> : TestBase where TItemUnderTest : class, new()
    {
        protected TItemUnderTest ItemUnderTest { get; private set; }

        protected TestBase() : this(new RandomDataGenerator())
        {
        }

        protected TestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator = null, IAssertAdapter assertAdapter = null, 
            IAssertHelper assertHelper = null, IMethodTester methodTester = null, IConstructorTester constructorTester = null)
            : base(dataGenerator, objectCreator, assertAdapter, assertHelper, methodTester, constructorTester)
        {
        }

        protected override void Setup()
        {
            base.Setup();

            ItemUnderTest = new TItemUnderTest();
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            ItemUnderTest = null;
        }
    }
}