using System;
using System.Linq.Expressions;
using Norml.Common.Exceptions;
using NUnit.Framework;
using Norml.Tests.Common.Base;

namespace Norml.Common.Data.Tests.ValueFactoryTests
{
    [TestFixture]
    public class TheGetValueFactoryMethod : MockTestBase<TestableValueFactory>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfKeyIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.GetValueFactory(null))
                .AndVerifyHasParameter("key");
        }

        [Test]
        public void WillThrowArgumentEmptyExceptionIfKeyIsEmpty()
        {
            Asserter
                .AssertException<ArgumentEmptyException>(
                    () => SystemUnderTest.GetValueFactory(String.Empty))
                .AndVerifyHasParameter("key");
        }

        [Test]
        public void WillReturnExpressionIfParameterIsNull()
        {
            Expression<Func<object, object>> expected = null;
            var key = String.Empty;

            expected = e => null;
            key = DataGenerator.GenerateString();

            SystemUnderTest.Delegates
                .Add(key, expected);
            var actual = SystemUnderTest.GetValueFactory(key);

            Asserter.AssertEquality("() => e => null", actual.ToString());
        }

        [Test]
        public void WillReturnExpressionWithParameter()
        {
            string expected = null;
            var key = String.Empty;
            var random = String.Empty;

            key = DataGenerator.GenerateString();
            expected = DataGenerator.GenerateString();
            random = DataGenerator.GenerateString();

            Expression<Func<object, string>> expression = e => e.ToString() + random;

            SystemUnderTest.Delegates
                .Add(key, expression);
            var returnExpression = SystemUnderTest.GetValueFactory(key, new ParameterInfo(typeof(string), expected));
            var actual = returnExpression.Compile()();

            Asserter.AssertEquality(expected + random, actual.ToString());
        }
    }
}