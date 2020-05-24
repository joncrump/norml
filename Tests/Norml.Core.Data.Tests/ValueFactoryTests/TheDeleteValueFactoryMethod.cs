using System;
using System.Linq.Expressions;
using Norml.Core.Data.Constants;
using Norml.Core.Exceptions;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Data.Tests.ValueFactoryTests
{
    [TestFixture]
    public class TheDeleteValueFactoryMethod : MockTestBase<TestableValueFactory>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfKeyIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.DeleteValueFactory(null))
                .AndVerifyHasParameter("key");
        }

        [Test]
        public void WillThrowArgumentEmptyExceptionIfKeyIsEmpty()
        {
            Asserter
                .AssertException<ArgumentEmptyException>(
                    () => SystemUnderTest.DeleteValueFactory(String.Empty))
                .AndVerifyHasParameter("key");
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfKeyDoesNotExist()
        {
            Expression<Func<object, object>> expression = e => null;

            SystemUnderTest.Delegates.Add(DataGenerator.GenerateString(), expression);
            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.DeleteValueFactory(DataGenerator.GenerateString()))
                .AndVerifyMessageContains(ErrorMessages.CannotDeleteExpression);
        }

        [Test]
        public void WillDeleteValueFactory()
        {
            var key = String.Empty;

            key = DataGenerator.GenerateString();
            Expression<Func<object, object>> expression = e => null;

            SystemUnderTest.Delegates.Add(key, expression);
            SystemUnderTest.DeleteValueFactory(key);

            Assert.IsFalse(SystemUnderTest.Delegates.ContainsKey(key));
        }
    }
}