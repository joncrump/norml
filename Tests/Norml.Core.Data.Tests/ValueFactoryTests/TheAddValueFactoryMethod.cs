using System;
using System.Linq.Expressions;
using Norml.Common.Data.Constants;
using Norml.Common.Exceptions;
using NUnit.Framework;
using Norml.Tests.Common.Base;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Norml.Common.Data.Tests.ValueFactoryTests
{
    [TestFixture]
    public class TheAddValueFactoryMethod : MockTestBase<TestableValueFactory>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfKeyIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.AddValueFactory(null, e => new object()))
                .AndVerifyHasParameter("key");
        }

        [Test]
        public void WillThrowArgumentEmptyExceptionIfKeyIsEmpty()
        {
            Asserter
                .AssertException<ArgumentEmptyException>(
                    () => SystemUnderTest.AddValueFactory(String.Empty, e => new object()))
                .AndVerifyHasParameter("key");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfValueFactoryIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.AddValueFactory(DataGenerator.GenerateString(), (Expression<Action>) null))
                .AndVerifyHasParameter("valueFactory");
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfValueFactoryAlreadyExists()
        {
            var key = String.Empty;

            Expression<Func<object, object>> expression = e => null;
            key = DataGenerator.GenerateString();

            SystemUnderTest.Delegates.Add(key, expression);
            Asserter
                .AssertException<InvalidOperationException>(
                    () => SystemUnderTest.AddValueFactory(key, e => (Expression<Action>)null))
                .AndVerifyMessageContains(ErrorMessages.CannotAddExpression);
        }

        [Test]
        public void WillAddDelegate()
        {
            var key = String.Empty;

            key = DataGenerator.GenerateString();
            Expression<Func<object, object>> expression = e => null;

            SystemUnderTest.Delegates.Add(DataGenerator.GenerateString(), expression);
            Expression<Func<object, object>> param = e => DataGenerator.GenerateInteger(1, 5);

            SystemUnderTest.AddValueFactory(key, param);

            Assert.IsTrue(SystemUnderTest.Delegates.ContainsKey(key));
        }
    }
}
