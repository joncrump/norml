using System;
using Norml.Core.Helpers;
using Norml.Core.Tests.Common.Base;
using NUnit.Framework;

namespace Norml.Core.Tests.Helpers.HashHelperTests
{
    [TestFixture]
    public class TheGenerateHashMethod : MockTestBase<HashHelper>
    {
        [Test]
        public void WillThrowArgumentNullExceptionIfValueIsNull()
        {
            Asserter
                .AssertException<ArgumentNullException>(
                    () => SystemUnderTest.GenerateHash(null))
                .AndVerifyHasParameter("value");
        }

        [Test]
        public void WillReturnHashIfValueIsHashable()
        {
            var actual = SystemUnderTest.GenerateHash(new HashableTestFixture());

            Asserter.AssertEquality("TestingHash", actual);
        }

        [Test]
        public void WillReturnHashCodeOfObject()
        {
            string value = null;
            string expected = null;

            value = DataGenerator.GenerateString();
            expected = value.GetHashCode().ToString();
            var actual = SystemUnderTest.GenerateHash(value);

            Asserter.AssertEquality(expected, actual);
        }

        public class HashableTestFixture : IHashable
        {
            public string Hash
            {
                get { return "TestingHash"; }
            }
        }
    }
}
